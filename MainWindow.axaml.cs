using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Threading;
using System.IO;
using Shelly.UI;
using System.Threading;
using System.Threading.Tasks;
namespace Shelly;

public partial class MainWindow : Window
{
    private CancellationTokenSource _cts = new();

    public MainWindow()
    {
        InitializeComponent();

        Console.WriteLine(SelectTime.DateHourToLog() + " Application started");
        this.Opened += OnOpened;


    }

    private async void OnOpened(object? sender, EventArgs e)
    {
        await Task.Run(() => WriteToFilePeriodically(_cts.Token));
        await Task.Run(() => VerifyTimeForChangeTheme(VerifyFile.ReturnTime()));
    }

    private async Task WriteToFilePeriodically(CancellationToken token)
    {
        bool fileExist;
        System.Console.WriteLine(SelectTime.DateHourToLog() + " Checking file");

        do
        {
            try
            {
                await Task.Delay(1000, token);
                System.Console.WriteLine(SelectTime.DateHourToLog() + " ...");
                fileExist = VerifyFile.VerifyFileExists("/tmp/Shelly-Selected-Time.txt");

                if (fileExist)
                {
                    Console.WriteLine(SelectTime.DateHourToLog() + " File exists [main]");
                    break;
                }

            }
            catch (TaskCanceledException)
            {
                System.Console.WriteLine("Task cancelada");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao escrever no arquivo: {ex.Message}");
                break;
            }
        } while (!token.IsCancellationRequested || !fileExist);
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        _cts.Cancel();
    }

    private async Task VerifyTimeForChangeTheme(string time)
    {
        //horario atual
        string actualTime;
        var counter = 0;

        Console.WriteLine(SelectTime.DateHourToLog() + " Selected time: " + time);

        do
        {
            actualTime = SelectTime.VerifyActualTime();

            if (time != VerifyFile.ReturnTime())
            {
                System.Console.WriteLine(SelectTime.DateHourToLog() + " Time changed");
                System.Console.WriteLine(SelectTime.DateHourToLog() + " New time: " + VerifyFile.ReturnTime());
                time = VerifyFile.ReturnTime();
            }

            try
            {
                await Task.Delay(1000, _cts.Token);
                System.Console.WriteLine(SelectTime.DateHourToLog() + " ...");
                if (time.Contains(actualTime) && counter == 0)
                {
                    Console.WriteLine(SelectTime.DateHourToLog() + " Time to change theme");
                    counter++;
                }
            }
            catch (TaskCanceledException)
            {
                System.Console.WriteLine("Task cancelada");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao escrever no arquivo: {ex.Message}");
                break;
            }
        } while (time != actualTime);
        System.Console.WriteLine(SelectTime.DateHourToLog() + " Theme changed");
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        var selectedHour = (HourComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        var selectedMinute = (MinuteComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        var selectedPeriod = (PeriodComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;

        SelectTime.TextFileSelectedTime(selectedHour, selectedMinute, selectedPeriod);

    }
}