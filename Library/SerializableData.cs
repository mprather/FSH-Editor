/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.IO;

namespace FSH {

  public abstract class SerializableData {

    public const int MaximumStringLength             = 16;

    public virtual ushort CalculateSize() {
      
      // NOTE: if an inheritor does not override the method but is called 
      //       during size calculation, this exception will isolate the error
      throw new NotImplementedException();

    }  // End of CalculateSize

    public abstract void Deserialize(BinaryReader reader);

    public abstract void Serialize(BinaryWriter writer);

    /// <summary>
    /// Returns the string defined by null-terminator
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    protected string CleanString(char[] original) {

      int index = original.Length;
      for (int i = 0; i < original.Length; i++) {
        if (original[i] == 0x0) {
          index = i;
          break;
        }
      }

      return new string(original, 0, index);

    }  // End of CleanString

    /// <summary>
    /// Ensure all trailing chars are nulls
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    protected char[] NullPaddedString(string original) {

      char[] temp = new char[SerializableData.MaximumStringLength];
      original.CopyTo(0, temp, 0, original.Length);
      return temp;

    }  // End of NullPaddedString

    protected int TrimString(string input, ref string destination, ref char[] rawDestination) {

      if (input.Length > SerializableData.MaximumStringLength) {
        throw new ApplicationException();
      }

      rawDestination = input.ToCharArray();
      destination = input;

      return rawDestination.Length;

    }  // End of TrimString

  }  // End of SerializableData class

}
