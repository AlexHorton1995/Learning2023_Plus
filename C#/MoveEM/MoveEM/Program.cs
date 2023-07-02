using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace MoveEM
{
    internal class Program : IDisposable
    {
        internal static ISettings? ConfigSettings { get; set; }
        internal static string SettingType { get; set; }


        static Program() { }

        public Program()
        {
        }

        private static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }


        static void Main(string[] args)
        {
#if DEBUG
            SettingType = "DebugSettings";
#else
            SettingType = "ReleaseSettings";
#endif
            var config = GetConfig();
            var configs = config?.GetRequiredSection(SettingType).Get<Settings>();
            LoadSettings(configs);

            using (var log = new StreamWriter($"{ConfigSettings.LogDirectory}\\MoveEmLog_{DateTime.Now.ToString("yyyyMMddHHmmss")}"))
            using (var prog = new Program())
            {
                try
                {
                    prog.Run(log);
                }
                catch (Exception ex)
                {
                    Extensions.LogError(log, ex, "NOFILE");
                }
                finally
                {
                    Extensions.LogEntry(log, "FileMover Process Completed.");
                    log.Close();
                }
            }


        }

        public void Run(StreamWriter log)
        {
            Extensions.LogEntry(log, "FileMover process has Started.");
            MoveFiles(log);
        }

        public static void LoadSettings(Settings configs)
        {
            ConfigSettings = new Settings();
            ConfigSettings.LoadSettings(configs);
        }

        public void MoveFiles(StreamWriter log)
        {
            string fileName = string.Empty;
            string filePath = string.Empty;
            int fileCntr = 0;

            //data Entry Paths
            Extensions.LogEntry(log, "Initalizing Source Folders...");
            var cPath = string.Format(ConfigSettings?.DataEntryDirectory + $"\\Contributions\\{DateTime.Now.Year}");
            var cIPath = string.Format(ConfigSettings?.DataEntryDirectory + $"\\Capital Improvement\\{DateTime.Now.Year}");
            var sOPath = string.Format(ConfigSettings?.DataEntryDirectory + $"\\Special Offering\\{DateTime.Now.Year}");
            var gPath = string.Format(ConfigSettings?.DataEntryDirectory + $"\\Givelify\\{DateTime.Now.Year}");

            //Finance Directories
            Extensions.LogEntry(log, "Initalizing Destination Folders...");
            var outCPath = string.Format(ConfigSettings?.FinanceDirectory + $"\\Contributions\\{DateTime.Now.Year}");
            var outCIPath = string.Format(ConfigSettings?.FinanceDirectory + $"\\Capital Improvement\\{DateTime.Now.Year}");
            var outSOPath = string.Format(ConfigSettings?.FinanceDirectory + $"\\Special (Sacrificial) Offering\\{DateTime.Now.Year}");
            var outGPath = string.Format(ConfigSettings?.FinanceDirectory + $"\\Givelify\\{DateTime.Now.Year}");


            //Existing Data Entry files
            var contrFiles = Directory.EnumerateFiles(cPath).ToList();
            var capImpFiles = Directory.EnumerateFiles(cIPath).ToList();
            var specialFiles = Directory.EnumerateFiles(sOPath).ToList();
            var givelifyFiles = Directory.EnumerateFiles(gPath).ToList();

            #region Contributions
            Extensions.LogEntry(log, "*****************************************CONTRIBUTIONS*****************************************");
            if (contrFiles.Count > 0)
            {
                foreach (var contrFile in contrFiles)
                {
                    Extensions.LogEntry(log, $"Found file {contrFile} in {cPath}, moving file to {outCPath}");
                    try
                    {
                        fileName = Path.GetFileName(contrFile);
                        File.Move(contrFile, $"{outCPath}\\{fileName}");
                        Extensions.LogEntry(log, $"File moved Successfully!");
                        fileCntr++;
                    }
                    catch (Exception cFileEx)
                    {
                        Extensions.LogError(log, cFileEx, fileName);
                    }
                }
                Extensions.LogEntry(log, $"Moved {fileCntr} Contribution Files");
            }
            else
            {
                Extensions.LogEntry(log, $"No Contribution Reports Found!");
            }
            Extensions.LogEntry(log, "*****************************************CONTRIBUTIONS END*****************************************");
            #endregion

            Extensions.SectionSpace(log);

            #region Capital Improvement
            Extensions.LogEntry(log, "****************************************CAPITAL IMPROVEMENT***************************************");
            if (capImpFiles.Count > 0)
            {
                fileCntr = 0;
                foreach (var capImpFile in capImpFiles)
                {
                    Extensions.LogEntry(log, $"Found file {capImpFile} in {cIPath}, moving file to {outCIPath}");
                    try
                    {
                        fileName = Path.GetFileName(capImpFile);
                        File.Move(capImpFile, $"{outCIPath}\\{fileName}");
                        Extensions.LogEntry(log, "File moved Successfully!");
                        fileCntr++;
                    }
                    catch (Exception cIFileEx)
                    {
                        Extensions.LogError(log, cIFileEx, fileName);
                    }
                }
                Extensions.LogEntry(log, $"Moved {fileCntr} Capital Improvement Files");
            }
            else
            {
                Extensions.LogEntry(log, "No Capital Improvement Reports Found!");
            }
            Extensions.LogEntry(log, "*************************************CAPITAL IMPROVEMENT END**************************************");
            #endregion

            Extensions.SectionSpace(log);

            #region Special Offering
            Extensions.LogEntry(log, "****************************************SPECIAL OFFERING******************************************");
            if (specialFiles.Count > 0)
            {
                fileCntr = 0;
                foreach (var spcOffFile in specialFiles)
                {
                    Extensions.LogEntry(log, $"Found file {spcOffFile} in {sOPath}, moving file to {outSOPath}");
                    try
                    {
                        fileName = Path.GetFileName(spcOffFile);
                        File.Move(spcOffFile, $"{outSOPath}\\{fileName}");
                        Extensions.LogEntry(log, "File moved Successfully!");
                        fileCntr++;
                    }
                    catch (Exception sOFileEx)
                    {
                        Extensions.LogError(log, sOFileEx, fileName);
                    }
                }
                Extensions.LogEntry(log, $"Moved {fileCntr} Special Offering Files");
            }
            else
            {
                Extensions.LogEntry(log, "No Special Offering files Found!");
            }
            Extensions.LogEntry(log, "**************************************SPECIAL OFFERING END****************************************");
            #endregion

            Extensions.SectionSpace(log);

            #region Givelify
            Extensions.LogEntry(log, "********************************************GIVELIFY**********************************************");
            if (givelifyFiles.Count > 0)
            {
                fileCntr = 0;
                foreach (var givelifyFile in givelifyFiles)
                {
                    Extensions.LogEntry(log, $"Found file {givelifyFile} in {gPath}, moving file to {outGPath}");
                    try
                    {
                        fileName = Path.GetFileName(givelifyFile);
                        File.Move(givelifyFile, $"{outGPath}\\{fileName}");
                        Extensions.LogEntry(log, "File moved Successfully!");
                        fileCntr++;
                    }
                    catch (Exception gFileEx)
                    {
                        Extensions.LogError(log, gFileEx, fileName);
                    }
                }
                Extensions.LogEntry(log, $"Moved {fileCntr} Givelify Files");

            }
            else
            {
                Extensions.LogEntry(log, "No Givelify files Found!");
            }
            Extensions.LogEntry(log, "******************************************GIVELIFY END********************************************");
            #endregion

            Extensions.SectionSpace(log);

        }

        #region Disposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Program()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}