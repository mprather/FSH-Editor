/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather
Info: http://www.okeanvoyaging.com/fsh-editor-help-and-general-information

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Editor {
	
  public class DelegateCommand<T> : ICommand {

		public event EventHandler CanExecuteChanged = null;

		private bool canExecuteCache = false;

		private string name = null;

		private readonly Predicate<T> canExecute = null;
		private readonly Action<T> executeAction = null;

		/// <summary>
		/// Command name is primarily used for debugging. It could arguably be removed from this class if needed.
		/// </summary>
		public string Name {
			[DebuggerStepThrough]
			get {
				return name;
			}
		}  // End of property Name

		public DelegateCommand(string name, Action<T> executeAction, Predicate<T> canExecute) {
			this.executeAction = executeAction;
			this.canExecute = canExecute;
			this.name = name;
		}  // End of constructor

		[DebuggerStepThrough]
		bool ICommand.CanExecute(object parameter) {

			System.Diagnostics.Debug.WriteLine("CanExecute<T>: " + Name + ", " + parameter);

			if (parameter == null &&
					typeof(T).IsValueType) {
				return canExecuteCache;
			}

			bool temp = canExecute((T)parameter);

			if (canExecuteCache != temp) {
				canExecuteCache = temp;
				if (CanExecuteChanged != null) {
					CanExecuteChanged(this, new EventArgs());
				}
			}

			return canExecuteCache;

		}  // End of CanExecute

		[DebuggerStepThrough]
		void ICommand.Execute(object parameter) {
			if (parameter == null) {
				executeAction(default(T));
			} else {
				executeAction((T)parameter);
			}
		}  // End of Execute

		[DebuggerStepThrough]
		internal static bool DefaultCanExecute(object parameter) {
			return true;
		}  // End of DefaultCanExecute

	}  // End of DelegateCommand

	public class DelegateCommand : ICommand {

		public event EventHandler CanExecuteChanged = null;

		private bool canExecuteCache = false;

		private string name = null;

		private readonly Func<bool> canExecute = null;
		private readonly Action executeAction = null;

		public string Name {
			[DebuggerStepThrough]
			get {
				return name;
			}
		}  // End of property Name

		public DelegateCommand(string name, Action executeAction, Func<bool> canExecute) {
			this.executeAction = executeAction;
			this.canExecute = canExecute;
			this.name = name;
		}  // End of constructor

		[DebuggerStepThrough]
		bool ICommand.CanExecute(object parameter) {

			System.Diagnostics.Debug.WriteLine("CanExecute: " + Name);

			bool temp = canExecute();

			if (canExecuteCache != temp) {
				canExecuteCache = temp;
				if (CanExecuteChanged != null) {
					CanExecuteChanged(this, new EventArgs());
				}
			}

			return canExecuteCache;

		}  // End of CanExecute

		[DebuggerStepThrough]
		void ICommand.Execute(object parameter) {
			executeAction();
		}  // End of Execute

		[DebuggerStepThrough]
		internal static bool DefaultCanExecute() {
			return true;
		}  // End of DefaultCanExecute

	}  // End of DelegateCommand

}
