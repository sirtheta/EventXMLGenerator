using GenerateEventXML.Commands;
using GenerateEventXML.Logic;
using GenerateEventXML.MainClasses;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GenerateEventXML.ViewModels
{
  internal class MainViewModel : BaseViewModel
  {

    public MainViewModel()
    {
      BtnAddOne = new RelayCommand<object>(AddTextBox);
      BtnRemoveOne = new RelayCommand<object>(RemoveTextBox);
      BtnSave = new RelayCommand<object>(OnBtnSaveCmd);
      AddTextBox();
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

    public ICommand BtnAddOne
    {
      get;
      private set;
    }

    public ICommand BtnRemoveOne
    {
      get;
      private set;
    }

    public ICommand BtnSave
    {
      get;
      private set;
    }

    private void AddTextBox(object? parameter = null)
    {
      EventCollection.Add(new Event());
    }
    private void RemoveTextBox(object obj)
    {
      if (EventCollection.Count > 1)
      {
        EventCollection.RemoveAt(EventCollection.Count - 1);
      }
    }

    private void OnBtnSaveCmd(object obj)
    {
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
          }
        }
      }
      else
      {
        ShowNotification("Error", "Events not saved", NotificationType.Error);
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
            ShowNotification("Error", "Datum ist NULL!", NotificationType.Error);
            retVal = false;
          }
        }
        catch (Exception)
        {
          ShowNotification("Error", "Datum nicht korrekt eingegeben!", NotificationType.Error);
          retVal = false;
        }
      }
      return retVal;
    }
  }
}
