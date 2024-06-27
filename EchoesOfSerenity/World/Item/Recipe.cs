using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World.Item;

public class Recipe
{
    public Item Result = null!;
    public int ResultCount = 1;
    public List<(Item, int, bool)> Requirements = [];
    public Tile? RequiredTile = null;
}
