using CBAccountIntake.Models;
using System.Data;
using CodeBlackFinance;

namespace CBAccountIntake
{
    internal class Program
    {
        internal static Settings? Settings { get; set; }
        internal static ICodeBlackTables? CBData { get; set; }
        internal static IDataAccess? DataAccess { get; set; }
        internal static ICBVerification? Verification { get; set; }

        public Program() { }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            GetFileInfo(args);
            var dt = FileOperations();
            if (dt == null)
            {
                //say there's an error
                Console.WriteLine();
            }
            else
            {
                CommitData(dt);
            }
        }

        public static void GetFileInfo(string[] args)
        {
            Settings = new Settings();
            Settings.LoadSettings(args);
            DataAccess = new DataAccess();
            CBData = new CodeBlackTables();
            Verification = new CBVerification();
        }

        public static DataTable? FileOperations()
        {
            List<FileData> customerInfo = new List<FileData>();
            FileData? customer;

            try
            {
                //first, check to see if file exists
                FileInfo fi = new FileInfo(Settings?.FilePath);
                DataTable? dt = CBData?.CreateTempDataTable();

                var file = fi.FullName;

                if (!File.Exists(file))
                {
                    //file does not exist
                    return null;
                }
                else
                {
                    CBData = new CodeBlackTables();


                    var fileData = File.ReadAllLines(file);

                    for (int i = 1; i < fileData.Length; i++)
                    {
                        customer = new FileData();
                        var data = fileData[i].Split(',');

                        if (data.Length > 0)
                        {
                            customer.Parse(data);

                            if (Verification.CardValidated(customer.AccountNumber))
                                CBData.PopulateDataTable(dt, customer);
                        }
                    }

                    Console.WriteLine();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool CommitData(DataTable? dt)
        {
            try
            {
                if (!DataAccess.CreateTempTable())
                {
                    return false;
                }
                else
                {
                    bool success = DataAccess.BulkCopyDataTable(dt);
                    success = success && DataAccess.InsertAccounts();
                    success = success && DataAccess.InsertMembers();
                    DataAccess.DropTempTable();
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}