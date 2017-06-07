/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.IO;

namespace FSH {

  public class Block : SerializableData {
	  
		public ushort DataLength { get; set; }

		public ulong ID { get; set; }

		public BlockType Type { get; set; }

		public ushort Status { get; set; }

		public SerializableData Data { get; set; }

		public override ushort CalculateSize() {
      
      // NOTE: the size of the block is actually 14 bytes (block header) + data. 
      //       data schema does not account for the 14 bytes
      return (ushort)(Data.CalculateSize());

		}  // End of CalculateSize

		public override void Deserialize(BinaryReader reader) {
			
			this.DataLength = reader.ReadUInt16();
			this.ID         = reader.ReadUInt64();
			this.Type       = (BlockType) reader.ReadUInt16();
			this.Status     = reader.ReadUInt16();

			System.Diagnostics.Debug.WriteLine("[" + this.Type + "] ID: " + this.ID + ", Data Length: " + this.DataLength + ", Status: " + this.Status);

		}  // End of Deserialize

		public override void Serialize(BinaryWriter writer) {

      if (this.Data != null) {
        
        #if DEBUG
        ushort temp = CalculateSize();

        if (temp != this.DataLength) {
          System.Diagnostics.Debug.WriteLine("Error: size calculation differs. Expected: " + this.DataLength + " , Actual: " + temp + ", Type: " + this.Type);
        }
        #endif

        this.DataLength = CalculateSize();
      } else {
        System.Diagnostics.Debug.Assert (this.DataLength == 65535, "Unexpected DataLength value");
      }

			writer.Write(this.DataLength);
			writer.Write(this.ID);
			writer.Write((ushort)this.Type);
			writer.Write(this.Status);

			if (this.Data != null) {
				
			  this.Data.Serialize(writer);

				if (this.DataLength % 2 != 0) {
					
					// ----------------------------------------------------------------------
					// Alternative: this puts 0x0 instead of 0xff
					// writer.BaseStream.Position += 1;
					// ----------------------------------------------------------------------
					writer.Write((byte)0xFF);

				}

			} else {

			  if (this.Type != BlockType.Last) {
				  throw new ApplicationException("No data but block is not Last");
				}

			}

		}  // End of Serialize

	}  // End of Block class

}
