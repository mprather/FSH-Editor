﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Editor.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double DepthMeterOffset {
            get {
                return ((double)(this["DepthMeterOffset"]));
            }
            set {
                this["DepthMeterOffset"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Feet")]
        public global::Editor.Enums.DepthUnits DepthUnits {
            get {
                return ((global::Editor.Enums.DepthUnits)(this["DepthUnits"]));
            }
            set {
                this["DepthUnits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NauticalMiles")]
        public global::Editor.Enums.DistanceUnits DistanceUnits {
            get {
                return ((global::Editor.Enums.DistanceUnits)(this["DistanceUnits"]));
            }
            set {
                this["DistanceUnits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IncludeDepth {
            get {
                return ((bool)(this["IncludeDepth"]));
            }
            set {
                this["IncludeDepth"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AiOZFEZdEmq_wc2MzSWNRiLJeW3P_vHYTHVYivmFWzPtsnKFb-WwIsxTm3WTyCwe")]
        public string MapServiceKey {
            get {
                return ((string)(this["MapServiceKey"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string MostRecentFile {
            get {
                return ((string)(this["MostRecentFile"]));
            }
            set {
                this["MostRecentFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SaveTrackAsLayer {
            get {
                return ((bool)(this["SaveTrackAsLayer"]));
            }
            set {
                this["SaveTrackAsLayer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SaveWaypointAsLayer {
            get {
                return ((bool)(this["SaveWaypointAsLayer"]));
            }
            set {
                this["SaveWaypointAsLayer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UpgradeRequested {
            get {
                return ((bool)(this["UpgradeRequested"]));
            }
            set {
                this["UpgradeRequested"] = value;
            }
        }
    }
}
