using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {
        public static readonly List<Item> items = new List<Item>();
        public static readonly List<Monster> monsters = new List<Monster>();
        public static readonly List<Quest> quests = new List<Quest>();
        public static readonly List<Location> locations = new List<Location>();

        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        public const int ITEM_ID_DUAL_WIELD_LIGHTSABER = 11;
        public const int ITEM_ID_QUEST2_PASS = 12;

        public const float UNSELLABLE_ITEM_PRICE = -1f;

        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
        public const int QUEST_ID_CHAINED_QUEST_1 = 3;
        public const int QUEST_ID_CHAINED_QUEST_2 = 4;


        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMIST_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;

        static World()
        {
            populateItems();
            populateMonsters();
            populateQuests();
            populateLocations();
        }
        private static void populateItems()
        {
            items.Add(new Item(ITEM_ID_RAT_TAIL, "rat tail", "rat tails",15.99f));
            items.Add(new Item(ITEM_ID_FUR, "fur", "furs",5.00f));
            items.Add(new Item(ITEM_ID_SNAKE_FANG, "snake fang", "snake fangs",25.69f));
            items.Add(new Item(ITEM_ID_SNAKESKIN, "snake skin", "snake skins",49.98f));
            items.Add(new Weapon(ITEM_ID_CLUB, "club", "clubs", 10, 3,1.11f));
            items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "healing potion", "healing potions", 5,5.27f));
            items.Add(new Item(ITEM_ID_SPIDER_FANG, "spider fang", "spider fangs",6.77f));
            items.Add(new Item(ITEM_ID_SPIDER_SILK, "spider silk", "spider silks",9.99f));
            items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "adventurer pass", "adventurer passes",UNSELLABLE_ITEM_PRICE)); 
            items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "rusty sword", "rusty swords", 5, 0,99.00f));
            items.Add(new Weapon(ITEM_ID_DUAL_WIELD_LIGHTSABER,"dual wield light saber", "dual wield light sabers", 10000,10000,10000000f));
        }
        private static void populateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "rat", 3, 5, 10, 3, 3);
            rat.lootTable.Add(new LootItem(itemByID(ITEM_ID_RAT_TAIL), 75, true));
            rat.lootTable.Add(new LootItem(itemByID(ITEM_ID_DUAL_WIELD_LIGHTSABER), 100, false));
            monsters.Add(rat);
            Monster snake = new Monster(MONSTER_ID_SNAKE, "snake", 5, 5, 10, 3, 3);
            snake.lootTable.Add(new LootItem(itemByID(ITEM_ID_SNAKE_FANG), 75, true));
            snake.lootTable.Add(new LootItem(itemByID(ITEM_ID_SNAKESKIN), 75, false));
            monsters.Add(snake);
            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "giant spider", 1000, 1000, 1000, 1000000, 100000);
            giantSpider.lootTable.Add(new LootItem(itemByID(ITEM_ID_SPIDER_SILK), 90, true));
            giantSpider.lootTable.Add(new LootItem(itemByID(ITEM_ID_SPIDER_FANG), 90, false));
            giantSpider.lootTable.Add(new LootItem(itemByID(ITEM_ID_DUAL_WIELD_LIGHTSABER), 50, false));
            monsters.Add(giantSpider);
        }
        private static void populateQuests()
        {
            Quest clearAlchemistGarden = new Quest(QUEST_ID_CLEAR_ALCHEMIST_GARDEN, "clear the alchemist's garden", "kill 5 rats to pass this quest", 5, 50);
            clearAlchemistGarden.questCompletionItems.Add(new QuestCompletionItem(itemByID(ITEM_ID_RAT_TAIL), 5));
            clearAlchemistGarden.rewardItem = itemByID(ITEM_ID_HEALING_POTION);
            Quest clearFarmerField = new Engine.Quest(QUEST_ID_CLEAR_FARMERS_FIELD, "clear the farmer's field", "kill 1 spider to pass", 500, 5000);
            clearFarmerField.questCompletionItems.Add(new QuestCompletionItem(itemByID(ITEM_ID_SNAKESKIN), 5));
            clearFarmerField.rewardItem = itemByID(ITEM_ID_ADVENTURER_PASS);

            quests.Add(clearAlchemistGarden);
            quests.Add(clearFarmerField);
        }
        private static void populateLocations()
        {
            Location home = new Engine.Location(LOCATION_ID_HOME, "Home", "Your house. Mi casa e su casa");
            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town Square", "Town square. You see a famous person");
            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard Post", "Some next level bodyguard is here. LETS START SOME BEEF BRO",itemByID(ITEM_ID_ADVENTURER_PASS));
            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist Hut", "There are many strange potions here dawg. watch your back bro");
            Location alchemistGarden = new Location(LOCATION_ID_ALCHEMIST_GARDEN, "Alchemist Garden", "This place looks like shit. Theres a bunch of rat shit here bro watch your step i dont want to get my suede couch all rat shitty");
            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farm House", "Theres a small farmhouse with a farmer out front");
            Location farmField = new Location(LOCATION_ID_FARM_FIELD, "Farm Field", "Theres some plants bro. big deal",null,5);
            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "its a fuckin bridge bro. u trust bridges? u trust me? i dont trust bridges, so dont trust me bro");
            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Spider Field", "WTF u doin here bro? Theres no spiderman here bro. Ur gunna get killed bro. sliced and diced bro. like my moms risotto bro",null,10);

            alchemistHut.questAvailableHere = questByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);
            farmhouse.questAvailableHere = questByID(QUEST_ID_CLEAR_FARMERS_FIELD);
            alchemistGarden.monsterLivingHere = monsterByID(MONSTER_ID_RAT);
            farmField.monsterLivingHere = monsterByID(MONSTER_ID_SNAKE);
            spiderField.monsterLivingHere = monsterByID(MONSTER_ID_GIANT_SPIDER);

            home.locationNorth = townSquare;

            townSquare.locationNorth = alchemistHut;
            townSquare.locationEast = guardPost;
            townSquare.locationWest = farmhouse;
            townSquare.locationSouth = home;

            farmhouse.locationWest = farmField;
            farmhouse.locationEast = townSquare;

            farmField.locationEast = farmhouse;

            alchemistHut.locationNorth = alchemistGarden;
            alchemistHut.locationSouth = townSquare;

            alchemistGarden.locationSouth = alchemistHut;

            guardPost.locationWest = townSquare;
            guardPost.locationEast = bridge;

            bridge.locationEast = spiderField;
            bridge.locationWest = guardPost;

            spiderField.locationWest = bridge;

            Vendor bobTheRat = new Engine.Vendor("Bob the Rat");
            bobTheRat.addItemtoInventory(itemByID(ITEM_ID_RAT_TAIL), 5);
            bobTheRat.addItemtoInventory(itemByID(ITEM_ID_FUR), 10);
            townSquare.vendorWorkingHere = bobTheRat;

            Vendor stanTheSnake = new Vendor("Stan the Snake");
            stanTheSnake.addItemtoInventory(itemByID(ITEM_ID_SNAKESKIN), 10);
            stanTheSnake.addItemtoInventory(itemByID(ITEM_ID_SNAKE_FANG), 10);
            bridge.vendorWorkingHere = stanTheSnake;


            locations.Add(home);
            locations.Add(townSquare);
            locations.Add(guardPost);
            locations.Add(bridge);
            locations.Add(spiderField);
            locations.Add(farmField);
            locations.Add(farmhouse);
            locations.Add(alchemistGarden);
            locations.Add(alchemistHut);
        }
        public static Item itemByID(int id)
        {
           foreach(Item i in items){
                if(i.id == id)
                {
                    return i;
                }
            }
            return null;
        }
        public static Monster monsterByID(int id)
        {
            foreach (Monster m in monsters)
            {
                if (m.id == id)
                {
                    return m;
                }
            }
            return null;
        }
        public static Quest questByID(int id)
        {
            foreach (Quest q in quests)
            {
                if (q.id == id)
                {
                    return q;
                }
            }
            return null;
        }
        public static Location locationByID(int id)
        {
            foreach (Location l in locations)
            {
                if (l.id == id)
                {
                    return l;
                }
            }
            return null;
        }
    }
}