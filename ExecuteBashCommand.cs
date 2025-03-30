using System.Collections.Generic;
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

    public static List<string> VerifyUserThemes() {
        var themes = ExecuteCommand(@"lookandfeeltool -l | sed -e 's/^org\.kde\.//' -e 's/\.desktop$//' -e 's/^\(.\)/\U\1/'").Split("\n");
        List<string> themesList = [.. themes];

        return themesList;
    }

    public static void SetTheme(string theme) {
        System.Console.WriteLine(SelectTime.DateHourToLog() + "Setting theme to " + theme.ToLower());
        // verify name of theme
        var selectedTheme = ExecuteCommand("lookandfeeltool -l | grep " + theme.ToLower() + ".desktop");
        ExecuteCommand("lookandfeeltool -a " + selectedTheme);
    }

}