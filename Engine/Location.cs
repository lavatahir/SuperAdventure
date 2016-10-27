using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Location
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Item itemRequiredToEnter { get; set; }
        public Quest questAvailableHere { get; set; }
        public Monster monsterLivingHere { get; set; }
        public Location locationNorth { get; set; }
        public Location locationSouth { get; set; }
        public Location locationEast { get; set; }
        public Location locationWest { get; set; }
        public Vendor vendorWorkingHere { get; set; }
        public int levelRequiredToEnter { get; set; }

        public Location(int id, string name, string description, Item itemRequiredtoEnter =null,int levelRequiredToEnter = 1,Quest questAvailableHere = null, Monster monsterLivingHere = null)
        {
            this.name = name;
            this.id = id;
            this.description = description;
            this.itemRequiredToEnter = itemRequiredtoEnter;
            this.questAvailableHere = questAvailableHere;
            this.monsterLivingHere = monsterLivingHere;
            this.levelRequiredToEnter = levelRequiredToEnter;
        }
    }
}
