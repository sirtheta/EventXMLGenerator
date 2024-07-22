using GenerateEventXML.Commands;
using GenerateEventXML.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GenerateEventXML.Common;

namespace GenerateEventXML.MainClasses
{
  internal class Event : Notify
  {
    public Event()
    {
      _selectedLocation = Locations.FirstOrDefault();
      _selectedEventCategorie = EventCategories.FirstOrDefault();
      _dateTimeStart = DateTime.Now.ToString(ConfigData.DateFormat) + " 09:30";
      _dateTimeEnd = DateTime.Now.ToString(ConfigData.DateFormat) + " 12:00";
      BtnDeleteEventCklicked = new RelayCommand<object>(BtnDeleteEventExecute);
    }

    private string? _selectedLocation;
    private string? _selectedEventCategorie;
    private string _dateTimeStart;
    private string _dateTimeEnd;

    public ICommand BtnDeleteEventCklicked { get; private set; }

    internal EventHandler? DeleteEventClicked;
    private void BtnDeleteEventExecute(object obj)
    {
      ToDeleteSelected = true;
      DeleteEventClicked?.Invoke(this, EventArgs.Empty);
    }

    public string? EventTitle { get; set; }
    public string? Content { get; set; }
    public string DateTimeStart
    {
      get => _dateTimeStart;
      set
      {
        _dateTimeStart = value;
      }
    }
    public string DateTimeEnd
    {
      get => _dateTimeEnd;
      set
      {
        _dateTimeEnd = value;
      }
    }
    public bool ToDeleteSelected { get; set; } = false;

    /// <summary>
    /// List of Locations from app.config
    /// </summary>
    public static List<string> Locations
    {
      get => ConfigData.Locations;
      set
      {
        _ = value;
      }
    }

    /// <summary>
    /// List of EventCategories from app.config
    /// </summary>
    public static List<string> EventCategories
    {
      get => ConfigData.Categories;
      set
      {
        _ = value;
      }
    }
    public string? SelectedLocation
    {
      get { return _selectedLocation; }
      set { _selectedLocation = value; }
    }
    public string? SelectedEventCategorie
    {
      get => _selectedEventCategorie;
      set { _selectedEventCategorie = value; }
    }
  }
}
