using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLite.Interfaces {
    public interface IActor {
        string Name { get; set; }
        // used in calculating fov to determine max distance the actor is aware of
        int Awareness { get; set; }
    }
}
