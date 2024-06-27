namespace EchoesOfSerenity.World.Item;

public static class Recipes
{
    public static List<Recipe> RecipeList = [];

    public static void Init()
    {
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.Wood, 1) ],
            Result = Items.WoodPlank,
            ResultCount = 2
        });
        
        RecipeList.Add(new Recipe()
        {
            Requirements = [ (Items.WoodPlank, 10) ],
            Result = Items.WorkBench,
            ResultCount = 1
        });
    }
}
