using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Player
    {
        public Player()
        {
            ChequeCards = new List<ChequeCard>();
        }
        public int ChequeSum { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public List<PropertyCard> properties { get; set; }
        public bool HasPassed { get; set; }
        public int Bet { get; set; }

        public List<ChequeCard> ChequeCards { get; set; }
        public override string ToString()
        {
            string str = "";
            foreach (var item in properties.OrderBy(c => c.Value))
            {
                str = str + " " + item.Name;
            }
            return Name + " Money = " + Money + str;
        }
    }
}
