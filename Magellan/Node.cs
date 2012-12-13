using System;

/* notes on DeltaV for node:
 * x = rad
 * z = prograde
 * y = normal
*/
namespace KspMagellan
{
	public class Node
	{
		public Vessel vessel;
		public enum NodeType
		{
			Injection,
			Circularize,
			Adjust
        }
		;
		double ut;
		Vector3d dV = new Vector3d ();

		public void MakeNode (CelestialBody target, NodeType type)
		{
			if (type == NodeType.Injection) {
				MakeInjectionNode (target);
			}
		}

		void MakeInjectionNode (CelestialBody target)
		{
			double desiredPhase = Calc.PhaseAngleAtTransfer (target, vessel);
			double currentPhase = Calc.PhaseAngleNow (target, vessel);

			double difference = currentPhase > desiredPhase ? currentPhase - desiredPhase : 360 - desiredPhase;

			//not putting node in right place

			double targetangvel = 360 / 138984.38;           //mun's angular velocity
			double originangvel = 360 / vessel.orbit.period; //vessel angular velocity

			double angveldif = Math.Abs (targetangvel - originangvel);

			ut = Planetarium.GetUniversalTime () + difference / angveldif;

			dV.z = Calc.TransferDeltaV (target, vessel);
			Commit ();
		}

		void MakeCircularizationNode (CelestialBody target)
		{

		}

		void Commit ()
		{
			vessel.patchedConicSolver.AddManeuverNode (ut);
			foreach (ManeuverNode mn in vessel.patchedConicSolver.maneuverNodes) {
				if (mn.UT == ut)
					mn.DeltaV = dV;
			}
			vessel.patchedConicSolver.UpdateFlightPlan ();
		}
	}
}
