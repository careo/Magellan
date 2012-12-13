using System;
using UnityEngine;

namespace KspMagellan
{
	public class Calc
	{
		public static double PhaseAngleAtTransfer (CelestialBody target, Vessel vessel)   //phase angle when transfer should start
		{
			double target_alt = AltitudeOfTarget (target, Planetarium.GetUniversalTime ());
			double origin_alt = vessel.mainBody.GetAltitude (vessel.findWorldCenterOfMass ());
			double u = target.referenceBody.gravParameter;

			double th = Math.PI * Math.Sqrt (Math.Pow (origin_alt + target_alt, 3) / (8 * u));
			double phase = (180 - Math.Sqrt (u / target_alt) * (th / target_alt) * (180 / Math.PI));
			while (phase < 0)
				phase += 360;
			return phase % 360;
		}

		public static double PhaseAngleAtTime (CelestialBody target, Vessel vessel, double t)
		{
			double ut = Planetarium.GetUniversalTime () + t;
			Vector3d targetvector = target.orbit.getRelativePositionAtUT (ut);
			Vector3d originvector = vessel.orbit.getRelativePositionAtT (ut);

			double phase = Angle2d (originvector, targetvector);

			originvector = Quaternion.AngleAxis (90, Vector3d.forward) * originvector;

			if (Angle2d (originvector, targetvector) > 90)
				phase = 360 - phase;

			return phase;
		}

		public static double AltitudeOfTarget (CelestialBody target, double time)
		{
			return 12000000;
		}

		public static double TransferDeltaV (CelestialBody target, Vessel vessel)
		{
			double radius = target.referenceBody.Radius;
			double u = target.referenceBody.gravParameter;
			//double d_alt = Altitude_of_Target(target, 0);
			double d_alt = 12000000;
			double alt = (vessel.mainBody.GetAltitude (vessel.findWorldCenterOfMass ())) + radius;
			double v = Math.Sqrt (u / alt) * (Math.Sqrt ((2 * d_alt) / (alt + d_alt)) - 1);
			return Math.Abs ((Math.Sqrt (u / alt) + v) - vessel.orbit.GetVel ().magnitude);
		}

		public static double PhaseAngleNow (CelestialBody target, Vessel vessel)
		{
			double ut = Planetarium.GetUniversalTime ();
			Vector3d targetvector = target.orbit.getRelativePositionAtUT (ut);
			Vector3d originvector = vessel.orbit.getRelativePositionAtT (ut);

			double phase = Angle2d (originvector, targetvector);

			originvector = Quaternion.AngleAxis (90, Vector3d.forward) * originvector;

			if (Angle2d (originvector, targetvector) > 90)
				phase = 360 - phase;

			return phase;
		}

		public static double Angle2d (Vector3d vector1, Vector3d vector2)   //projects two vectors to 2D plane and returns angle between them from 0-180
		{
			Vector3d v1 = Vector3d.Project (new Vector3d (vector1.x, 0, vector1.z), vector1);
			Vector3d v2 = Vector3d.Project (new Vector3d (vector2.x, 0, vector2.z), vector2);
			return Vector3d.Angle (v1, v2);
		}
	}
}
