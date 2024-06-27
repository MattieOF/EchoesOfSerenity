using System.Diagnostics;
using EchoesOfSerenity.Core.Entity;
using EchoesOfSerenity.Core.Tilemap;

namespace EchoesOfSerenity.World.Item;

public class Inventory
{
    public int Size { get; private set; } = 18;
    public static int RowSize = 6;
    public readonly List<(Item?, int)> Contents = [];
    public static HashSet<Item> DiscoveredItems = [];
    
    public Inventory(int size = 18)
    {
        Size = size;
        Contents.Clear();
        for (int i = 0; i < Size; i++)
            Contents.Add((null, 0));
    }

    public bool CanPickUp(Item item)
    {
        for (int i = 0; i < Size; i++)
        {
            if (Contents[i].Item1 == null)
                return true;
            if (Contents[i].Item1 == item && Contents[i].Item2 < item.MaxStack)
                return true;
        }

        return false;
    }

    public int AddItem(Item item, int count)
    {
        DiscoveredItems.Add(item);
        
        // Stacking phase
        for (int i = 0; i < Size; i++)
        {
            if (Contents[i].Item1 == item && Contents[i].Item2 < item.MaxStack)
            {
                int space = item.MaxStack - Contents[i].Item2;
                if (space >= count)
                {
                    Contents[i] = (item, Contents[i].Item2 + count);
                    return 0;
                }
                else
                {
                    Contents[i] = (item, item.MaxStack);
                    count -= space;
                }
            }
        }
        
        // Filling phase
        for (int i = 0; i < Size; i++)
        {
            if (Contents[i].Item1 == null)
            {
                if (count > item.MaxStack)
                {
                    Contents[i] = (item, item.MaxStack);
                    count -= item.MaxStack;
                }
                else
                {
                    Contents[i] = (item, count);
                    return 0;
                }
            }
        }
        
        return count;
    }

    public int RemoveItem(Item item, int count)
    {
        for (int i = 0; i < Size; i++)
        {
            if (Contents[i].Item1 == item)
            {
                if (Contents[i].Item2 > count)
                {
                    Contents[i] = (item, Contents[i].Item2 - count);
                    return 0;
                }
                else
                {
                    count -= Contents[i].Item2;
                    Contents[i] = (null, 0);
                }
            }
        }

        return count;
    }
    
    public void Empty()
    {
        for (int i = 0; i < Size; i++)
            Contents[i] = (null, 0);
    }

    public void BuildRecipeLists(LivingEntity context, List<Recipe> craftable, List<Recipe> discovered)
    {
        Stopwatch sw = new();
        sw.Start();
        
        // Cache the items we have
        var items = new Dictionary<Item, int>();
        for (int i = 0; i < Size; i++)
        {
            if (Contents[i].Item1 is null)
                continue;
            
            if (items.ContainsKey(Contents[i].Item1!))
                items[Contents[i].Item1!] += Contents[i].Item2;
            else
                items.Add(Contents[i].Item1!, Contents[i].Item2);
        }
        
        // Loop through all tiles within 16x16 of the context entity
        var nearbyTiles = new HashSet<Tile>();
        (int tx, int ty) = context.World.TopLayer.WorldCoordToTileCoord(context.Center);
        for (int x = -8; x < 8; x++)
        {
            for (int y = -8; y < 8; y++)
            {
                var tile = context.World.TopLayer.TileAtTileCoord(tx + x, ty + y);
                if (tile != null)
                    nearbyTiles.Add(tile);
            }
        }
        
        foreach (var recipe in Recipes.RecipeList)
        {
            bool canCraft = true;
            bool hasDiscovered = false;
            
            foreach ((Item item, int count, bool canUnlock) in recipe.Requirements)
            {
                if (canUnlock && DiscoveredItems.Contains(item))
                    hasDiscovered = true;
                
                if (!items.ContainsKey(item) || items[item] < count)
                {
                    canCraft = false;
                }
            }
            
            if (recipe.RequiredTile is not null && !nearbyTiles.Contains(recipe.RequiredTile))
                canCraft = false;
            
            if (canCraft)
                craftable.Add(recipe);
            else if (hasDiscovered)
                discovered.Add(recipe);
        }
        
        sw.Stop();
        Console.WriteLine($"Built recipe lists in {sw.Elapsed.TotalMilliseconds:F}ms");
    }
}
