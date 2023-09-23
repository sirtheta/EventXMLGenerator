using GenerateEventXML.Commands;
using GenerateEventXML.Logic;
using GenerateEventXML.MainClasses;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace GenerateEventXML.ViewModels
{
  internal class MainViewModel : BaseViewModel
  {

    public MainViewModel()
    {
      BtnAddOne = new RelayCommand<object>(AddTextBox);
      BtnRemoveEvent = new RelayCommand<object>(RemoveEvent);
      BtnSave = new RelayCommand<object>(OnBtnSaveCmd);
      BtnOpen = new RelayCommand<object>(OnBtnOpenCmd);      
    }

    private ObservableCollection<Event> _eventCollection = new();
    public ObservableCollection<Event> EventCollection
    {
      get
      {
        return _eventCollection;
      }
      set
      {
        _eventCollection = value;
      }
    }

    private string _dateTimeStartImport = DateTime.Now.ToString(ConfigData.DateFormat);
    private string _copyright = $"Copyright © {DateTime.Now.Year} nemicomp. All rights reserved. Developed and designed by Michael Neuhaus, licensed under the MIT license. Version: {GetVersion()}";
    public string Copyright { get => _copyright; set => _copyright = value; }

    public ICommand BtnAddOne
    {
      get;
      private set;
    }

    public ICommand BtnRemoveEvent
    {
      get;
      private set;
    }

    public ICommand BtnSave
    {
      get;
      private set;
    }
    public ICommand BtnOpen
    {
      get;
      private set;
    }

    public string DateTimeStartImport
    {
      get => _dateTimeStartImport;
        set
      {
        _dateTimeStartImport = value;
      }
    }


    private static string? GetVersion()
    {
      return System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString();
    }

    private void AddTextBox(object? parameter = null)
    {
      EventCollection.Add(new Event());
    }

    /// <summary>
    /// removes all selected events in the collection
    /// </summary>
    /// <param name="obj"></param>
    private void RemoveEvent(object obj)
    {
      var eventsToRemove = EventCollection.Where(e => e.ToDeleteSelected).ToList();

      foreach (var e in eventsToRemove)
      {
        EventCollection.Remove(e);
      }
    }

    /// <summary>
    /// Opens a file dialog to select the ics file
    /// will clear current events in the list!
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnOpenCmd(object obj)
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
        EventCollection.Clear();
        foreach (var entry in IcalImportHandler.ImportFile(File.ReadAllText(dlg.FileName), startImportDate, ConfigData.ImportKeyword))
        {
          EventCollection.Add(entry);
        }
      }
    }

    /// <summary>
    /// Saves the event list to a zip file to import in wordpress
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSaveCmd(object obj)
    {
      bool showError = true;
      //do not add events without titel
      List<Event> events = new();
      foreach (var item in EventCollection)
      {
        if (item.EventTitle != null)
        {
          events.Add(item);
        }
      }

      if (events.Count > 0 && ValidateDateTime())
      {
        //open filedialog to save file
        SaveAndExportEvents sae = new();
        SaveFileDialog saveFileDialog = new()
        {
          Filter = "ZIP files (*.zip)|*.zip"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
          var fileName = saveFileDialog.FileName;
          if (sae.SaveAndExport(events, fileName))
          {
            ShowNotification("Success", "Events saved successfully", NotificationType.Success);
            showError = false;
          }
        }
      }
      if  (showError)
      {
        ShowNotification("Error", "Events not saved", NotificationType.Error, 5);
      }
    }
    /// <summary>
    /// Validates DateTime Entry. If DateTime is not valid, it will throw an error notification
    /// </summary>
    /// <returns></returns>
    private bool ValidateDateTime()
    {
      bool retVal = false;
      foreach (var item in EventCollection)
      {
        try
        {
          if (item.DateTimeStart != null && item.DateTimeEnd != null)
          {
            DateTime.Parse(item.DateTimeStart);
            DateTime.Parse(item.DateTimeEnd);
            retVal = true;
          }
          else
          {
            ShowNotification("Error", "Datum ist NULL!", NotificationType.Error, 5);
            retVal = false;
          }
        }
        catch (Exception)
        {
          ShowNotification("Error", "Datum nicht korrekt eingegeben!", NotificationType.Error, 5);
          retVal = false;
          break;
        }
      }
      return retVal;
    }
  }
}
