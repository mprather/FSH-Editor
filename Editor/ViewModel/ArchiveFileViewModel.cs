/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather
Info: http://www.okeanvoyaging.com/fsh-editor-help-and-general-information

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Input;

using FSH;

namespace Editor.ViewModel {

  public class ArchiveFileViewModel : PropertyChangedBase {

    private bool archivePresent                      = false;
    private bool saveButtonEnabled                   = true;

    private Model.FileManager fileManager            = new Model.FileManager();

    private Visibility operationalTabVisibility      = Visibility.Collapsed;
    private Visibility startSequenceVisibility       = Visibility.Visible;
    private Visibility waitSpinnerVisibility         = Visibility.Collapsed;

    private ICommand openMostRecentFileCommand       = null;

    public bool ArchivePresent {
      get {
        return this.archivePresent;
      }
      set {
        this.archivePresent = value;
        OnPropertyChanged("ArchivePresent");
      }
    }  // End of property ArchivePresent
    
    public string FileName {
      get {
        return this.fileManager.FileName;
      }
    }  // End of property FileName

    public long FileSize {
      get {
        return this.fileManager.FileSize;
      }
    }  // End of property FileSize

    public ObservableCollection<GroupViewModel> GroupViewModels { get; set; }

    public ICommand OpenMostRecentFileCommand {
      get {
        return openMostRecentFileCommand ?? (openMostRecentFileCommand = new DelegateCommand<ArchiveFileViewModel>("OpenMostRecentFileCommand", 
                                                                                                                   a => {
                                                                                                                     if (a != null) {
                                                                                                                       a.OpenMostRecentFile();
                                                                                                                     }
                                                                                                                   },
                                                                                                                   DelegateCommand<ArchiveFileViewModel>.DefaultCanExecute));
      }
    }  // End of OpenMostRecentFileCommand

    public Visibility OperationalTabVisibility {
     get {
        return this.operationalTabVisibility;
     }
     set {
        this.operationalTabVisibility = value;
        OnPropertyChanged("OperationalTabVisibility");
     }
    }  // End of OperationalTabVisibility

    public ObservableCollection<RouteViewModel> RouteViewModels { get; set; }

    public ICommand SaveArchiveFileCommand {
      get {

        return new DelegateCommand<ArchiveFileViewModel>(
            "SaveArchiveFileCommand",
            parameter => {
              if (parameter != null) {
                parameter.SaveFile();
              }
            },
            DelegateCommand<ArchiveFileViewModel>.DefaultCanExecute
        );

      }
    }  // End of property SaveArchiveFileCommand

    public bool SaveButtonEnabled {
      get {
        return this.saveButtonEnabled;
      }
      set {
        this.saveButtonEnabled = value;
        OnPropertyChanged("SaveButtonEnabled");
      }
    }  // End of property SaveButtonEnabled

    public SimpleWaypointsSummaryViewModel SimpleWaypointsSummaryViewModel { get; set; }

    public Visibility StartSequenceVisibility {
      get {
        return this.startSequenceVisibility;
      }
      set {
        this.startSequenceVisibility = value;
        OnPropertyChanged("StartSequenceVisibility");
      }
    }  // End of property StartSequenceVisibility

    public ObservableCollection<TrackMetadataViewModel> TrackMetadataViewModels { get; set; }

    public Visibility WaitSpinnerVisibility {
      get {
        return this.waitSpinnerVisibility;
      }
      set {
        this.waitSpinnerVisibility = value;
        OnPropertyChanged("WaitSpinnerVisibility");
      }
    }  // End of property WaitSpinnerVisibility

    public ArchiveFileViewModel() {
      
      this.GroupViewModels                 = new ObservableCollection<GroupViewModel>();
      this.RouteViewModels                 = new ObservableCollection<RouteViewModel>();
      this.TrackMetadataViewModels         = new ObservableCollection<TrackMetadataViewModel>();
      this.SimpleWaypointsSummaryViewModel = new SimpleWaypointsSummaryViewModel();

    }  // End of ctor

