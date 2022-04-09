using GenerateEventXML.Commands;
using GenerateEventXML.Logic;
using GenerateEventXML.MainClasses;
using Microsoft.Win32;
using Notifications.Wpf.Core;
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
      bool retVal = false;
      SaveAndExportEvents sae = new();
      SaveFileDialog saveFileDialog = new()
      {
        Filter = "ZIP files (*.zip)|*.zip"
      };
      if (saveFileDialog.ShowDialog() == true)
      {
        var fileName = saveFileDialog.FileName;
        retVal = sae.SaveAndExport(EventCollection.ToList(), fileName);
      }
      if (retVal)
      {
        ShowNotification("Success", "Events saved successfully", NotificationType.Success);
      }
      else
      {
        ShowNotification("Error", "Events not saved", NotificationType.Error);
      }
    }
  }
}
