using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// <summary>
    /// the rhino command logic (asks user to pick points, creates 
    /// a wall object then adds it to the document
    /// </summary>
    public class BimWallCommand : Command
    {

        public override string EnglishName => "BimWall";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            // 1️ Pick first point 
            var gp = new GetPoint();
            gp.SetCommandPrompt("Start wall");
            if (gp.Get() != GetResult.Point)
                return Result.Cancel;

            var lastpoint = gp.Point();

            // 2 Pick Second point, attach dynamic draw to secondpoint 
            var gpSecond = new GetPoint();
            gpSecond.SetCommandPrompt("Select next wall position");
            gpSecond.SetBasePoint(lastpoint, true);

            gpSecond.DynamicDraw += (sender, e) =>
            {
                var previewWall = new Wall
                {
                    Height = 3.0,
                    Thickness = 0.2,
                    Axis = new Line(lastpoint, e.CurrentPoint).ToNurbsCurve()
                };

                var brep = previewWall.GenerateGeometry();
                if (brep == null) return;

                e.Display.DrawBrepWires(brep, Color.Black);
            };

            if (gpSecond.Get() != GetResult.Point)
                return Result.Cancel;

            var currentPoint = gpSecond.Point();

            // 3 Create and add wall
            var wall = new Wall
            {
                Axis = new Line(lastpoint, currentPoint).ToNurbsCurve(),
                Height = 3.0,
                Thickness = 0.2
            };

            BimDatabase.Add(wall);
            RhinoElementBinder.Add(doc, wall);

            lastpoint = currentPoint;

            // 4 repeat point picking for additional wall segments
            while (true)
            {
                var gpNext = new GetPoint();
                gpNext.SetCommandPrompt("Select next wall position");
                gpNext.SetBasePoint(lastpoint, true);

                gpNext.DynamicDraw += (sender, e) =>
                {
                    var previewWall = new Wall
                    {
                        Height = 3.0,
                        Thickness = 0.2,
                        Axis = new Line(lastpoint, e.CurrentPoint).ToNurbsCurve()
                    };

                    var brep = previewWall.GenerateGeometry();
                    if (brep == null) return;

                    e.Display.DrawBrepWires(brep, Color.Black);
                };
                var res = gpNext.Get();

                if (res == GetResult.Point)
                {
                    currentPoint = gpNext.Point();

                    // Create add wall for this segment
                    var nextWall = new Wall
                    {
                        Axis = new Line(lastpoint, currentPoint).ToNurbsCurve(),
                        Height = 3.0,
                        Thickness = 0.2
                    };

                    BimDatabase.Add(nextWall);
                    RhinoElementBinder.Add(doc, nextWall);

                    lastpoint = currentPoint;
                    continue;
                }

                if (res == GetResult.Nothing) // enter pressed
                    break;
                return Result.Cancel;
            }

            // end command
            doc.Views.Redraw();
            return Result.Success;
        }
    }
}
