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
            foreach (var line in File.ReadLines("/tmp/Shelly-Selected-Time.txt"))
            {
                if (line.Contains("selected-time = "))
                {
                    var selectedTime = line.Split(" = ")[1];
                    return selectedTime;
                }

            }

            return null;

        }

        public static string ReturnSecondTime()
        {
            foreach (var line in File.ReadLines("/tmp/Shelly-Selected-Time.txt"))
            {
                if (line.Contains("selected-second-time = "))
                {
                    var secondTimeSelected = line.Split(" = ")[1];
                    return secondTimeSelected;
                }

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
                    return selectedTheme;
                }
            }
            return null;

        }

        public static string ReturnSecondTheme()
        {
            foreach (var line in File.ReadLines("/tmp/Shelly-Selected-Time.txt"))
            {
                if (line.Contains("selected-second-theme = "))
                {
                    var secondThemeSelected = line.Split(" = ")[1];
                    return secondThemeSelected;
                }
            }
            return null;

        }
    }
}