using GenerateEventXML.Commands;
using GenerateEventXML.Logic;
using GenerateEventXML.MainClasses;
using System;
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

    private void OnBtnSaveCmd(object obj)
    {
      GenerateXML.GenerateXMLFile(EventCollection.ToList());
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
  }
}
