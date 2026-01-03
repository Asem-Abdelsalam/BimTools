using BimTools.Elements;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Geometry
{
    /// <summary>
    /// /contains the logic that converts wall element data to 3d geometry
    /// </summary>
    public static class WallGeometry
    {
        public static Brep Build(Wall wall)
        {
            // Validate wall has required properties 
            if (wall?.Axis == null || wall.Thickness < 0 || wall.Height <= 0)
                return null;

            // Take the axis curve and offset it left and right by 0.5 wall thickness
            var plane = Plane.WorldXY;
            var left = wall.Axis.Offset(plane, wall.Thickness / 2, 0.01, CurveOffsetCornerStyle.Sharp);
            var right = wall.Axis.Offset(plane, -wall.Thickness / 2, 0.01, CurveOffsetCornerStyle.Sharp);
            if (left == null || right == null)
                return null;

            // Create an array that contain the two offsets 
            var curves = new Curve[]
            {
                left[0],
                right[0].DuplicateCurve()
            };
            curves[1].Reverse();

            // Connect the endpoints to form a closed profile
            var startLine = new Line(curves[0].PointAtEnd, curves[1].PointAtStart);
            var endLine = new Line(curves[1].PointAtEnd, curves[0].PointAtStart);

            // new array with the 4 curves and join them 
            var allCurves = new Curve[] { curves[0], startLine.ToNurbsCurve(), curves[1], endLine.ToNurbsCurve() };
            var joined = Curve.JoinCurves(allCurves);

            if (joined.Length != 1 || !joined[0].IsClosed)
                return null;

            var extrusion = Extrusion.Create(joined[0], wall.Height, true);
            return extrusion?.ToBrep();

        }
    }
}
