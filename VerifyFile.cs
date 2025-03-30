using System.IO;
using System;

namespace Shelly.UI
{
    public class VerifyFile
    {
        public static bool VerifyFileExists(string path)
        {
            var verifyFile = System.IO.File.Exists(path);

            if (verifyFile)
            {
                return true;
            }
            return false;
        }

        public static string ReturnTime()
        {
            using StreamReader sr = File.OpenText("/tmp/Shelly-Selected-Time.txt");
            string s = sr.ReadLine();

            if (s.Contains("selected-time = "))
            {
                var selectedTime = s.Split(" = ")[1];
                return selectedTime;
            }
            return null;

        }

        public static string ReturnTheme()
        {
            foreach (var line in File.ReadLines("/tmp/Shelly-Selected-Time.txt"))
            {
                if (line.Contains("selected-theme = "))
                {
                    var selectedTheme = line.Split(" = ")[1];
                    System.Console.WriteLine(selectedTheme);
                    return selectedTheme;
                }
            }
            return null;

        }
    }
}