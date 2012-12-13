using System.Collections.Generic;

namespace KspMagellan
{
	public class Course
	{
		public Vessel vessel;
		bool plotted = false;
		List<Node> courseNodes = new List<Node> ();

		public void PlotCourse (CelestialBody target)
		{
			courseNodes.Add (new Node ());
			courseNodes [0].vessel = vessel;
			courseNodes [0].MakeNode (target, Node.NodeType.Injection);

			plotted = true;
		}
	}
}
