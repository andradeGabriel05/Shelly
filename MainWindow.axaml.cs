using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Threading;
using System.IO;
using Shelly.UI;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
namespace Shelly;

public partial class MainWindow : Window
{
    private CancellationTokenSource _cts = new();
    bool fileExist = VerifyFile.VerifyFileExists("/tmp/Shelly-Selected-Time.txt");

    public MainWindow()
    {
        InitializeComponent();

        Console.WriteLine(SelectTime.DateHourToLog() + " Application started");

        List<string> themes = ExecuteBashCommand.Commands.VerifyUserThemes();

        foreach (var theme in themes)
        {
            ThemeComboBox.Items.Add(new ComboBoxItem { Content = theme });
        }

        ThemeComboBox.SelectedIndex = ThemeComboBox.Items.IndexOf(ThemeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == themes[0]));


        if (fileExist)
        {
            var selectedTime = VerifyFile.ReturnTime();
            var selectedTheme = VerifyFile.ReturnTheme();
            var selectedTimeSplit = selectedTime.Split(":");
            var selectedHour = selectedTimeSplit[0];
            var selectedMinute = selectedTimeSplit[1];

            HourComboBox.SelectedIndex = HourComboBox.Items.IndexOf(HourComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == selectedHour));
            MinuteComboBox.SelectedIndex = MinuteComboBox.Items.IndexOf(MinuteComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == selectedMinute));
            ThemeComboBox.SelectedIndex = ThemeComboBox.Items.IndexOf(ThemeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == selectedTheme));
        }

        this.Opened += OnOpened;
    }

    private async void OnOpened(object? sender, EventArgs e)
    {
        await Task.Run(() => WriteToFilePeriodically(_cts.Token));
    }

    private async Task WriteToFilePeriodically(CancellationToken token)
    {
        if (fileExist && SelectTime.VerifyActualTime() == VerifyFile.ReturnTime())
        {
            System.Console.WriteLine(SelectTime.DateHourToLog() + " Waiting 1 minute to check file");
            await Task.Delay(60000, _cts.Token);
        }

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
                    Console.WriteLine(SelectTime.DateHourToLog() + " File exists");
                    await Task.Run(() => VerifyTimeForChangeTheme(VerifyFile.ReturnTime()));
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
                // await Task.Delay(30000, _cts.Token);
                await Task.Delay(1000, _cts.Token);
                System.Console.WriteLine(SelectTime.DateHourToLog() + " ...");
                if (time.Contains(actualTime))
                {
                    Console.WriteLine(SelectTime.DateHourToLog() + " Time to change theme");
                    ExecuteBashCommand.Commands.SetTheme(VerifyFile.ReturnTheme());
                    ExecuteBashCommand.Commands.SetTheme(VerifyFile.ReturnTheme());
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
        WriteToFilePeriodically(_cts.Token);
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        var selectedHour = (HourComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        var selectedMinute = (MinuteComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        var selectedTheme = (ThemeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        SelectTime.TextFileSelectedTime(selectedHour, selectedMinute, selectedTheme);

    }
}