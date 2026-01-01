using BimTools.Elements;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Geometry
{
    public static class WallGeometry
    {
        public static Brep Build(Wall wall)
        {
            var plane = Plane.WorldXY;
            var left = wall.Axis.Offset(
                plane, wall.Thickness / 2, 0.01, CurveOffsetCornerStyle.Sharp);
            var right = wall.Axis.Offset(
                plane, -wall.Thickness / 2, 0.01, CurveOffsetCornerStyle.Sharp);
            if (left == null || right == null)
                return null;

            var curves = new Curve[]
            {
                left[0],
                right[0].DuplicateCurve()
            };
            curves[1].Reverse();
            // Connect the endpoints to form a closed profile
            var startLine = new Line(curves[0].PointAtEnd, curves[1].PointAtStart);
            var endLine = new Line(curves[1].PointAtEnd, curves[0].PointAtStart);

            var allCurves = new Curve[] { curves[0], startLine.ToNurbsCurve(), curves[1], endLine.ToNurbsCurve() };


            var joined = Curve.JoinCurves(allCurves);
            if (joined.Length != 1 || !joined[0].IsClosed)
                return null;

            var extrusion = Extrusion.Create(joined[0], wall.Height, true);
            return extrusion?.ToBrep();

        }
    }
}
