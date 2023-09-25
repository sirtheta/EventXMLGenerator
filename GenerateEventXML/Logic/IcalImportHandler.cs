using GenerateEventXML.MainClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GenerateEventXML.Logic
{
  internal static class IcalImportHandler
  {
    private const string _importTimeFormat = "dd-MM-yyyy HH:mm";
    /// <summary>
    /// Importfunction for the ics file, will import all entrys starting with the given 
    /// keyword in the event description from the given start date on
    /// </summary>
    /// <param name="icalText"></param>
    /// <param name="startTime"></param>
    /// <returns></returns>
    internal static IEnumerable<Event> ImportFile(string icalText, DateTime startTime, string keyword)
    {
      List<Event> events = new();

      var calendar = Ical.Net.Calendar.Load(icalText);

      foreach (var calendarEvent in calendar.Events)
      {
        if (calendarEvent.DtStart.Value < startTime)
          continue;

        if (calendarEvent.Description?.StartsWith(keyword) == true)
        {
          var mappedEvent = new Event
          {
            EventTitle = calendarEvent.Summary,
            DateTimeStart = calendarEvent.DtStart.Value.ToString(_importTimeFormat),
            DateTimeEnd = calendarEvent.DtEnd.Value.ToString(_importTimeFormat),
          };
          events.Add(mappedEvent);
        }
      }

      return events.OrderBy(e => DateTime.ParseExact(e.DateTimeStart, _importTimeFormat, CultureInfo.InvariantCulture));
    }
  }
}
