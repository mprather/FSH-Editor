/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/


using FSH;

namespace Editor.ViewModel {

  public class TrackPointViewModel : PropertyChangedBase {

    private TrackPoint trackpoint;

    public double Latitude {
      get {
        return trackpoint.Latitude;
      }
    }

    public double Longitude {
      get {
        return trackpoint.Longitude;
      }
    }

    public TrackPointViewModel (TrackPoint trackpoint) {
      this.trackpoint = trackpoint;
    }

  }
}
