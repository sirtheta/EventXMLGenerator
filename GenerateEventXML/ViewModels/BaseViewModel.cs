using Notifications.Wpf.Core;
using System;
using GenerateEventXML.Common;

namespace GenerateEventXML.ViewModels
{
  internal abstract class BaseViewModel : Notify
  {

    internal static void ShowNotification(string titel, string message, NotificationType type, int displayTime = 2)
    {
      var notificationManager = new NotificationManager();
      notificationManager.ShowAsync(new NotificationContent { Title = titel, Message = message, Type = type },
              areaName: "WindowArea", expirationTime: new TimeSpan(0, 0, displayTime));
    }
  }
}
