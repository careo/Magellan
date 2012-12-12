using System.Collections.Generic;

namespace KSP_Magellan
{
    public class Course
    {
        public Vessel vessel;
        private bool plotted = false;
        private List<Node> courseNodes = new List<Node>();

        public void PlotCourse(CelestialBody target)
        {
            courseNodes.Add(new Node());
            courseNodes[0].vessel = vessel;
            courseNodes[0].MakeNode(target, Node.nodeType.injection);

            plotted = true;
        }
    }
}
