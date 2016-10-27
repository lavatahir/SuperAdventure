﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class QuestCompletionItem
    {
        public Item details { get; set; }
        public int quantity { get; set; }
        public QuestCompletionItem(Item details, int quantity)
        {
            this.details = details;
            this.quantity = quantity;
        }
    }
}
