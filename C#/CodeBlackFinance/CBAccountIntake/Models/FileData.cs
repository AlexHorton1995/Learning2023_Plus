using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAccountIntake.Models
{
    public interface IFileData
    {
        string? FirstName { get; }
        string? LastName { get; }
        long AccountNumber { get; }
        string? ExpDate { get; }
        short CardID { get; }
        string? Email { get; }
        string? Address1 { get; }
        string? Address2 { get; }
        string? City { get; }
        string? State { get; }
        string? Zip { get; }

        void Initialize();
    }

    public class FileData
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long AccountNumber { get; set; }
        public DateTime? ExpDate { get; set; }
        public short CardID { get; set; }
        public string? Email { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        public void Initialize()
        {
            this.FirstName = null;
            this.LastName = null;
            this.AccountNumber = 0;
            this.ExpDate = null;
            this.CardID = 0;
            this.Email = null;
            this.Address1 = null;
            this.Address2 = null;
            this.City = null;
            this.State = null;
            this.Zip = null;
        }

        public void Parse(string[] data)
        {
            this.FirstName = data[0];
            
            this.LastName = data[1];

            if (long.TryParse(data[2], out long acctNum))
                this.AccountNumber = acctNum;

            if (DateTime.TryParse(data[3], out DateTime expDate))
                this.ExpDate = expDate;

            if (short.TryParse(data[4], out short cid))
                this.CardID = cid;

            this.Email = data[5];
            this.Address1 = data[6];
            this.Address2 = data[7];
            this.City = data[8];
            this.State = data[9];
            this.Zip = data[10];

        }

    }
}
