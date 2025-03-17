class SelectTime {
    public static bool InputTimeToSwitch(string actualTime) {
        Console.WriteLine("Select a time: ");
        var selectHour = Console.ReadLine();

        Console.WriteLine("Select minutes: ");
        var selectMinutes = Console.ReadLine();

        if (string.IsNullOrEmpty(selectHour) || string.IsNullOrEmpty(selectMinutes)) {
            Console.WriteLine("Invalid input. Please enter a valid time.");
            return false;
        }

        var selectedTime = selectHour + ":" + selectMinutes;
        if(actualTime.Contains(selectedTime)) {
            Console.WriteLine("Time is valid.");
            return true;
        } else {
            Console.WriteLine("Time is invalid.");
            return false;
        }
    }
    
}