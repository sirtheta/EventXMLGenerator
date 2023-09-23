using GenerateEventXML.Logic;
using System.Windows;

namespace GenerateEventXML
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      ConfigData.ReadConfigData();
    }
  }
}
