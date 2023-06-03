using CBAccountIntake.Models;
using System.IO;
using System.Runtime.CompilerServices;

namespace CBAccountIntake
{
    internal class Program
    {
        internal static Settings Settings { get; set; }

        static Program() { }

        public Program() { }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            GetFileInfo(args);

            if (FileOperations())
            {
                Console.WriteLine();
            }
        }

        public static void GetFileInfo(string[] args)
        {
            Settings = new Settings();
            Settings.LoadSettings(args);
        }

        public static bool FileOperations()
        {
            List<FileData> customerInfo = new List<FileData>();
            FileData? customer;

            try
            {
                //first, check to see if file exists
                FileInfo fi = new FileInfo(Settings?.FilePath);

                var file = fi.FullName;

                if (!File.Exists(file))
                {
                    //file does not exist
                    return false;
                }
                else
                {
                    var fileData = File.ReadAllLines(file);

                    for (int i = 1; i < fileData.Length; i++)
                    {
                        customer = new FileData();
                        var data = fileData[i].Split(',');

                        if (data.Length > 0)
                        {
                            customer.Parse(data);
                            customerInfo.Add(customer);
                        }
                    }

                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}