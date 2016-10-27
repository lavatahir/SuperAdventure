using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class HealingPotion : Item
    {
        public int amountToHeal { get; set; }
        public HealingPotion(int id, string name, string namePlural, int amountToHeal, float price) : base(id, name, namePlural, price)
        {
            this.amountToHeal = amountToHeal;
        }
    }
}
