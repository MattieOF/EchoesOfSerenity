using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Item;
using Raylib_cs;

namespace EchoesOfSerenity.World.Tiles;

public static class Tiles
{
    public static Tileset TerrainTileset = null!;

    public static List<Sound> StoneHitSounds = [ ContentManager.GetSound("Content/Sounds/stone_hit1.wav"), ContentManager.GetSound("Content/Sounds/stone_hit2.wav"), ContentManager.GetSound("Content/Sounds/stone_hit3.wav") ];
    public static List<Sound> StoneBreakSounds = [ ContentManager.GetSound("Content/Sounds/stone_break1.wav"), ContentManager.GetSound("Content/Sounds/stone_break2.wav") ];
    public static List<Sound> PotBreakSounds = [ ContentManager.GetSound("Content/Sounds/pot_break1.wav") ];
    public static List<Sound> GrassFootstepSounds = [ ContentManager.GetSound("Content/Sounds/grass_step1.wav"), ContentManager.GetSound("Content/Sounds/grass_step2.wav"), ContentManager.GetSound("Content/Sounds/grass_step3.wav") ];
    public static List<Sound> SandFootstepSounds = [ ContentManager.GetSound("Content/Sounds/sand_step1.wav"), ContentManager.GetSound("Content/Sounds/sand_step2.wav"), ContentManager.GetSound("Content/Sounds/sand_step3.wav") ];
    public static List<Sound> StoneFootstepSounds = [ ContentManager.GetSound("Content/Sounds/stone_step1.wav"), ContentManager.GetSound("Content/Sounds/stone_step2.wav"), ContentManager.GetSound("Content/Sounds/stone_step3.wav") ];
    public static List<Sound> WaterFootstepSounds = [ ContentManager.GetSound("Content/Sounds/water_step1.wav"), ContentManager.GetSound("Content/Sounds/water_step2.wav"), ContentManager.GetSound("Content/Sounds/water_step3.wav") ];
    public static List<Sound> WoodHitSounds = [ ContentManager.GetSound("Content/Sounds/wood_hit1.wav"), ContentManager.GetSound("Content/Sounds/wood_hit2.wav"), ContentManager.GetSound("Content/Sounds/wood_hit3.wav") ];
    public static List<Sound> WoodBreakSounds = [ ContentManager.GetSound("Content/Sounds/wood_break1.wav"), ContentManager.GetSound("Content/Sounds/wood_break2.wav") ];

    public static Tile Grass = new Tile { IsSolid = false, TileSetIndex = 0, RandomRotation = true, FootstepSounds = GrassFootstepSounds};
    public static Tile FloweryGrass = new Tile { IsSolid = false, TileSetIndex = 10, RandomRotation = true, FootstepSounds = GrassFootstepSounds };
    public static Tile Sand = new Tile { IsSolid = false, TileSetIndex = 1, RandomRotation = true, FootstepSounds = SandFootstepSounds };
    public static Tile Water = new Tile { IsSolid = false, TileSetIndex = 2, Animated = true, Frames = 6, FPS = 3, FootstepSounds = WaterFootstepSounds };
    public static Tile DeepWater = new Tile { IsSolid = false, TileSetIndex = 18, Animated = true, Frames = 6, FPS = 3, FootstepSounds = WaterFootstepSounds };
    public static Tile StoneFloor = new Tile { IsSolid = false, TileSetIndex = 8, RandomRotation = true, FootstepSounds = StoneFootstepSounds };
    public static Tile StoneWall = new Tile { IsSolid = true, TileSetIndex = 9, RandomRotation = true, Strength = 5, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Stone, 1, 3)], HitSounds = StoneHitSounds, BreakSounds = StoneBreakSounds, FootstepSounds = StoneFootstepSounds  };
    public static Tile IronOre = new Tile { IsSolid = true, TileSetIndex = 24, RandomRotation = true, Strength = 10, RequiredTool = ToolType.Pickaxe, MinimumToolStrength = 3, Drops = [(Items.Stone, 1, 2), (Items.RawIron, 1, 2)], HitSounds = StoneHitSounds, BreakSounds = StoneBreakSounds, FootstepSounds = StoneFootstepSounds  };
    public static Tile CoalOre = new Tile { IsSolid = true, TileSetIndex = 25, RandomRotation = true, Strength = 6, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Stone, 1, 2), (Items.Coal, 1, 2)], HitSounds = StoneHitSounds, BreakSounds = StoneBreakSounds, FootstepSounds = StoneFootstepSounds  };
    public static Tile Pebbles = new Tile { IsSolid = false, TileSetIndex = 11, RandomRotation = false, Strength = 3, CanBePunched = true, RequiredTool = ToolType.Pickaxe, Drops = [(Items.Pebbles, 1, 1)], HitSounds = StoneHitSounds, BreakSounds = StoneBreakSounds, FootstepSounds = StoneFootstepSounds  };
    public static Tile Rock = new Tile { IsSolid = true, TileSetIndex = 12, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 4, Drops = [(Items.Stone, 0, 1)], HitSounds = StoneHitSounds, BreakSounds = StoneBreakSounds, FootstepSounds = StoneFootstepSounds  };
    public static Tile Tree = new TreeTile { IsSolid = true, TileSetIndex = 13, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.Axe, Strength = 7, Drops = [(Items.Wood, 3, 6)], HitSounds = WoodHitSounds, BreakSounds = WoodBreakSounds };
    public static Tile WoodPlank = new Tile { IsSolid = true, TileSetIndex = 14, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.Axe, Strength = 4, Drops = [(Items.WoodPlank, 1, 1)], HitSounds = WoodHitSounds, BreakSounds = WoodBreakSounds };
    public static Tile WorkBench = new Tile { IsSolid = true, TileSetIndex = 15, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.Axe, Strength = 7, Drops = [(Items.WorkBench, 1, 1)], Name = "Work Bench", HitSounds = WoodHitSounds, BreakSounds = WoodBreakSounds };
    public static Tile IronAnvil = new Tile { IsSolid = true, TileSetIndex = 16, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 7, Drops = [(Items.IronAnvil, 1, 1)], Name = "Iron Anvil" };
    public static Tile Furnace = new Tile { IsSolid = true, TileSetIndex = 17, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 7, Drops = [(Items.Furnace, 1, 1)], Name = "Furnace", HitSounds = StoneHitSounds, BreakSounds = StoneBreakSounds };
    public static Tile SugarCane = new Tile { IsSolid = false, TileSetIndex = 26, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.None, Strength = 2, Drops = [(Items.SugarCane, 1, 1)] };
    public static Tile SulfurRock = new Tile { IsSolid = true, TileSetIndex = 27, RandomRotation = false, CanBePunched = false, RequiredTool = ToolType.Pickaxe, Strength = 5, Drops = [(Items.Stone, 0, 1), (Items.SulfurDust, 1, 3)], HitSounds = StoneHitSounds, BreakSounds = StoneBreakSounds };
    public static Tile CavePot = new PotTile { IsSolid = true, TileSetIndex = 28, RandomRotation = false, CanBePunched = true, RequiredTool = ToolType.None, Strength = 2, BreakSounds = PotBreakSounds };
    
    public static void Init()
    {
        TerrainTileset = new Tileset("Content/Spritesheets/TerrainSpritesheet.png", 16, 16);
        
        foreach (var sound in WaterFootstepSounds)
            Raylib.SetSoundVolume(sound, 0.4f);
    }
}
