using BimTools.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Elements
{
    /// <summary>
    /// wall element representation, it has axis, properties, 
    /// and a method to generate its 3D shape
    /// </summary>

    public class Wall : BimElement
    {
        public override BimCategory Category => BimCategory.walls;

        public Curve Axis { get; set; }
        public double Height { get; set; }
        public double Thickness { get; set; }

        public override Brep GenerateGeometry()
        {
            return Geometry.WallGeometry.Build(this);
        }
    }
}