    public void OpenFile(string fileName) {
      
      // Update the UI...
      this.OperationalTabVisibility  = Visibility.Collapsed;
      this.WaitSpinnerVisibility     = Visibility.Visible;

      // Clear any existing data...
      this.GroupViewModels.Clear();
      this.RouteViewModels.Clear();
      this.TrackMetadataViewModels.Clear();
      this.SimpleWaypointsSummaryViewModel.SimpleWaypoints.Clear();

      // Fire off the task to load the file...
      Func<string, bool> asyncRequest = new Func<string, bool>(LoadArchiveFile);
      asyncRequest.BeginInvoke(fileName, CompleteLoadArchiveFile, asyncRequest);

    }  // End of OpenFile

    private void CompleteLoadArchiveFile(IAsyncResult result) {

      var caller = result.AsyncState as Func<string, bool>;

      bool success = caller.EndInvoke(result);
      
      if (success) {

        this.OperationalTabVisibility = Visibility.Visible;
        this.StartSequenceVisibility  = Visibility.Collapsed;
        this.WaitSpinnerVisibility    = Visibility.Collapsed;

        // Trigger update for properties off of the filemanager...
        OnPropertyChanged("FileName");
        OnPropertyChanged("FileSize");

        App.Current.Dispatcher.Invoke(() => {
          
          this.fileManager.ArchiveFile.Flobs.ForEach(f => {

            f.Blocks.ForEach(b => {

              if (b.Status != 0) {
                switch (b.Type) {
                  case BlockType.Group:
                    this.GroupViewModels.Add(new ViewModel.GroupViewModel(b.Data as Group, f));
                    break;
                  case BlockType.Route:
                    this.RouteViewModels.Add(new ViewModel.RouteViewModel(b.Data as Route));
                    break;
                  case BlockType.TrackMetadata:
                    this.TrackMetadataViewModels.Add(new ViewModel.TrackMetadataViewModel(b.Data, this.fileManager.ArchiveFile.Flobs));
                    break;
                  case BlockType.SimpleWaypoint:
                    this.SimpleWaypointsSummaryViewModel.SimpleWaypoints.Add(new ViewModel.SimpleWaypointViewModel(b.Data as SimpleWaypoint));
                    break;
                }
              } else {
                this.ArchivePresent = true;
              }

            });

          });  // End of foreach flob

        });  // End of dispatcher

      } else {

        this.OperationalTabVisibility = Visibility.Collapsed;
        this.StartSequenceVisibility  = Visibility.Visible;
        this.WaitSpinnerVisibility    = Visibility.Collapsed;

      }

    }  // End of CompleteLoadArchiveFile

    private void CompleteSaveArchiveFile(IAsyncResult result) {
      
      this.WaitSpinnerVisibility = Visibility.Collapsed;
      this.SaveButtonEnabled     = true;

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = Properties.Resources.ExportFolderName
      });

    }  // End of CompleteSaveArchiveFile

    private bool LoadArchiveFile(string fileName) {
      
      // Open the specified file...
      bool result = this.fileManager.Open(fileName);

      // If the read was OK, let's save the file name so that we can reuse the file later...
      if (result) {
        using (StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("FSHEditor.LastFile", FileMode.Create, IsolatedStorageFile.GetUserStoreForAssembly()))) {
          writer.WriteLine(fileName);
        }
      }

      return result;

    }  //  End of LoadArchiveFile

    private void OpenMostRecentFile() {

      OpenFile(App.Current.Properties["ArchiveFile"] as string);

    }  // End of OpenMostRecentFile

    private void SaveFile() {
      
      this.WaitSpinnerVisibility = Visibility.Visible;
      this.SaveButtonEnabled     = false;

      Action<string> asyncRequest = new Action<string>(SaveArchiveFile);
      asyncRequest.BeginInvoke("archive." + DateTime.Now.ToString("yyyy_MM_dd_HH_mmm") + ".fsh", 
                               CompleteSaveArchiveFile, 
                               asyncRequest);
      
    }  // End of SaveFile

    private void SaveArchiveFile(string fileName) {
      this.fileManager.Save(fileName);
    }  // End of SaveArchiveFile

  }

}
