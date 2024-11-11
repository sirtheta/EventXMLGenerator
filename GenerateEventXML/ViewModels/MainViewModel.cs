using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenerateEventXML.DomainModel;
using GenerateEventXML.Logic;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;


namespace GenerateEventXML.ViewModels
{
  internal partial class MainViewModel : ObservableObject
  {

    public MainViewModel()
    {
      WeakReferenceMessenger.Default.Register<DeleteEventMessage>(this, HandleEventDeletion);
    }

    private void HandleEventDeletion(object recipient, DeleteEventMessage message)
    {
      EventCollection.Remove(message.EventViewModel);
    }

    internal static void ShowNotification(string titel, string message, NotificationType type, int displayTime = 2)
    {
      var notificationManager = new NotificationManager();
      notificationManager.ShowAsync(new NotificationContent { Title = titel, Message = message, Type = type },
              areaName: "WindowArea", expirationTime: new TimeSpan(0, 0, displayTime));
    }

    [ObservableProperty]
    private ObservableCollection<EventItemViewModel> _eventCollection = new();


    [ObservableProperty]
    private string _dateTimeStartImport = DateTime.Now.ToString(ConfigData.DateFormat);

    [ObservableProperty]
    private string _copyright = $"Copyright © {DateTime.Now.Year} nemicomp. All rights reserved. Developed and designed by Michael Neuhaus, licensed under the MIT license. Version: {GetVersion()}";


    private static string? GetVersion() => System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString();


    [RelayCommand]
    public void AddEvent(Event? evt = null)
    {
      var newEvent = evt ?? new Event();
      var eventVM = new EventItemViewModel(newEvent);
      EventCollection.Add(eventVM);
    }

    [RelayCommand]
    private void OpenIcalFile()
    {
      OpenFileDialog dlg = new()
      {
        Filter = "ical files (*.ics)|*.ics"
      };

      if (dlg.ShowDialog() == true)
      {
        var startImportDate = DateTime.Now;
        try
        {
          startImportDate = DateTime.ParseExact(DateTimeStartImport, ConfigData.DateFormat, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
          ShowNotification("Warning", "Import start date not correct! Taking today", NotificationType.Warning, 5);
        }
        // clear the current EventCollection before import
        EventCollection.Clear();
        // Import events and convert to EventItemViewModels
        var importedEvents = IcalImportHandler.ImportFile(File.ReadAllText(dlg.FileName), startImportDate, ConfigData.ImportKeyword);
        foreach (var evt in importedEvents)
        {
          EventCollection.Add(new EventItemViewModel(evt));
        }
      }
    }

    [RelayCommand]
    private void SaveDataFile()
    {
      var eventCollection = EventCollection.Select(e => e.GetEvent()).ToList();

      if (SaveAndExportEvents.SaveEvent(eventCollection))
      {
        ShowNotification("Success", "Events saved successfully", NotificationType.Success);
      }
      else
      {
        ShowNotification("Error", "Events not saved", NotificationType.Error, 5);
      }
    }
  }
}
