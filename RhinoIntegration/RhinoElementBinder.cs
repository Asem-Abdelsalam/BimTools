using BimTools.Core.Element;
using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.RhinoIntegration
{
    public static class RhinoElementBinder
    {
        public static Guid Add(RhinoDoc doc, IBimElement element)
        {
            var brep = element.GenerateGeometry();
            if (brep == null) return Guid.Empty;

            var attr = new ObjectAttributes();
            attr.UserData.Add(new BimUserData
            {
                ElementId = element.Id,
                Category = element.Category.ToString()
            });

            return doc.Objects.AddBrep(brep, attr);
        }

        public static void Replace(RhinoDoc doc, Guid objectId, IBimElement element)
        {
            var brep = element.GenerateGeometry();
            if (brep == null) return;

            doc.Objects.Replace(objectId, brep);
        }
    }
}
