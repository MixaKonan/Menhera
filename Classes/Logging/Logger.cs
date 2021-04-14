using System;
using System.IO;
using System.Threading.Tasks;

namespace Menhera.Classes.Logging
{
    internal static class Logger
    {
        public static async Task LogIntoFile(string path, string message, LoggingInformationKind kind)
        {
            await using (var stream = File.AppendText(path))
            {
                switch (kind)
                {
                    case LoggingInformationKind.Info:
                    {
                        var content = string.Concat("INFO: ", DateTime.Now, " ", message);
                        await stream.WriteLineAsync(content);
                        break;
                    }
                    case LoggingInformationKind.Warning:
                    {
                        var content = string.Concat("WARNING: ", DateTime.Now, " ",message);
                        await stream.WriteLineAsync(content);
                        break;
                    }
                    case LoggingInformationKind.Error:
                    {
                        var content = string.Concat("ERROR: ", DateTime.Now, " ",message);
                        await stream.WriteLineAsync(content);
                        break;
                    }
                    case LoggingInformationKind.Fatal:
                    {
                        var content = string.Concat("FATAL: ", DateTime.Now, " ",message);
                        await stream.WriteLineAsync(content);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
        }
    }
}