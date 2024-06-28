using EchoesOfSerenity.Core.Content;
using Raylib_cs;

namespace EchoesOfSerenity.World.Item;

public static class Recipes
{
    public static List<Recipe> RecipeList = [];

    public static void Init()
    {
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Wood, 1, true) ],
            Result = Items.WoodPlank,
            ResultCount = 2,
            CraftingSound = ContentManager.GetSound("Content/Sounds/sawing2.wav")
        });
        Raylib.SetSoundVolume(ContentManager.GetSound("Content/Sounds/sawing2.wav"), 0.25f);
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.WoodPlank, 10, true), (Items.Wood, 2, true) ],
            Result = Items.WorkBench,
            ResultCount = 1,
            CraftingSound = ContentManager.GetSound("Content/Sounds/sawing2.wav")
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.WoodPlank, 2, true) ],
            Result = Items.Stick,
            ResultCount = 4,
            CraftingSound = ContentManager.GetSound("Content/Sounds/sawing2.wav")
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Stone, 20, true), (Items.Coal, 10, true) ],
            Result = Items.Furnace,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.WorkBench
        });

        RecipeList.Add(new Recipe()
        {
            Requirements = [(Items.RawIron, 1, true)],
            Result = Items.IronIngot,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.Furnace
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Stick, 3, true), (Items.Wood, 10, true) ],
            Result = Items.WoodenPickaxe,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.WorkBench
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Stick, 3, true), (Items.Wood, 10, true) ],
            Result = Items.WoodenAxe,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.WorkBench
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Stick, 3, false), (Items.Stone, 15, true) ],
            Result = Items.StonePickaxe,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.WorkBench
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Stick, 3, false), (Items.Stone, 15, true) ],
            Result = Items.StoneAxe,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.WorkBench
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.IronIngot, 6, false), (Items.Stone, 10, true) ],
            Result = Items.IronAnvil,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.WorkBench
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Stick, 3, false), (Items.IronIngot, 5, true) ],
            Result = Items.IronPickaxe,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.IronAnvil
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Stick, 3, false), (Items.IronIngot, 5, true) ],
            Result = Items.IronAxe,
            ResultCount = 1,
            RequiredTile = Tiles.Tiles.IronAnvil
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.SugarCane, 1, true) ],
            Result = Items.Sugar,
            ResultCount = 3,
            CraftingSound = ContentManager.GetSound("Content/Sounds/shake.wav")
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.SulfurDust, 4, true), (Items.IronIngot, 1, true) ],
            Result = Items.Bomb,
            ResultCount = 2,
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Sugar, 2, false), (Items.CaveBread, 2, false) ],
            Result = Items.JakeVoodooDoll,
            ResultCount = 1,
        });
    }
}
