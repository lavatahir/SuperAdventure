using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;

namespace Engine
{
    public class Player : LivingCreature
    {
        private int _gold;
        private int _expPoints;
        private Location _currentLocation;
        public int level { get { return ((expPoints / 100) + 1); } }
        public BindingList<InventoryItem> inventory { get; set; }
        public BindingList<PlayerQuest> quests { get; set; }
        public Weapon currentWeapon { get; set; }
        public Monster currentMonster;

        public int gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("gold");
            }
        }
        public int expPoints
        {
            get { return _expPoints; }
            private set
            {
                _expPoints = value;
                OnPropertyChanged("expPoints");
                OnPropertyChanged("level");
            }
        }
        public List<Weapon> weapons
        {
            get { return inventory.Where(x => x.details is Weapon).Select(x => x.details as Weapon).ToList(); }
        }
        public List<HealingPotion> potions
        {
            get { return inventory.Where(x => x.details is HealingPotion).Select(x => x.details as HealingPotion).ToList(); }
        }
        public Location currentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged("currentLocation");
            }
        }
        private Player(int currentHitPoints, int maxHitPoints, int gold, int expPoints) : base(currentHitPoints, maxHitPoints)
        {
            this.gold = gold;
            this.expPoints = expPoints;
            inventory = new BindingList<InventoryItem>();
            quests = new BindingList<PlayerQuest>();
        }
        public static Player createDefaultPlayer()
        {
            Player player = new Player(10, 10, 20, 0);
            player.inventory.Add(new InventoryItem(World.itemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.currentLocation = World.locationByID(World.LOCATION_ID_HOME);
            return player;
        }
        public static Player CreatePlayerFromXMLString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();
                playerData.LoadXml(xmlPlayerData);
                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maxHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int expPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExpierencePoints").InnerText);
                Player player = new Player(currentHitPoints, maxHitPoints, gold, expPoints);
                player.currentLocation = World.locationByID(Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText));
                if(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null)
                {
                    int crntWpnID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                    player.currentWeapon = (Weapon)World.itemByID(crntWpnID);
                }
                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);
                    for(int i = 0; i < quantity; i++)
                    {
                        player.addItemtoInventory(World.itemByID(id));
                    }
                }
                foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);
                    PlayerQuest playerQuest = new PlayerQuest(World.questByID(id));
                    playerQuest.isCompleted = isCompleted;
                    player.quests.Add(playerQuest);
                }
                return player;
            }
            catch
            {
                return Player.createDefaultPlayer();
            }
        }
        
        private void raiseInventoryChangedEvent(Item item)
        {
            if(item is Weapon)
            {
                OnPropertyChanged("weapons");
            }
            if(item is HealingPotion)
            {
                OnPropertyChanged("potions");
            }
        }
        public void removeItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = inventory.SingleOrDefault(ii => ii.details.id == itemToRemove.id);
            if(item != null)
            {
                item.quantity -= quantity;
                if(item.quantity < 0)
                {
                    item.quantity = 0;
                }
                if(item.quantity == 0)
                {
                    inventory.Remove(item);
                }
                raiseInventoryChangedEvent(itemToRemove);
            }
        }
        public void addExpierencePoints(int expierencePointsToAdd)
        {
            expPoints += expierencePointsToAdd;
            maxHitPoints = level * 10;
        }
        public bool hasRequiredItemToEnterLocation(Location location)
        {
            if(location.itemRequiredToEnter == null)
            {
                return true;
            }
            return inventory.Any(ii => ii.details.id == location.itemRequiredToEnter.id);
        }
        public bool hasRequiredLevelToEnterLocation(Location location)
        {
            if(location.levelRequiredToEnter == level)
            {
                return true;
            }
            return false;
        }
        public bool hasThisQuest(Quest q)
        {
            return quests.Any(pq => pq.details.id == q.id);
        }
        public bool completedQuest(Quest q)
        {
            foreach(PlayerQuest pq in quests)
            {
                if(pq.details.id == q.id)
                {
                    return pq.isCompleted;
                }
            }
            return false;
        }
        public bool hasAllQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem qci in quest.questCompletionItems)
            {
                if (!inventory.Any(ii => ii.details.id == qci.details.id && ii.quantity >= qci.quantity))
                {
                    return false;
                }
            }
            return true;
        }
        public void removeQuestCompletionItem(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.questCompletionItems)
            {
                InventoryItem item = inventory.SingleOrDefault(ii => ii.details.id == qci.details.id);
                if(item != null)
                {
                    removeItemFromInventory(item.details, qci.quantity);
                }
            }
        }
        public void addItemtoInventory(Item addItem)
        {
            InventoryItem item = inventory.SingleOrDefault(ii => ii.details.id == addItem.id);
            if(item == null)
            {
                inventory.Add(new InventoryItem(addItem, 1));
            }
            else
            {
                item.quantity++;
            }
            raiseInventoryChangedEvent(addItem);
        }
        public void markQuestCompleted (Quest quest)
        {
            PlayerQuest playerQuest =  quests.SingleOrDefault(q => q.details.id == quest.id);
            if(playerQuest != null)
            {
                playerQuest.isCompleted = true;
            }
        }
        public void moveTo(Location newLocation)
        {
               if (!hasRequiredItemToEnterLocation(newLocation))
                {
                    RaiseMessage("You must have a " + newLocation.itemRequiredToEnter.name + " to enter " + newLocation.name);
                    return;
                }
            if (!hasRequiredLevelToEnterLocation(newLocation))
            {
                RaiseMessage("You must be level " + newLocation.levelRequiredToEnter + " to enter " + newLocation.name);
                return;
            }
                currentLocation = newLocation;
                currentHitPoints = maxHitPoints;

                if (newLocation.questAvailableHere != null)
                {
                    bool playerAlreadyHasQuest = hasThisQuest(newLocation.questAvailableHere);
                    bool playerAlreadyCompletedQuest = completedQuest(newLocation.questAvailableHere);
                    if (playerAlreadyHasQuest)
                    {
                        if (!playerAlreadyCompletedQuest)
                        {
                            bool playerHasAllItemsToCompleteQuest = hasAllQuestCompletionItems(newLocation.questAvailableHere);
                            if (playerHasAllItemsToCompleteQuest)
                            {
                                RaiseMessage("");
                                RaiseMessage("You completed the '" + newLocation.questAvailableHere.name + "' quest.");
                                removeQuestCompletionItem(newLocation.questAvailableHere);
                                RaiseMessage("You receive: ");
                                RaiseMessage(newLocation.questAvailableHere.rewardExpPoints.ToString() + " expierence points");
                                RaiseMessage(newLocation.questAvailableHere.rewardGold.ToString() + " gold");
                                RaiseMessage(newLocation.questAvailableHere.rewardItem.name, true);

                                addExpierencePoints(newLocation.questAvailableHere.rewardExpPoints);
                                gold += newLocation.questAvailableHere.rewardGold;
                                addItemtoInventory(newLocation.questAvailableHere.rewardItem);
                                markQuestCompleted(newLocation.questAvailableHere);
                            }
                        }
                    }
                    else
                    {
                        RaiseMessage("You receive the " + newLocation.questAvailableHere.name + " quest.");
                        RaiseMessage(newLocation.questAvailableHere.description);
                        RaiseMessage("To complete this quest, please return with: ");
                        foreach (QuestCompletionItem qci in newLocation.questAvailableHere.questCompletionItems)
                        {
                            if (qci.quantity == 1)
                            {
                                RaiseMessage(qci.quantity.ToString() + " " + qci.details.name);
                            }
                            else
                            {
                                RaiseMessage(qci.quantity.ToString() + " " + qci.details.namePlural);
                            }
                        }
                        RaiseMessage("");
                        quests.Add(new PlayerQuest(newLocation.questAvailableHere));
                    }
                }
            if (newLocation.monsterLivingHere != null)
            {
                RaiseMessage("You see a " + newLocation.monsterLivingHere.name);
                Monster standardMonster = World.monsterByID(newLocation.monsterLivingHere.id);
                currentMonster = new Monster(standardMonster.id, standardMonster.name, standardMonster.maxDamage, standardMonster.rewardExpPoints, standardMonster.rewardGold, standardMonster.currentHitPoints, standardMonster.maxHitPoints);
                foreach (LootItem li in standardMonster.lootTable)
                {
                    currentMonster.lootTable.Add(li);
                }
            }
            else
            {
                currentMonster = null;
            }
        }
        public void useWeapon(Weapon weapon)
        {
            if (canHit())
            {
                int damageToMonster = RandomNumberGenerator.numberBetween(weapon.minDamage + strength, weapon.maxDamage + strength);
                currentMonster.currentHitPoints -= damageToMonster;
                RaiseMessage("You hit the " + currentMonster.name + " for " + damageToMonster.ToString() + " points.");
                if (currentMonster.currentHitPoints <= 0)
                {
                    RaiseMessage("You defeated the " + currentMonster.name);
                    addExpierencePoints(currentMonster.rewardExpPoints);
                    RaiseMessage("You have received " + currentMonster.rewardExpPoints.ToString() + " xp points ");
                    gold += currentMonster.rewardGold;
                    RaiseMessage("You have receivved " + currentMonster.rewardGold.ToString() + " gold ");

                    List<InventoryItem> lootedItems = new List<InventoryItem>();
                    foreach (LootItem ii in currentMonster.lootTable)
                    {
                        if (RandomNumberGenerator.numberBetween(1, 100) <= ii.dropPercentage)
                        {
                            lootedItems.Add(new InventoryItem(ii.details, 1));
                        }
                    }
                    if (lootedItems.Count == 0)
                    {
                        foreach (LootItem ii in currentMonster.lootTable)
                        {
                            if (ii.isDefaultItem)
                            {
                                lootedItems.Add(new InventoryItem(ii.details, 1));
                            }
                        }
                    }
                    foreach (InventoryItem ii in lootedItems)
                    {
                        addItemtoInventory(ii.details);
                        if (ii.quantity == 1)
                        {
                            RaiseMessage("You have looted " + ii.quantity.ToString() + " " + ii.details.name);
                        }
                        else
                        {
                            RaiseMessage("You have looted " + ii.quantity.ToString() + " " + ii.details.namePlural);
                        }
                    }
                    checkKillQuestCompletion(World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN, World.ITEM_ID_RAT_TAIL, 5);
                    checkKillQuestCompletion(World.QUEST_ID_CLEAR_FARMERS_FIELD, World.ITEM_ID_SNAKESKIN, 5);
                    //updateLabels();
                    moveTo(currentLocation);
                }
                else
                {
                    monsterTurn();
                }
            }
            else
            {
                RaiseMessage("You missed!");
                monsterTurn();
            }
        }
        public void monsterTurn()
        {
            if (this.canHit())
            {
                int damageToPlayer = RandomNumberGenerator.numberBetween(0, currentMonster.maxDamage);
                RaiseMessage("The " + currentMonster.name + " did " + damageToPlayer.ToString() + " damage points.");
                currentHitPoints -= damageToPlayer;
                if (currentHitPoints <= 0)
                {
                    RaiseMessage("YOU GOT FUCKED BY " + currentMonster.name);
                    moveTo(World.locationByID(World.LOCATION_ID_HOME));
                }
            }
            else
            {
                RaiseMessage(currentMonster.name + " missed!");
            }
        }
        private void checkKillQuestCompletion(int questID, int itemID, int maxItems)
        {
            int numItems = 0;
            foreach (InventoryItem i in inventory)
            {
                if (i.details.id == itemID)
                {
                    numItems++;
                }
            }
            if (numItems == maxItems)
            {
                foreach (PlayerQuest pq in quests)
                {
                    if (pq.details.id == questID)
                    {
                        pq.isCompleted = true;
                        foreach (QuestCompletionItem qci in pq.details.questCompletionItems)
                        {
                            inventory.Add(new InventoryItem(qci.details, 1));
                        }
                        inventory.Add(new InventoryItem(pq.details.rewardItem, 1));
                    }
                }
            }
        }
        public void usePotion(HealingPotion hp)
        {
            currentHitPoints += hp.amountToHeal;
            if (currentHitPoints > maxHitPoints)
            {
                currentHitPoints = maxHitPoints;
            }
            removeItemFromInventory(hp, 1);
            RaiseMessage("you drink a " + hp.name);
        }
        private void checkQuestCompletion(int questID, int itemID, int maxItems)
        {
            int numItems = 0;
            foreach (InventoryItem i in inventory)
            {
                if (i.details.id == itemID)
                {
                    numItems++;
                }
            }
            if (numItems == maxItems)
            {
                foreach (PlayerQuest pq in quests)
                {
                    if (pq.details.id == questID)
                    {
                        pq.isCompleted = true;
                        foreach (QuestCompletionItem qci in pq.details.questCompletionItems)
                        {
                            inventory.Add(new InventoryItem(qci.details, 1));
                        }
                        inventory.Add(new InventoryItem(pq.details.rewardItem, 1));
                    }
                }
            }
        }
        
        public void moveNorth() {
           if(currentLocation.locationNorth != null){
                moveTo(currentLocation.locationNorth);
            }
        }
        public void moveSouth()
        {
            if (currentLocation.locationSouth != null)
            {
                moveTo(currentLocation.locationSouth);
            }
        }
        public void moveEast()
        {
            if (currentLocation.locationEast != null)
            {
                moveTo(currentLocation.locationEast);
            }
        }
        public void moveWest()
        {
            if (currentLocation.locationWest != null)
            {
                moveTo(currentLocation.locationWest);
            }
        }
        public string toXMLString()
        {
            XmlDocument playerData = new XmlDocument();
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);
            XmlNode crntHitPoints = playerData.CreateElement("CurrentHitPoints");
            crntHitPoints.AppendChild(playerData.CreateTextNode(this.currentHitPoints.ToString()));
            stats.AppendChild(crntHitPoints);
            XmlNode mxHtPts = playerData.CreateElement("MaximumHitPoints");
            mxHtPts.AppendChild(playerData.CreateTextNode(this.maxHitPoints.ToString()));
            stats.AppendChild(mxHtPts);
            XmlNode gld = playerData.CreateElement("Gold");
            gld.AppendChild(playerData.CreateTextNode(this.gold.ToString()));
            stats.AppendChild(gld);
            XmlNode expierencePts = playerData.CreateElement("ExpierencePoints");
            expierencePts.AppendChild(playerData.CreateTextNode(this.expPoints.ToString()));
            stats.AppendChild(expierencePts);
            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.currentLocation.id.ToString()));
            stats.AppendChild(currentLocation);
            if(currentWeapon != null)
            {
                XmlNode crntWpn = playerData.CreateElement("CurrentWeapon");
                crntWpn.AppendChild(playerData.CreateTextNode(this.currentWeapon.id.ToString()));
                stats.AppendChild(crntWpn);
            }
            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);
            foreach(InventoryItem ii in this.inventory)
            {
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");
                XmlAttribute idAtribute = playerData.CreateAttribute("ID");
                idAtribute.Value = ii.details.id.ToString();
                inventoryItem.Attributes.Append(idAtribute);
                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = ii.quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);
                inventoryItems.AppendChild(inventoryItem);
            }
            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);
            foreach(PlayerQuest pq in quests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = pq.details.id.ToString();
                playerQuest.Attributes.Append(idAttribute);
                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = pq.isCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);
                playerQuests.AppendChild(playerQuest);
            }
            return playerData.InnerXml;
        }
    }
}