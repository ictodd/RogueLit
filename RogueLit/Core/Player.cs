using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLite.Core {
    public class Player : Actor{
        public Player() {
            Awareness = 15;
            Name = "Rogue";
            Colour = Colours.Player;
            Symbol = '@';
            X = 10;
            Y = 10;
        }
    }
}
