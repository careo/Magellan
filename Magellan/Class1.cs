using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.IO;

namespace KSP_Magellan
{

    public class Magellan : PartModule
    {
        Course shipCourse = new Course();

        void Driver()
        {
            List<CelestialBody> allBodies = FlightGlobals.Bodies;
            bool nodeMade = false;
            if (!nodeMade)
            {
                foreach (CelestialBody body in allBodies)
                {
                    if (body.name == "Mun")
                    {
                        shipCourse.PlotCourse(body);
                    }
                }
            }
        }

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
        }
    }

    public class Node
    {

        public enum nodeType
        {
            injection,
            circularize,
            adjust
        };

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
        }

        private void MakeCircularizationNode(CelestialBody target)
        {

        }
    }

    public class Course : Node
    {
        private bool plotted = false;
        private List<Node> courseNodes = new List<Node>();

        public void PlotCourse(CelestialBody target)
        {
            courseNodes[0].MakeNode(target, Node.nodeType.injection);
            courseNodes[1].MakeNode(target, Node.nodeType.circularize);
        }
    }

    public class Calc
    {
        public static double PhaseAngleAtTransfer(CelestialBody target, Vessel vessel)   //phase angle when transfer should start
        {
            double target_alt = Altitude_of_Target(target, Planetarium.GetUniversalTime());
            double origin_alt = vessel.mainBody.GetAltitude(vessel.findWorldCenterOfMass());
            double u = target.referenceBody.gravParameter;

            double th = Math.PI * Math.Sqrt(Math.Pow(origin_alt + target_alt, 3) / (8 * u));
            double phase = (180 - Math.Sqrt(u / target_alt) * (th / target_alt) * (180 / Math.PI));
            while (phase < 0) phase += 360;
            return phase % 360;
        }

        public static double Altitude_of_Target(CelestialBody target, double time)
        {
            return 12000000;
        }
    }


}
