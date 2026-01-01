using BimTools.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Elements
{
    public class Wall : BimElement
    {
        public override string Category => "Wall";

        public Curve Axis { get; set; }
        public double Height { get; set; }
        public double Thickness { get; set; }

        public override Brep GenerateGeometry()
        {
            return Geometry.WallGeometry.Build(this);
        }
    }
}
