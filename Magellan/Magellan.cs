using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Reflection;

namespace KSP_Magellan
{
    public class Magellan : PartModule
    {
        private Rect windowPos = new Rect(Screen.width / 2, Screen.height / 2, 10, 10);
        private Course shipCourse = new Course();
        private string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        void Driver()
        {
            shipCourse.vessel = vessel;
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
            if (state != StartState.Editor)
            {
                RenderingManager.AddToPostDrawQueue(3, new Callback(drawGUI));
            }
        }

        public void drawGUI()
        {
            if (vessel == FlightGlobals.ActiveVessel)
            {
                windowPos = GUILayout.Window(987456, windowPos, mainGUI, "Magellan v." + version, GUILayout.Width(1), GUILayout.Height(1));
            }
        }

        public void mainGUI(int windowID)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginHorizontal();
            {
                if(GUILayout.Button("Plot", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
                {
                    Driver();
                }
            } 
            GUILayout.EndHorizontal();
        }
    }    
}
