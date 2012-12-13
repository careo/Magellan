using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace KspMagellan
{
	public class Magellan : PartModule
	{
		Rect windowPos = new Rect (Screen.width / 2, Screen.height / 2, 10, 10);
		Course shipCourse = new Course ();
		string version = Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();

		void Driver ()
		{
			shipCourse.vessel = vessel;
			List<CelestialBody> allBodies = FlightGlobals.Bodies;
			bool nodeMade = false;
			if (!nodeMade) {
				foreach (CelestialBody body in allBodies) {
					if (body.name == "Mun") {
						shipCourse.PlotCourse (body);
					}
				}
			}
		}

		public override void OnStart (PartModule.StartState state)
		{
			base.OnStart (state);
			if (state != StartState.Editor) {
				RenderingManager.AddToPostDrawQueue (3, new Callback (DrawGui));
			}
		}

		public void DrawGui ()
		{
			if (vessel == FlightGlobals.ActiveVessel) {
				windowPos = GUILayout.Window (987456, windowPos, MainGui, "Magellan v." + version, GUILayout.Width (1), GUILayout.Height (1));
			}
		}

		public void MainGui (int windowID)
		{
			GUI.skin = HighLogic.Skin;
			GUILayout.BeginHorizontal ();
			{
				if (GUILayout.Button ("Plot", GUILayout.ExpandHeight (true), GUILayout.ExpandWidth (true))) {
					Driver ();
				}
			}
			GUILayout.EndHorizontal ();
		}
	}
}
