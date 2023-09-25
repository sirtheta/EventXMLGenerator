using System.Collections.Generic;
using System.Configuration;

namespace GenerateEventXML.Logic
{
  internal static class ConfigData
  {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal static string DateFormat
    {
      get;
      private set;
    }

    internal static string ImportKeyword
    {
      get;
      private set;
    }

    internal static List<string> Locations
    {
      get;
      private set;
    }

    internal static List<string> Categories
    {
      get;
      private set;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    internal static void ReadConfigData()
    {
      ReadAllLocations();
      ReadAllEventCategories();
      ReadImportKeywordFromConfig();
      ReadDateFormatFromConfig();
    }

    /// <summary>
    /// read "Location" keys from app.config
    /// </summary>
    /// <returns></returns>
    private static void ReadAllLocations()
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
      Locations = locations;
    }

    /// <summary>
    /// read "EventCategorie" keys from app.config
    /// </summary>
    /// <returns></returns>
    private static void ReadAllEventCategories()
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
      Categories = categories;
    }

#pragma warning disable CS8601 // Possible null reference argument.
    private static void ReadImportKeywordFromConfig()
    {
      foreach (var key in ConfigurationManager.AppSettings.AllKeys)
      {
        if (key != null && key.StartsWith("ImportKeyword"))
        {
          ImportKeyword = ConfigurationManager.AppSettings[key];
        }
      }
    }

    private static void ReadDateFormatFromConfig()
    {
      foreach (var key in ConfigurationManager.AppSettings.AllKeys)
      {
        if (key != null && key.StartsWith("DateFormat"))
        {
          DateFormat = ConfigurationManager.AppSettings[key];
        }
      }
    }
#pragma warning restore CS8601 // Possible null reference argument.
  }
}
