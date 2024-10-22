using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;

namespace EchoesOfSerenity.World.Achievement;

public class Achievements
{
    public Dictionary<string, Achievement> AchievementList = [];
    public float CompletionTime = 0;

    public Achievements()
    {
        AchievementList.Add("punch_tree", new Achievement()
        {
            Name = "Getting Wood",
            Description = "Punch a tree to get wood",
            Icon = ContentManager.GetTexture("Content/UI/Tree.png")
        });
        
        AchievementList.Add("craft_work_bench", new Achievement()
        {
            Name = "Getting to Work",
            Description = "Use Wood to make a Work Bench, which can be used to craft more interesting things!",
            Icon = ContentManager.GetTexture("Content/Items/WorkBench.png")
        });

        AchievementList.Add("craft_wooden_tool", new Achievement()
        {
            Name = "Not Quite Stone Age",
            Description = "Use Wood to craft a Wooden Tool, allowing you to mine stone",
            Icon = ContentManager.GetTexture("Content/Items/WoodenPickaxe.png")
        });

        AchievementList.Add("obtain_stone", new Achievement()
        {
            Name = "Stoner",
            Description = "Using your Wooden Pickaxe, mine some stone",
            Icon = ContentManager.GetTexture("Content/Items/Stone.png")
        });

        AchievementList.Add("craft_stone_tool", new Achievement()
        {
            Name = "Stone Age",
            Description = "Use Wood and Stone to craft a Stone Tool, allowing you to mine Iron",
            Icon = ContentManager.GetTexture("Content/Items/StonePickaxe.png")
        });

        AchievementList.Add("obtain_coal", new Achievement()
        {
            Name = "Coal Miner",
            Description = "Obtain some coal, which can be used to make a Furnace",
            Icon = ContentManager.GetTexture("Content/Items/Coal.png")
        });

        AchievementList.Add("craft_furnace", new Achievement()
        {
            Name = "Average UK Summer",
            Description = "Use Stone and Coal to craft a Furnace, which can be used to smelt ores",
            Icon = ContentManager.GetTexture("Content/Items/Furnace.png")
        });
        
        AchievementList.Add("smelt_iron", new Achievement()
        {
            Name = "Acquire Hardware",
            Description = "Smelt Raw Iron in a Furnace to make Iron Ingots",
            Icon = ContentManager.GetTexture("Content/Items/IronIngot.png")
        });

        AchievementList.Add("craft_anvil", new Achievement()
        {
            Name = "Metal Worker",
            Description = "Use Iron Ingots to craft an Iron Anvil, which can be used to craft metal items",
            Icon = ContentManager.GetTexture("Content/Items/IronAnvil.png")
        });

        AchievementList.Add("craft_iron_tool", new Achievement()
        {
            Name = "Iron Age",
            Description = "Use Wood and Iron to craft a Iron Tool!",
            Icon = ContentManager.GetTexture("Content/Items/IronPickaxe.png")
        });

        AchievementList.Add("obtain_pebbles", new Achievement()
        {
            Name = "Pebble Collector",
            Description = "Obtain some pebbles, which can be placed",
            Icon = ContentManager.GetTexture("Content/Items/Pebbles.png")
        });
        
        AchievementList.Add("obtain_sugarcane", new Achievement()
        {
            Name = "Cane Collector",
            Description = "Obtain some sugar cane, which you can find on sand near water",
            Icon = ContentManager.GetTexture("Content/Items/SugarCane.png")
        });
        
        AchievementList.Add("eat_bread", new Achievement()
        {
            Name = "Greggs Fan",
            Description = "Eat some Cave Bread, a tasty (albeit stale) treat",
            Icon = ContentManager.GetTexture("Content/Items/CaveBread.png")
        });

        AchievementList.Add("eat_sugar", new Achievement()
        {
            Name = "Sweet Tooth",
            Description = "Eat some Sugar, good for a quick energy boost",
            Icon = ContentManager.GetTexture("Content/Items/Sugar.png")
        });

        AchievementList.Add("craft_75_items", new Achievement()
        {
            Name = "Big-time Crafter",
            Description = "Craft 75 items",
            StatID = "items_crafted",
            StatGoal = 75,
            Icon = ContentManager.GetTexture("Content/Items/WorkBench.png")
        });

        AchievementList.Add("place_50_tiles", new Achievement()
        {
            Name = "House Builder Gang",
            Description = "Place 50 tiles",
            StatID = "tiles_placed",
            StatGoal = 50,
            Icon = ContentManager.GetTexture("Content/Items/WoodPlank.png")
        });

        AchievementList.Add("trash_64_items", new Achievement()
        {
            Name = "Wasteful",
            Description = "Trash 64 items",
            StatID = "items_trashed",
            StatGoal = 64,
            Icon = ContentManager.GetTexture("Content/UI/TrashSlot.png")
        });

        AchievementList.Add("craft_bomb", new Achievement()
        {
            Name = "Demolition Expert",
            Description = "Use Sulfur and Iron to craft a bomb!",
            Icon = ContentManager.GetTexture("Content/Items/Bomb.png")
        });
        
        AchievementList.Add("walk_1000_blocks", new Achievement()
        {
            Name = "Marathon Runner",
            Description = "Walk 1000 blocks",
            StatID = "units_moved",
            StatGoal = 1000,
            Icon = ContentManager.GetTexture("Content/Items/Sugar.png")
        });

        AchievementList.Add("blow_up_200_tiles", new Achievement()
        {
            Name = "Bulldozer",
            Description = "Blow up 200 tiles with bombs",
            StatID = "tiles_blown_up",
            StatGoal = 200,
            Icon = ContentManager.GetTexture("Content/Items/Bomb.png")
        });
        
        AchievementList.Add("craft_jake", new Achievement()
        {
            Name = "Abyss Crafter",
            Description = "Craft a Jake Voodoo Doll, an ancient relic of the Abyss\nIt's said to be sweet to the taste, and high in carbs...",
            Icon = ContentManager.GetTexture("Content/Items/Jake.png")
        });
    }

    public void Update(Stats stats)
    {
        foreach ((string name, Achievement achievement) in AchievementList)
        {
            if (achievement.Completed)
                continue;

            if (!string.IsNullOrEmpty(achievement.StatID))
            {
                if (stats.StatList[achievement.StatID].Value > achievement.StatGoal)
                    CompleteAchievement(name);
            }
        }
    }

    public void CompleteAchievement(string id)
    {
        if (!AchievementList.ContainsKey(id))
        {
            Console.WriteLine($"Achievement {id} does not exist");
            return;    
        }
        
        Achievement achievement = AchievementList[id];
        if (achievement.Completed) return;
        achievement.Completed = true;
        AchievementNotificationLayer layer = new(achievement);
        Game.Instance.AttachLayer(layer, Game.Instance.GetLayerCount() - 1);
        
        if (AchievementList.All(pair => pair.Value.Completed)) 
            CompletionTime = Echoes.EchoesInstance.World.Time;
    }
}
