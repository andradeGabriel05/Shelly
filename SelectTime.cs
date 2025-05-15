using System.Diagnostics;
using System.Threading;
using ExecuteBashCommand;
using System.IO;
using System;
class SelectTime
{

// funcao para salvar arquivos com tempo escolhido
    public static void TextFileSelectedTime(string hour, string secondHour, string minute, string secondMinute, string theme, string secondTheme)
    {
        var selectedTime = hour + ":" + minute;
        var selectedSecondTime = secondHour + ":" + secondMinute;

        string path = "/tmp/Shelly-Selected-Time.txt";

        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("selected-time = " + selectedTime);
                sw.WriteLine("selected-theme = " + theme);
                if(selectedSecondTime != null) {
                    sw.WriteLine("selected-second-time = " + selectedSecondTime);
                    sw.WriteLine("selected-second-theme = " + secondTheme);
                }
            }
            System.Console.WriteLine(DateHourToLog() + " File created");
            return;
        }

        using (StreamWriter sw = new StreamWriter(path, false))
        {
            sw.WriteLine("selected-time = " + selectedTime);
            sw.WriteLine("selected-theme = " + theme);
            if(selectedSecondTime != "") {
                    sw.WriteLine("selected-second-time = " + selectedSecondTime);
                    sw.WriteLine("selected-second-theme = " + secondTheme);
                }
        }
        System.Console.WriteLine(DateHourToLog() + " File updated");
    }
    
    public static string VerifyActualTime() {
        return ExecuteBashCommand.Commands.ExecuteCommand("date +'%H:%M'").Trim();
    }

    public static string DateHourToLog()
    {
        return DateTime.Now.ToString("[ yyyy-MM-dd HH:mm:ss ]");
    }

}