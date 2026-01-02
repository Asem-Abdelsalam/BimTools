using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Core
{
    /// public interface for all Bim elements 
    public interface IBimElement
    {
        Guid Id { get; }                     // unique ID
        BimCategory Category { get; }             // categories like walls, doors,..etc "ex: builtincategories in revit"
        Brep GenerateGeometry();             // Create A geometric representation
    }
}
// The category determines what an element can do.
// For example, only elements in certain categories
// can host others (e.g., Walls can host Doors) or be included in specific schedules. 
