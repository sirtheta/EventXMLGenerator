using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace GenerateEventXML.MainClasses
{
  internal class Event
  {
    public Event()
    {
      _selectedLocation = Locations.FirstOrDefault();
      _selectedEventCategorie = EventCategories.FirstOrDefault();
      _dateTimeStart = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
      _dateTimeEnd = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
    }

    public string? _selectedLocation;
    public string? _selectedEventCategorie;
    public string? _dateTimeStart;
    public string? _dateTimeEnd;

    public string? Id { get; set; }
    public string? EventTitle { get; set; }
    public string? Content { get; set; }
    public string? DateTimeStart
    {
      get { return _dateTimeStart; }
      set
      {
        _dateTimeStart = value;
      }
    }
    public string? DateTimeEnd
    {
      get { return _dateTimeEnd; }
      set
      {
        _dateTimeEnd = value;
      }
    }
    public List<string>? Locations
    {
      get
      {
        return ReadAllLocations();
      }
      set
      {
        _ = value;
      }
    }
    public List<string>? EventCategories
    {
      get
      {
        return ReadAllEventCategories();
      }
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
      get { return _selectedEventCategorie; }
      set { _selectedEventCategorie = value; }
    }

    private static List<string> ReadAllEventCategories()
    {
      List<string> categories = new();

      foreach (var key in ConfigurationManager.AppSettings.AllKeys)
      {
        if (key != null && key.StartsWith("EventCategorie"))
        {
          categories.Add(ConfigurationManager.AppSettings[key]);
        }
      }

      return categories;
    }

    private static List<string> ReadAllLocations()
    {
      List<string> locations = new();

      foreach (var key in ConfigurationManager.AppSettings.AllKeys)
      {
        if (key != null && key.StartsWith("Location"))
        {
          locations.Add(ConfigurationManager.AppSettings[key]);
        }
      }

      return locations;
    }

  }
}
