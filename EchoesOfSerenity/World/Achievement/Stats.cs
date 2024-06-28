namespace EchoesOfSerenity.World.Achievement;

public class Stats
{
    public Dictionary<string, Stat> StatList = [];
    
    public Stats()
    {
        StatList.Add("units_moved", new Stat()
        {
            Name = "Distance Traveled",
            Unit = "blocks"
        });
        StatList.Add("tiles_broken", new Stat()
        {
            Name = "Tiles Broken",
            Unit = "tiles"
        });
        StatList.Add("tiles_blown_up", new Stat()
        {
            Name = "Tiles Blown Up",
            Unit = "tiles"
        });
        StatList.Add("tiles_placed", new Stat()
        {
            Name = "Tiles Placed",
            Unit = "tiles"
        });
        StatList.Add("items_picked_up", new Stat()
        {
            Name = "Items Picked Up",
            Unit = "items"
        });
        StatList.Add("items_crafted", new Stat()
        {
            Name = "Items Crafted",
            Unit = "items"
        });
        StatList.Add("items_trashed", new Stat()
        {
            Name = "Items Trashed",
            Unit = "items"
        });
    }

    public void AddStat(string stat, float value)
    {
        StatList[stat].Value += value;
    }
}
