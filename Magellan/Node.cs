using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* notes on DeltaV for node:
 * x = rad
 * z = prograde
 * y = normal
*/
namespace KSP_Magellan
{
    public class Node
    {
        public Vessel vessel;
        public enum nodeType
        {
            injection,
            circularize,
            adjust
        };
        private double UT;
        private Vector3d DV = new Vector3d();

        public void MakeNode(CelestialBody target, nodeType type)
        {
            if (type == nodeType.injection)
            {
                MakeInjectionNode(target);
            }
        }

        private void MakeInjectionNode(CelestialBody target)
        {
            double desiredPhase = Calc.PhaseAngleAtTransfer(target, vessel);
            double currentPhase = Calc.PhaseAngleNow(target, vessel);

            double difference = currentPhase > desiredPhase ? currentPhase - desiredPhase : 360 - desiredPhase;

            //not putting node in right place

            double targetangvel = 360/ 138984.38;   //mun's angular velocity
            double originangvel = 360 / vessel.orbit.period;    //vessel angular velocity

            double angveldif = Math.Abs(targetangvel - originangvel);

            UT = Planetarium.GetUniversalTime() + difference / angveldif;

            DV.z = Calc.TransferDeltaV(target, vessel);
            Commit();
        }

        private void MakeCircularizationNode(CelestialBody target)
        {

        }

        private void Commit()
        {
            vessel.patchedConicSolver.AddManeuverNode(UT);
            foreach (ManeuverNode mn in vessel.patchedConicSolver.maneuverNodes)
            {
                if (mn.UT == UT) mn.DeltaV = DV;
            }
            vessel.patchedConicSolver.UpdateFlightPlan();
        }
    }
}
