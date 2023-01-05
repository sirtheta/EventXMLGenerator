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
      _dateTimeStart = DateTime.Now.ToString("dd-MM-yyyy") + " 09:30";
      _dateTimeEnd = DateTime.Now.ToString("dd-MM-yyyy") + " 12:00";
    }

    public string? _selectedLocation;
    public string? _selectedEventCategorie;
    public string _dateTimeStart;
    public string _dateTimeEnd;

    public string? Id { get; set; }
    public string? EventTitle { get; set; }
    public string? Content { get; set; }
    public string DateTimeStart
    {
      get { return _dateTimeStart; }
      set
      {
        _dateTimeStart = value;
      }
    }
    public string DateTimeEnd
    {
      get { return _dateTimeEnd; }
      set
      {
        _dateTimeEnd = value;
      }
    }
    /// <summary>
    /// List of Locations from app.config
    /// </summary>
    public static List<string> Locations
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
    /// <summary>
    /// List of EventCategories from app.config
    /// </summary>
    public static List<string> EventCategories
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

    /// <summary>
    /// read "EventCategorie" keys from app.config
    /// </summary>
    /// <returns></returns>
    private static List<string> ReadAllEventCategories()
    {
      List<string> categories = new();

      foreach (var key in ConfigurationManager.AppSettings.AllKeys)
      {
        if (key != null && key.StartsWith("EventCategorie"))
        {
#pragma warning disable CS8604 // Possible null reference argument.
          categories.Add(ConfigurationManager.AppSettings[key]);
#pragma warning restore CS8604 // Possible null reference argument.
        }
      }

      return categories;
    }

    /// <summary>
    /// read "Location" keys from app.config
    /// </summary>
    /// <returns></returns>
    private static List<string> ReadAllLocations()
    {
      List<string> locations = new();

      foreach (var key in ConfigurationManager.AppSettings.AllKeys)
      {
        if (key != null && key.StartsWith("Location"))
        {
#pragma warning disable CS8604 // Possible null reference argument.
          locations.Add(ConfigurationManager.AppSettings[key]);
#pragma warning restore CS8604 // Possible null reference argument.
        }
      }

      return locations;
    }

  }
}
