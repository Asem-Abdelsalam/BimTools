using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Core
{
    /// the base class implementing IBimElement
    public abstract class BimElement : IBimElement
    {
        public Guid Id { get; } = Guid.NewGuid();
        public abstract BimCategory Category { get; }
        public abstract Brep GenerateGeometry();
    }
}
