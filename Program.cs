// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        var verifyUserThemes = ExecuteBashCommand.ExecuteCommand("lookandfeeltool -l");
        Console.WriteLine(verifyUserThemes);

        var time = ExecuteBashCommand.VerifyTime();
        var selectedTime = SelectTime.InputTimeToSwitch(time);

        if (selectedTime)
        {
            ExecuteBashCommand.ExecuteCommand("lookandfeeltool -a \"org.kde.breezedark.desktop\"");
        }
    }
}