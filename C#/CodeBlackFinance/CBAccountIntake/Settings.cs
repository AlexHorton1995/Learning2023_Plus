using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAccountIntake
{
    public interface ISettings
    {
        DateTime? Date { get; }
        string FilePath { get; }

        bool LoadSettings(string[] args);
    }

    internal class Settings : ISettings
    {
        public DateTime? Date { get; set; }
        public string? FilePath { get; set; }

        public Settings()
        {
            this.Date = null;
            this.FilePath = null;
        }

        public bool LoadSettings(string[] args)
        {
            char[] delimiters = new char[] { ' ', ':' };

            foreach (string arg in args)
            {
                var splitInfo = arg.Split(delimiters);

                switch (splitInfo[0])
                {
                    case "Date":
                        if (DateTime.TryParseExact(splitInfo[1], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,  out DateTime runDate))
                            this.Date = runDate;
                        break;
                    case "FileName":
                            this.FilePath = splitInfo[1];
                        break;
                }
            }

            return ((this.Date == null) || (string.IsNullOrEmpty(this.FilePath)));

        }
    }
}
