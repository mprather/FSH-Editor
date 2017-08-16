/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather
Info: http://www.okeanvoyaging.com/fsh-editor-help-and-general-information

This software has been released under GPL v3.0 license. 

*/

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;

using Editor.ViewModel;

namespace Editor {
	
  /// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
	  
	  public MainWindow() {
			InitializeComponent();
		}

    private void SummaryGridDrop(object sender, DragEventArgs e) {

      if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (files.Length > 0) {
          ArchiveFileViewModel a = this.DataContext as ArchiveFileViewModel;
          a.OpenFile(files[0]);
        }

      }

    }  // End of SummaryGridDrop

    private void WindowLoaded(object sender, RoutedEventArgs e) {
      
      if (Properties.Settings.Default.UpgradeRequested) {
        // reference Properties.Settings.Default.GetPreviousVersion for selective upgrade
        Properties.Settings.Default.Upgrade();
        Properties.Settings.Default.UpgradeRequested = false;
        Properties.Settings.Default.Save();
      }

      if (String.IsNullOrEmpty(Properties.Settings.Default.MostRecentFile)) {
        this.textBlockMostRecent.Visibility = Visibility.Hidden;
      }

      ArchiveFileViewModel a = this.DataContext as ArchiveFileViewModel;

      if (Directory.Exists(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "opencpn"))) {
        a.CanSaveTrackAsLayer    = true;
        a.CanSaveWaypointAsLayer = true;
      } else {
        a.SaveTrackAsLayer       = false;
        a.SaveWaypointAsLayer    = false;
      }

    }  // End of WindowLoaded

    private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
      Properties.Settings.Default.Save();
    }  // End of WindowClosing

  }
}
