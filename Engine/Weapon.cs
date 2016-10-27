using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon : Item
    {
        public int maxDamage { get; set; }
        public int minDamage { get; set; }

        public Weapon(int id, string name, string namePlural, int maxDamage, int minDamage, float price) : base(id, name, namePlural, price)
        {
            this.maxDamage = maxDamage;
            this.minDamage = minDamage;
        }

    }
}
