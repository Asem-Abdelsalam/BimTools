using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Core
{
    // Base for all Bim elements 
    public interface IBimElement
    {
        Guid Id { get; }                                    // unique ID
        string Category { get; }                            // categories like walls, doors,..etc
        Brep GenerateGeometry();                            // Create A Brep object representing the element's geometry in 3D space
    }
}
