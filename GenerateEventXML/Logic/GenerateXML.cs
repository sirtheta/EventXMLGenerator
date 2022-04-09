using GenerateEventXML.MainClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GenerateEventXML.Logic
{
  internal class GenerateXML
  {
    /// <summary>
    /// Generate XML-File with XML text writer
    /// </summary>
    /// <param name="events"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool GenerateXMLFile(List<Event> events, string fileName)
    {
      bool retVal = false;
      try
      {
        //Create XML-File
        using StreamWriter file = new(fileName + ".xml");
        //Create XML-Document
        System.Xml.XmlTextWriter writer = new(file)
        {
          Formatting = System.Xml.Formatting.Indented
        };
        writer.WriteStartDocument();
        writer.WriteStartElement("data");

        int _idCounter = 0;
        //Write all events to XML-File
        foreach (Event e in events)
        {
          //contains all data for one event
          _idCounter++;
          writer.WriteStartElement("post");
          writer.WriteElementString("identifyer", _idCounter.ToString());
          writer.WriteElementString("Title", e.EventTitle);
          writer.WriteElementString("Content", e.Content);
          writer.WriteElementString("EventCategories", e.SelectedEventCategorie);
          writer.WriteElementString("imic_strict_no_sidebar", "0");
          writer.WriteElementString("imic_sidebar_columns_layout", "3");
          writer.WriteElementString("imic_featured_event", "no");
          writer.WriteElementString("imic_event_start_dt", e.DateTimeStart);
          writer.WriteElementString("imic_event_end_dt", e.DateTimeEnd);
          writer.WriteElementString("imic_google_map_track", "0");
          writer.WriteElementString("imic_event_registration", "0");
          writer.WriteElementString("imic_custom_event_registration_target", "0");
          writer.WriteElementString("imic_event_frequency_type", "0");
          writer.WriteElementString("imic_event_day_month", "first");
          writer.WriteElementString("imic_event_week_day","sunday");
          writer.WriteElementString("imic_event_frequency", "35");
          writer.WriteElementString("imic_pages_Choose_slider_display", "2");
          writer.WriteElementString("imic_pages_banner_overlay", "0");
          writer.WriteElementString("imic_pages_banner_animation", "0");
          writer.WriteElementString("imic_pages_select_revolution_from_list", "2");
          writer.WriteElementString("imic_pages_slider_pagination", "yes");
          writer.WriteElementString("imic_pages_slider_auto_slide", "yes");
          writer.WriteElementString("imic_pages_slider_direction_arrows", "yes");
          writer.WriteElementString("imic_pages_slider_interval", "7000");
          writer.WriteElementString("imic_pages_slider_effects", "fade");
          writer.WriteElementString("imic_pages_nivo_effects", "sliceDown");
          writer.WriteElementString("adore_ticket_status", "0");
          writer.WriteElementString("imic_event_frequency_end", DateTime.Parse(e.DateTimeEnd).Date.ToString("yyyy-MM-dd"));
          writer.WriteElementString("imic_event_address2", e.SelectedLocation);
          writer.WriteElementString("_last_editor_used_jetpack", "classic-editor");
          writer.WriteElementString("Status", "publish");          
          writer.WriteEndElement();
        }

        //Close XML-Document
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
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
