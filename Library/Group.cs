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
        this.rawNameLength = (short) TrimString(value, ref this.name, ref this.rawName);
      }
		}  // End of property Name

		public List<long> WaypointIDs { get; set; }

		public List<Waypoint> Waypoints { get; set; }

		public Group() {
			this.WaypointIDs = new List<long>();
			this.Waypoints   = new List<Waypoint>();
		}  // End of ctor

		public override ushort CalculateSize() {
		  
      int length = 2 + 2 + this.rawNameLength + 8 *this.WaypointIDs.Count;
      
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

			for (short i = 0; i < this.items; i++) {
				this.WaypointIDs.Add(reader.ReadInt64());
			}
			
			for (short i = 0; i < this.items; i++) {
				
			  Waypoint wp = new Waypoint();
				wp.Deserialize(reader);
				
				this.Waypoints.Add(wp);

				//System.Diagnostics.Debug.WriteLine(wp.Data.Name + ": " + wp.Latitude + ", " + wp.Longitude);

			}
			
		}  // End of Deserialize

		public override void Serialize(BinaryWriter writer) {

			writer.Write(this.rawNameLength);
			writer.Write(this.items);
			writer.Write(this.rawName);

			this.WaypointIDs.ForEach(id => writer.Write(id));

			this.Waypoints.ForEach(wp => wp.Serialize(writer));
			
		}  // End of Serialize

	}  // End of Group class

}
