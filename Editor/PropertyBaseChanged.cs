/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor {
	
  public abstract class PropertyChangedBase : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged = null;

		protected virtual bool ThrowOnInvalidPropertyName {
			get;
			private set;
		}

		protected void OnPropertyChanged(string propertyName) {
			if (this.PropertyChanged != null) {
				VerifyPropertyName(propertyName);
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}  // End of RaisePropertyChanged

		/// <summary>
		/// Warns the developer if this object does not have
		/// a public property with the specified name. This 
		/// method does not exist in a Release build.
		/// </summary>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		protected void VerifyPropertyName(string propertyName) {

			// If you raise PropertyChanged and do not specify a property name,
			// all properties on the object are considered to be changed by the binding system.
			if (String.IsNullOrEmpty(propertyName))
				return;

			// Verify that the property name matches a real,  
			// public, instance property on this object.
			if (TypeDescriptor.GetProperties(this)[propertyName] == null) {

				string text = "Invalid property name: " + propertyName;

				if (this.ThrowOnInvalidPropertyName) {
					throw new ArgumentException(text);
				} else {
					Debug.Fail(text);
				}

			}

		}  // End of VerifyPropertyName

	}  // End of PropertyChangedBase class

}
