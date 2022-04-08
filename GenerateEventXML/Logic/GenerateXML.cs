using GenerateEventXML.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateEventXML.Logic
{
  internal class GenerateXML
  {
    //Generate XML-File with XML text writer
    public static void GenerateXMLFile(List<Event> events)
    {
      //Create XML-File
      using System.IO.StreamWriter file = new("Events.xml");
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
        _idCounter++;
        writer.WriteStartElement("post");
        writer.WriteElementString("id", _idCounter.ToString());
        writer.WriteElementString("Title", e.EventTitle);
        writer.WriteElementString("Content", e.Content);
        writer.WriteElementString("imic_event_start_dt", e.DateTimeStart);
        writer.WriteElementString("imic_event_end_dt", e.DateTimeEnd);
        writer.WriteElementString("imic_event_address2", e.SelectedLocation);
        writer.WriteElementString("EventCategories", e.SelectedEventCategorie);
        writer.WriteEndElement();
      }

      //Close XML-Document
      writer.WriteEndElement();
      writer.WriteEndDocument();
      writer.Close();
    }
  }
}
