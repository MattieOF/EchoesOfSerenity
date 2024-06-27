namespace EchoesOfSerenity.World.Achievement;

public class Stats
{
    public Dictionary<string, Stat> StatList = [];
    
    public Stats()
    {
        StatList.Add("tiles_blown_up", new Stat()
        {
            Name = "Tiles Blown Up"
        });
    }

    public void AddStat(string stat, float value)
    {
        StatList[stat].Value += value;
    }
}
