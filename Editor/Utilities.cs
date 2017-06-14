using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FSH;

namespace Editor {

  public static class Utilities {

    public static string TrimmedString(string value, bool isComment) {
      int maxLength = isComment ? SerializableData.MaximumCommentLength : SerializableData.MaximumNameLength;
      if (value.Length > maxLength) {
        value = value.Substring(0, maxLength);
      }
      return value;
    }  // End of TrimmedString

  }
}
