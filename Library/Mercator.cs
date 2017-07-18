/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;

namespace FSH {

  public sealed class Mercator {

    private const double Eccentricity                = 0.08181919;
    private const double IterationAccuracy           = 1.5e-8;
    private const double LatitudeScale               = 107.1709342;
    private const double MaximumIterations           = 32;
    private const double SemiMajorAxis               = 6378137;

    private Mercator() {
      // Hide the ctor
    }

    public static double Latitude(int north) {
      return Phi(north / Mercator.LatitudeScale) * 180 / System.Math.PI; 
    }  // End of Latitude

    public static double Longitude(int east) {
      return (double)east / 0x7fffffff * 180.0;
    }  // End of Longitude

    private static double Phi(double north) {

      double phi, phi0;

      int i;

      for (phi = 0, phi0 = Math.PI, i = 0; Math.Abs(phi - phi0) > IterationAccuracy && i < MaximumIterations; i++) {
        phi0 = phi;
        phi = ReversePhi(north, phi0);
      }

      return phi;

    }  // End of Phi

    private static double ReversePhi(double north, double phi0) {

      double esin = Eccentricity * Math.Sin(phi0);
      return Math.PI / 2 - 2.0 * Math.Atan(Math.Exp(-north / SemiMajorAxis) * Math.Pow((1 - esin) / (1 + esin), Eccentricity / 2));

    }  // End of ReversePhi

  }  // End of Mercator class

}
