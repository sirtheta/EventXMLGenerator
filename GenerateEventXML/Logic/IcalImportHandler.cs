using GenerateEventXML.DomainModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace GenerateEventXML.Logic
{
    internal static partial class IcalImportHandler 
    {
        private const string _importTimeFormat = "dd-MM-yyyy HH:mm";

        [GeneratedRegex(@"\bGD\b")]
        private static partial Regex GdRegex();

        internal static IEnumerable<Event> ImportFile(string icalText, DateTime startTime, string keyword)
        {
            List<Event> events = [];
            var calendar = Ical.Net.Calendar.Load(icalText);
            foreach (var calendarEvent in calendar!.Events)
            {
                if (calendarEvent.DtStart!.Value < startTime)
                    continue;
                if (calendarEvent.Description?.StartsWith(keyword) == true)
                {
                    var mappedEvent = new Event
                    {
                        EventTitle = GdRegex().Replace(calendarEvent.Summary ?? "", "Gottesdienst"),
                        DateTimeStart = calendarEvent.DtStart.Value.ToString(_importTimeFormat),
                        DateTimeEnd = calendarEvent.DtEnd!.Value.ToString(_importTimeFormat),
                    };

                    events.Add(mappedEvent);
                }
            }
            return events.OrderBy(e => DateTime.ParseExact(e.DateTimeStart, _importTimeFormat, CultureInfo.InvariantCulture));
        }
    }
}
