using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveEM
{
    public interface ISettings
    {
        string? WorkingDirectory { get; }
        string? FinanceDirectory { get; }
        string? DataEntryDirectory { get; }
        string? LogDirectory { get; }
        void LoadSettings(ISettings settings);
    }

    public class Settings : ISettings
    {
        public string? WorkingDirectory { get; set; }
        public string? FinanceDirectory { get; set; }
        public string? DataEntryDirectory { get; set; }
        public string? LogDirectory { get; set; }

        public void LoadSettings(ISettings settings)
        {
            WorkingDirectory = settings.WorkingDirectory;
            FinanceDirectory = string.Format(this.WorkingDirectory, settings.FinanceDirectory);
            DataEntryDirectory = string.Format(this.WorkingDirectory, settings.DataEntryDirectory);
            LogDirectory = string.Format(this.WorkingDirectory, settings.LogDirectory);
        }
    }
}
