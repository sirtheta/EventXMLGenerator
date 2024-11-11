using GenerateEventXML.DomainModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GenerateEventXML.Logic
{
  internal class SaveAndExportEvents
  {
    private string? _template;
    private readonly string _fileName = "Veranstaltungen " + DateTime.Today.ToString("dd-MMMM-yyyy");

    /// <summary>
    /// Will export the eventlist to a zip file
    /// </summary>
    /// <param name="events"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool SaveAndExport(IEnumerable<Event> events, string fileName)
    {
      var tempPath = Path.GetTempPath() + "\\eventExport\\bundle\\_\\";
      Directory.CreateDirectory(tempPath);
      bool retVal = true;
      retVal &= GenerateXML.GenerateXMLFile(events, tempPath + _fileName);
      retVal &= ReadTemplateFromJSon();
      retVal &= GenerateJSonWithTemplate(tempPath);
      retVal &= CreateZipFile(fileName);
      DeleteTempFiles();
      return retVal;
    }

    /// <summary>
    /// Import needs an Template. This can be defined in app.config
    /// </summary>
    /// <returns></returns>
    private bool ReadTemplateFromJSon()
    {
      bool retVal = false;
      try
      {
        var templatePath = ConfigurationManager.AppSettings["Template"];
        if (!string.IsNullOrEmpty(templatePath))
        {
          _template = File.ReadAllText(templatePath);
          retVal = true;
        }
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message);
      }
      return retVal;
    }

    /// <summary>
    /// Generate the Json from the import template
    /// </summary>
    /// <param name="tempPath"></param>
    /// <returns></returns>
    private bool GenerateJSonWithTemplate(string tempPath)
    {
      bool retVal = false;
      if (_template != null)
      {
        dynamic? jsonObj = JsonConvert.DeserializeObject(_template);
        try
        {
          if (jsonObj != null)
          {
            jsonObj["event"][0]["name"] = _fileName;
            jsonObj["event"][0]["source_file_name"] = _fileName + ".xml";
            File.WriteAllText(tempPath + "WP All Import Template - " + _fileName + ".txt", JsonConvert.SerializeObject(jsonObj, Formatting.None));
            retVal = true;
          }
        }
        catch (Exception e)
        {
          Debug.WriteLine(e.Message);
        }
      }
      return retVal;
    }

    /// <summary>
    /// Delete all temporary generated files
    /// </summary>
    private static void DeleteTempFiles()
    {
      Directory.Delete(Path.GetTempPath() + "eventExport", true);
    }

    /// <summary>
    /// Creates the zip file
    /// </summary>
    /// <param name="outPath"></param>
    /// <returns></returns>
    private static bool CreateZipFile(string outPath)
    {
      bool retVal = false;

      var path = Path.GetTempPath() + "eventExport";
      try
      {
        if (File.Exists(outPath))
        {
          File.Delete(outPath);
        }
        ZipFile.CreateFromDirectory(path, outPath);
        retVal = true;
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message);
      }
      return retVal;
    }

    /// <summary>
    /// Saves the event list to a zip file to import in wordpress
    /// </summary>
    /// <param name="eventCollection"></param>
    /// <returns></returns>
    internal static bool SaveEvent(IEnumerable<Event> eventCollection)
    {
      bool success = false;
      //do not add events without titel
      List<Event> events = new();
      foreach (var item in eventCollection)
      {
        if (item.EventTitle != null)
        {
          events.Add(item);
        }
      }

      if (events.Count > 0 && ValidateDateTime(eventCollection))
      {
        //open filedialog to save file
        SaveFileDialog saveFileDialog = new()
        {
          Filter = "ZIP files (*.zip)|*.zip"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
          SaveAndExportEvents sae = new();
          var fileName = saveFileDialog.FileName;
          if (sae.SaveAndExport(events, fileName))
          {
            success = true;
          }
        }
      }
      return success;
    }

    /// <summary>
    /// Validates DateTime Entry. If DateTime is not valid, it will return success false
    /// </summary>
    /// <param name="eventCollection"></param>
    /// <returns></returns>
    internal static bool ValidateDateTime(IEnumerable<Event> eventCollection)
    {
      bool success = false;
      foreach (var item in eventCollection)
      {
        try
        {
          if (item.DateTimeStart != null && item.DateTimeEnd != null)
          {
            DateTime.Parse(item.DateTimeStart);
            DateTime.Parse(item.DateTimeEnd);
            success = true;
          }
        }
        catch (Exception e)
        {
          Debug.WriteLine(e.Message);
          break;
        }
      }
      return success;
    }
  }
}
