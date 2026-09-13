using System;
using System.Diagnostics;
using System.IO;

namespace Mile.DotNet.Helpers
{
    internal static class Git
    {
        public static string GetRootPath()
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        FileName = "git",
                        Arguments = "rev-parse --show-toplevel",
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
                    }
                };
                if (process.Start())
                {
                    string result = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        if (!string.IsNullOrEmpty(result))
                        {
                            return Path.GetFullPath(result);
                        }
                    }
                }
            }
            catch
            {
                // Git discovery is allowed to fail.
            }

            return string.Empty;
        }
    }
}
