using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;

namespace EchoesOfSerenity.World.Achievement;

public class Achievements
{
    public Dictionary<string, Achievement> AchievementList = [];

    public Achievements()
    {
        AchievementList.Add("punch_tree", new Achievement()
        {
            Name = "Getting Wood",
            Description = "Punch a tree to get wood",
            Icon = ContentManager.GetTexture("Content/UI/Tree.png")
        });
        
        AchievementList.Add("blow_up_200_tiles", new Achievement()
        {
            Name = "Bulldozer",
            Description = "Blow up 200 tiles with bombs",
            StatID = "tiles_blown_up",
            StatGoal = 200,
            Icon = ContentManager.GetTexture("Content/Items/Bomb.png")
        });
        
        AchievementList.Add("gg", new Achievement()
        {
            Name = "Getting Wood",
            Description = "Punch a tree to get wood",
            Icon = ContentManager.GetTexture("Content/UI/Tree.png")
        });
        
        AchievementList.Add("ggg", new Achievement()
        {
            Name = "Bulldozer",
            Description = "Blow up 200 tiles with bombs",
            StatID = "tiles_blown_up",
            StatGoal = 200,
            Icon = ContentManager.GetTexture("Content/Items/Bomb.png")
        });
        AchievementList.Add("df", new Achievement()
        {
            Name = "Getting Wood",
            Description = "Punch a tree to get wood",
            Icon = ContentManager.GetTexture("Content/UI/Tree.png")
        });
        
        AchievementList.Add("ur", new Achievement()
        {
            Name = "Bulldozer",
            Description = "Blow up 200 tiles with bombs",
            StatID = "tiles_blown_up",
            StatGoal = 200,
            Icon = ContentManager.GetTexture("Content/Items/Bomb.png")
        });
        AchievementList.Add("qdf", new Achievement()
        {
            Name = "Getting Wood",
            Description = "Punch a tree to get wood",
            Icon = ContentManager.GetTexture("Content/UI/Tree.png")
        });
        
        AchievementList.Add("refad", new Achievement()
        {
            Name = "Bulldozer",
            Description = "Blow up 200 tiles with bombs",
            StatID = "tiles_blown_up",
            StatGoal = 200,
            Icon = ContentManager.GetTexture("Content/Items/Bomb.png")
        });
        AchievementList.Add("thr", new Achievement()
        {
            Name = "Getting Wood",
            Description = "Punch a tree to get wood",
            Icon = ContentManager.GetTexture("Content/UI/Tree.png")
        });
        
        AchievementList.Add("rthdf", new Achievement()
        {
            Name = "Bulldozer",
            Description = "Blow up 200 tiles with bombs",
            StatID = "tiles_blown_up",
            StatGoal = 200,
            Icon = ContentManager.GetTexture("Content/Items/Bomb.png")
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
        Achievement achievement = AchievementList[id];
        if (achievement.Completed) return;
        achievement.Completed = true;
        AchievementNotificationLayer layer = new(achievement);
        Game.Instance.AttachLayer(layer, Game.Instance.GetLayerCount() - 1);
    }
}
