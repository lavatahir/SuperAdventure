using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Engine
{
    public class Monster : LivingCreature
    {
        public int id { get; set; }
        public string name { get; set; }
        public int maxDamage { get; set; }
        public int rewardExpPoints { get; set; }
        public int rewardGold { get; set; }
        public List<LootItem> lootTable { get; set; }

        public Monster(int id, string name, int currentHitPoints, int maxHitPoints, int maxDamage, int rewardExpPoints, int rewardGold) : base(currentHitPoints, maxHitPoints)
        {
            this.id = id;
            this.name = name;
            this.maxDamage = maxDamage;
            this.rewardExpPoints = rewardExpPoints;
            this.rewardGold = rewardGold;
            lootTable = new List<LootItem>();
        }
    }
}
