using GenerateEventXML.MainClasses;
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

    
    public bool SaveAndExport(List<Event> events, string fileName)
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
    /// Delete all temporary generated files
    /// </summary>
    private static void DeleteTempFiles()
    {
      Directory.Delete(Path.GetTempPath() + "eventExport", true);
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
        }
        retVal = true;
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message);
      }
      return retVal;
    }

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
          }
          retVal = true;
        }
        catch (Exception e)
        {
          Debug.WriteLine(e.Message);
        }
      }
      return retVal;
    }

    private static bool CreateZipFile(string outPath)
    {
      bool retVal = false;

      var path = Path.GetTempPath() + "eventExport";
      if (File.Exists(outPath))
      {
        File.Delete(outPath);
      }
      try
      {
        ZipFile.CreateFromDirectory(path, outPath);
        retVal = true;
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message);
      }
      return retVal;
    }
  }
}
