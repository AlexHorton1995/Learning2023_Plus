using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveEM
{
    internal static class Extensions
    {
        public static void LogEntry(StreamWriter log, string val)
        {
            log.WriteLine($"{DateTime.Now} {val}");
        }

        public static void LogError(StreamWriter log, Exception ex, string file) 
        {
            log.WriteLine($"{DateTime.Now} {file} ERROR! {ex.Message}");
        }

        public static void SectionSpace(StreamWriter log)
        {
            log.WriteLine(); log.WriteLine();
        }
    }
}
