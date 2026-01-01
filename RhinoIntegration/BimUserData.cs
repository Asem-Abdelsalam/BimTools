using Rhino.DocObjects.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.RhinoIntegration
{
    public class BimUserData :UserData
    {
        public Guid ElementId;
        public string Category;

        public override string Description => "BimTools ElementData";
        public override bool ShouldWrite => true;

        protected override void OnDuplicate(UserData source)
        {
            if (source is BimUserData src)
            {
                ElementId = src.ElementId;
                Category = src.Category;
            }
        }
    }
}
