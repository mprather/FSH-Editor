/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Windows.Input;

namespace Editor.ViewModel {

  public abstract class MappingViewModel : PropertyChangedBase {
    
    public static string HtmlTemplate                = Properties.Resources.WaypointHtmlMapTemplate.Replace("{MapServiceKey}", Properties.Settings.Default.MapServiceKey);

    public ICommand CreateMapCommand {
      get {
        return new DelegateCommand<MappingViewModel>(
          "CreateMapCommand",
          parameter => {
            if (parameter != null) {
              parameter.CreateMap();
            }
          },
          DelegateCommand<RouteViewModel>.DefaultCanExecute
        );
      }
    }  // End of property CreateMapCommand

    protected virtual void CreateMap() {

      // Ensure the holding folder exists...
      System.IO.Directory.CreateDirectory(Properties.Resources.MapFolderName);

    }  // End of CreateMap

  }

}
