using GenerateEventXML.Logic;
using System;

namespace GenerateEventXML.DomainModel
{
  public class Event
  {
    public string? EventTitle { get; set; }
    public string? Content { get; set; }
    public string DateTimeStart { get; set; }
    public string DateTimeEnd { get; set; }
    public string? Location { get; set; }
    public string? EventCategory { get; set; }

    public Event()
    {
      DateTimeStart = DateTime.Now.ToString(ConfigData.DateFormat) + " 09:30";
      DateTimeEnd = DateTime.Now.ToString(ConfigData.DateFormat) + " 12:00";
    }
  }
}
