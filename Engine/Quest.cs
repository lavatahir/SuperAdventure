using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Quest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int rewardExpPoints { get; set; }
        public int rewardGold { get; set; }
        public Item rewardItem { get; set; }
        public List<QuestCompletionItem> questCompletionItems { get; set; }

        public Quest(int id, string name, string description, int rewardExp, int rewardGold)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.rewardExpPoints = rewardExp;
            this.rewardGold = rewardGold;
            questCompletionItems = new List<QuestCompletionItem>();
        }
    }
}
