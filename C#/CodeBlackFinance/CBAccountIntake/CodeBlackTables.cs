using CBAccountIntake.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAccountIntake
{
    public interface ICodeBlackTables
    {
        DataTable? CreateTempDataTable();
        void PopulateDataTable(DataTable? dt, FileData data);
    }

    public class CodeBlackTables : ICodeBlackTables
    {

        public DataTable? CreateTempDataTable()
        {
            var dt = new DataTable(@"TempData");
            dt.Columns.Add("AccountNumber", typeof(string));
            dt.Columns.Add("ExpDate", typeof(string));
            dt.Columns.Add("CardID", typeof(string));
            dt.Columns.Add("FName", typeof(string));
            dt.Columns.Add("LName", typeof(string));
            dt.Columns.Add("BAddress1", typeof(string));
            dt.Columns.Add("BAddress2", typeof(string));
            dt.Columns.Add("BCity", typeof(string));
            dt.Columns.Add("BState", typeof(string));
            dt.Columns.Add("BZip", typeof(string));

            return dt;
        }

        public void PopulateDataTable(DataTable? dt, FileData data)
        {
            DataRow dr = dt.NewRow();
            dr["AccountNumber"] = data.AccountNumber.ToString();
            dr["ExpDate"] = data.ExpDate.HasValue ? data.ExpDate.Value.ToString("yyyy-MM") : null;
            dr["CardID"] = data.CardID.ToString();
            dr["FName"] = data.FirstName?.Length > 50 ? data.FirstName?.Substring(0, 50) : data.FirstName;
            dr["LName"] = data.LastName?.Length > 50 ? data.LastName?.Substring(0, 50) : data.LastName;
            dr["BAddress1"] = data.Address1?.Length > 50 ? data.Address1?.Substring(0, 50) : data.Address1;
            dr["BAddress2"] = data.Address2?.Length > 50 ? data.Address2?.Substring(0, 50) : data.Address2;
            dr["BCity"] = data.City?.Length > 50 ? data.City?.Substring(0, 50) : data.City;
            dr["BState"] = data.State;
            dr["BZip"] = data.Zip;
            dt.Rows.Add(dr);
        }


    }
}
