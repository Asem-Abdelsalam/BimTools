using System;
using System.Collections.Generic;
using BimTools.Core;
using BimTools.Elements;
using BimTools.RhinoIntegration;
using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace BimTools.Commands
{
    public class BimWallCommand : Command
    {

        public override string EnglishName => "BimWall";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var points = new List<Point3d>();

            // ─────────────────────────────────────────────
            // 1️⃣ Pick first point
            // ─────────────────────────────────────────────
            var gp = new GetPoint();
            gp.SetCommandPrompt("Start wall");
            if (gp.Get() != GetResult.Point)
                return Result.Cancel;

            points.Add(gp.Point());

            // Preview wall
            var previewWall = new Wall
            {
                Height = 3.0,
                Thickness = 0.2
            };

            // ─────────────────────────────────────────────
            // 2️⃣ Repeated point picking (polyline)
            // ─────────────────────────────────────────────
            while (true)
            {
                var gpNext = new GetPoint();
                gpNext.SetCommandPrompt("Next point (Enter to finish)");
                gpNext.SetBasePoint(points[points.Count - 1], true);

                gpNext.DynamicDraw += (sender, e) =>
                {
                    if (points.Count < 1) return;

                    var tempPts = new List<Point3d>(points)
                    {
                        e.CurrentPoint
                    };

                    var pl = new Polyline(tempPts);
                    if (!pl.IsValid || pl.Count < 2)
                        return;

                    previewWall.Axis = pl.ToPolylineCurve();

                    var brep = previewWall.GenerateGeometry();
                    if (brep == null) return;

                    e.Display.DrawBrepWires(brep, System.Drawing.Color.Black);
                };

                var res = gpNext.Get();

                if (res == GetResult.Point)
                {
                    points.Add(gpNext.Point());
                    continue;
                }

                if (res == GetResult.Nothing) // Enter pressed
                    break;

                return Result.Cancel;
            }

            // Need at least 2 points
            if (points.Count < 2)
                return Result.Cancel;

            // ─────────────────────────────────────────────
            // 3️⃣ Create final wall
            // ─────────────────────────────────────────────
            var polyline = new Polyline(points);
            var axis = polyline.ToPolylineCurve();

            var wall = new Wall
            {
                Axis = axis,
                Height = 3.0,
                Thickness = 0.2
            };

            BimDatabase.Add(wall);
            RhinoElementBinder.Add(doc, wall);

            doc.Views.Redraw();
            return Result.Success;
        }
    }
}
