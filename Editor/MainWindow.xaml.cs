/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather
Info: http://www.okeanvoyaging.com/fsh-editor-help-and-general-information

This software has been released under GPL v3.0 license. 

*/

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

		private void WindowLoaded(object sender, RoutedEventArgs e) {
    
      try {
				using (StreamReader reader = new StreamReader(new IsolatedStorageFileStream("FSHEditor.LastFile", FileMode.Open, IsolatedStorageFile.GetUserStoreForAssembly()))) {
					App.Current.Properties["ArchiveFile"] = reader.ReadLine();
				}
			}
			catch {
				this.textBlockMostRecent.Visibility = Visibility.Hidden;
			}

		}  // End of WindowLoaded

    private void SummaryGridDrop(object sender, DragEventArgs e) {

      if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (files.Length > 0) {
          ArchiveFileViewModel a = this.DataContext as ArchiveFileViewModel;
          a.OpenFile(files[0]);
        }

      }

    }  // End of SummaryGridDrop

	}
}
