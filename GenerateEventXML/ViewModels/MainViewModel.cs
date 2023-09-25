using GenerateEventXML.Commands;
using GenerateEventXML.Logic;
using GenerateEventXML.MainClasses;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
      BtnSave = new RelayCommand<object>(OnBtnSaveCmd);
      BtnOpen = new RelayCommand<object>(OnBtnOpenCmd);
      ((INotifyCollectionChanged)EventCollection).CollectionChanged += DeleteEventHandler;
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

    private void DeleteEventHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (var item in e.NewItems?.OfType<Event>() ?? Enumerable.Empty<Event>())
        {
          item.DeleteEventClicked += RemoveEvent;
        }
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var item in e.OldItems?.OfType<Event>() ?? Enumerable.Empty<Event>())
        {
          item.DeleteEventClicked -= RemoveEvent;
        }
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


    private static string? GetVersion() => System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString();

    private void AddTextBox(object? parameter = null)
    {
      EventCollection.Add(new Event());
    }

    /// <summary>
    /// removes the event in the collection
    /// </summary>
    /// <param name="obj"></param>
    private void RemoveEvent(object? obj, EventArgs e)
    {
      EventCollection.Remove(EventCollection.Where(e => e.ToDeleteSelected).First());
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
        // clear the current EventCollection before import
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
      if (SaveAndExportEvents.SaveEvent(EventCollection))
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
