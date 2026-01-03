using BimTools.Core.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimTools.Core
{
    public static class BimDatabase
    {
        private static readonly Dictionary<Guid, IBimElement> _elements = new Dictionary<Guid, IBimElement>();
        public static void Add(IBimElement element) 
        {
            _elements[element.Id] = element; 
        }
        public static IBimElement Get(Guid id)
        {
            return _elements.TryGetValue(id, out var e) ? e : null;
        }
        public static void Remove(Guid id)
        {
            _elements.Remove(id);
        }
    }
}
