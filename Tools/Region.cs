using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string TreePath { get; set; }
    }
}
