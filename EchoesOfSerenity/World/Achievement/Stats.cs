namespace EchoesOfSerenity.World.Achievement;

public class Stats
{
    public Dictionary<string, Stat> StatList = [];
    
    public Stats()
    {
        StatList.Add("tiles_blown_up", new Stat()
        {
            Name = "Tiles Blown Up",
            Unit = "tiles"
        });
        StatList.Add("units_moved", new Stat()
        {
            Name = "Distance Traveled",
            Unit = "blocks"
        });
    }

    public void AddStat(string stat, float value)
    {
        StatList[stat].Value += value;
    }
}
