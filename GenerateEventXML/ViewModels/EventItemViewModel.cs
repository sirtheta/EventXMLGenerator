using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenerateEventXML.DomainModel;
using GenerateEventXML.Logic;
using System.Collections.Generic;
using System.Linq;

namespace GenerateEventXML.ViewModels
{
  public record DeleteEventMessage(EventItemViewModel EventViewModel);
  public partial class EventItemViewModel : ObservableObject
  {
    private readonly Event _event;

    [ObservableProperty]
    private string? eventTitle;

    [ObservableProperty]
    private string? content;

    [ObservableProperty]
    private string dateTimeStart;

    [ObservableProperty]
    private string dateTimeEnd;

    [ObservableProperty]
    private string? selectedLocation;

    [ObservableProperty]
    private string? selectedEventCategory;

    public EventItemViewModel(Event evt)
    {
      _event = evt;

      // Initialize ViewModel properties from the model
      EventTitle = _event.EventTitle;
      Content = _event.Content;
      DateTimeStart = _event.DateTimeStart;
      DateTimeEnd = _event.DateTimeEnd;

      SetDefaultLocation();
      SetDefaultEventCategory();

      // Subscribe to property changes to update the model
      PropertyChanged += (s, e) => UpdateModel(e.PropertyName);
    }

    private void SetDefaultLocation()
    {
      if (string.IsNullOrEmpty(_event.Location))
      {
        _event.Location = Locations.FirstOrDefault();
        SelectedLocation = _event.Location;
      }
      else
      {
        SelectedLocation = _event.Location;
      }
    }

    private void SetDefaultEventCategory()
    {
      if (string.IsNullOrEmpty(_event.EventCategory))
      {
        _event.EventCategory = EventCategories.FirstOrDefault();
        SelectedEventCategory = _event.EventCategory;
      }
      else
      {
        SelectedEventCategory = _event.EventCategory;
      }
    }

    private void UpdateModel(string? propertyName)
    {
      switch (propertyName)
      {
        case nameof(EventTitle):
          _event.EventTitle = EventTitle;
          break;
        case nameof(Content):
          _event.Content = Content;
          break;
        case nameof(DateTimeStart):
          _event.DateTimeStart = DateTimeStart;
          break;
        case nameof(DateTimeEnd):
          _event.DateTimeEnd = DateTimeEnd;
          break;
        case nameof(SelectedLocation):
          _event.Location = SelectedLocation;
          break;
        case nameof(SelectedEventCategory):
          _event.EventCategory = SelectedEventCategory;
          break;
      }
    }

    // Get the underlying model if needed
    public Event GetEvent() => _event;

    public static List<string> Locations => ConfigData.Locations;
    public static List<string> EventCategories => ConfigData.Categories;

    [RelayCommand]
    private void DeleteEvent()
    {
      WeakReferenceMessenger.Default.Send(new DeleteEventMessage(this));
    }
  }
}
