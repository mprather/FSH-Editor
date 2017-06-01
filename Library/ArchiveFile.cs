﻿/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Collections.Generic;
using System.IO;

namespace FSH {

	/// <summary>
  /// Main class for interacting with Archive.FSH files. 
  /// </summary>
  public class ArchiveFile : SerializableData {

		private byte[] header;
		private short flobCount;
		private short a;
		private short b;
		private short c;
		private short d;
		private short e;

		public List<Flob> Flobs { get; set; }

		public ArchiveFile() {
			
			this.header = new byte[16];
			this.header = System.Text.Encoding.ASCII.GetBytes("RL90 FLASH FILE\0");
			
			this.flobCount  = 0;
			this.a      = 0;
			this.b      = 0;
			this.c      = 1;
			this.d      = 1;
			this.e      = 1;

			//this.Headers = new List<FlobHeader>();
			this.Flobs  = new List<Flob>();

		}
		
		public override void Deserialize(BinaryReader reader) {
      
      byte[] verifyHeader = reader.ReadBytes(16);

      for (int i = 0; i < verifyHeader.Length; i++) {
        if (verifyHeader[i] != header[i]) {
          throw new ApplicationException("Attempting to read file with unknown format");
        }
      }

			this.flobCount  = reader.ReadInt16();
			this.a          = reader.ReadInt16();
			this.b          = reader.ReadInt16();
			this.c          = reader.ReadInt16();
			this.d          = reader.ReadInt16();
			this.e          = reader.ReadInt16();

			for (int i = 0; i < this.flobCount; i++) {

				Flob flob = new Flob();
				flob.StartOffset = 28 + i * Flob.BoundarySize;
				flob.Deserialize(reader);

				this.Flobs.Add(flob);

			}

			System.Diagnostics.Debug.Assert(this.flobCount == this.Flobs.Count);

		}  // End of Deserialize
		
		public override void Serialize(BinaryWriter writer) {

			this.flobCount = (short) this.Flobs.Count;

			writer.Write(this.header);
			writer.Write(this.flobCount);
			writer.Write(this.a);
			writer.Write(this.b);
			writer.Write(this.c);
			writer.Write(this.d);
			writer.Write(this.e);

			this.Flobs.ForEach(f => f.Serialize(writer));

		}  // End of Serialize

  }  // End of ArchiveFile class

}
