/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.Generic;
using System.IO;

namespace FSH {

  public class Group : SerializableData {

		private char[] rawName;
		
		private short rawNameLength;
		private short items;

    private string name                              = null;

		public string Name {
			get {
        if (this.name == null) {
          this.name = new string(this.rawName);
        }
        return this.name;
      }
			set {
        this.rawNameLength = (short) TrimString(value, ref this.name, ref this.rawName, false);
      }
		}  // End of property Name
		
    public List<WaypointReference> Waypoints { get; set; }

		public Group() {
      this.Waypoints = new List<WaypointReference>();
		}  // End of ctor

		public override ushort CalculateSize() {
		  
      int length = 2 + 2 + this.rawNameLength + 8*this.Waypoints.Count;
      
      this.Waypoints.ForEach(w => {
        length += w.CalculateSize();
      });

      return (ushort) length;

		}  // End of CalculateSize

		public override void Deserialize(BinaryReader reader) {

			this.rawNameLength  = reader.ReadInt16();
			this.items          = reader.ReadInt16();
			this.rawName        = reader.ReadChars(this.rawNameLength);

			System.Diagnostics.Debug.WriteLine(" (g) Name: " + this.Name + ", Items: " + this.items);

      // ============================================================================
      // Within groups, Waypoints are referenced without IDS. We need to read the 
      // collection of IDs first...
      // ============================================================================
      for (short i = 0; i < this.items; i++) {
        
        WaypointReference w = new WaypointReference(false) {
          ID = reader.ReadInt64()
        };

        this.Waypoints.Add(w);

			}

      // ============================================================================
      // Now that the IDs have been read, we will read the remaining portion of data 
      // that compromises the Waypoint reference...
      // ============================================================================
      for (short i = 0; i < this.items; i++) {
        this.Waypoints[i].Deserialize(reader);
			}
			
		}  // End of Deserialize

		public override void Serialize(BinaryWriter writer) {

			writer.Write(this.rawNameLength);
			writer.Write(this.items);
			writer.Write(this.rawName);

      this.Waypoints.ForEach(w => writer.Write(w.ID));
      this.Waypoints.ForEach(w => w.Serialize(writer));
			
		}  // End of Serialize

	}  // End of Group class

}
