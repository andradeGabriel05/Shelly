using System.Diagnostics;

namespace ExecuteBashCommand;
class Commands {
    public static string ExecuteCommand(string command)
    {
        
        command = command.Replace("\"","\\\"");

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \""+ command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        proc.Start();
        proc.WaitForExit();

        return proc.StandardOutput.ReadToEnd();

    }

    // public static string VerifyTime() {
    //     return ExecuteCommand("date '+%X'");
    // }

    public static string VerifyUserThemes() {
        return ExecuteCommand("lookandfeeltool -l");
    }

}