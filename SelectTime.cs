using System.Diagnostics;
using System.Threading;
using ExecuteBashCommand;
using System.IO;
using System;
class SelectTime
{

    public static void TextFileSelectedTime(string hour, string minute, string period)
    {
        var selectedTime = hour + ":" + minute;

        string path = "/tmp/Shelly-Selected-Time.txt";

        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("selected-time = " + selectedTime);
            }
            System.Console.WriteLine(DateHourToLog() + " File created");
            return;
        }

        using (StreamWriter sw = new StreamWriter(path, false))
        {
            sw.WriteLine("selected-time = " + selectedTime);
            sw.WriteLine("selected-period = " + period);
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