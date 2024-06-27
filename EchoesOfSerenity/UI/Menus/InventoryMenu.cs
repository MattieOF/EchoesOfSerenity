using System.Numerics;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;
using EchoesOfSerenity.World.Item;
using Raylib_cs;

namespace EchoesOfSerenity.UI.Menus;

public class InventoryMenu : Menu
{
    private PlayerEntity Player;
    private Font _font, _tooltipBoldFont, _tooltipFont, _headerFont;
    private Texture2D _frame;
    private (Item? item, int count) _pickedUpItem = (null, 0);

    private List<Recipe> _craftable = [], _visible = [];
    
    public InventoryMenu(PlayerEntity player)
    {
        Player = player;
        player.Inventory.BuildRecipeLists(player, _craftable, _visible);
        _font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);
        _tooltipBoldFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 18);
        _tooltipFont = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf", 18);
        _headerFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf", 40);
        _frame = ContentManager.GetTexture("Content/UI/Frame.png");
    }

    public override void Render()
    {
        base.Render();

        var mousePos = Raylib.GetMousePosition();
        int screenWidth = Raylib.GetScreenWidth();
        int screenHeight = Raylib.GetScreenHeight();

        int slotSize = (int)(screenHeight * 0.3f / ((float)Player.Inventory.Contents.Count / Inventory.RowSize));
        int startX = screenWidth / 2 - slotSize * Inventory.RowSize / 2;
        int x = startX - slotSize, y = 150;

        bool invEdited = false;

        (Item? item, int count) tooltipItem = (null, 0);
        for (int i = 0; i < Player.Inventory.Contents.Count; i++)
        {
            if (i != 0 && i % Inventory.RowSize == 0)
            {
                x = startX;
                y += slotSize;
            }
            else x += slotSize;

            var rect = new Rectangle(x, y, slotSize, slotSize);
            bool hovered = Raylib.CheckCollisionPointRec(mousePos, rect);
            Raylib.DrawRectanglePro(rect, Vector2.Zero, 0, hovered ? Color.DarkGray : new Color(20, 20, 20, 255));
            Raylib.DrawRectangleLinesEx(rect, 2, Color.Black);

            (Item? item, int count) = Player.Inventory.Contents[i];
            if (item != null)
            {
                Raylib.DrawTexturePro(item.Texture, new Rectangle(0, 0, item.Texture.Width, item.Texture.Height),
                    new Rectangle(x + 4, y + 4, slotSize - 8, slotSize - 8), Vector2.Zero, 0, Color.White);
                if (count > 1)
                    Raylib.DrawTextEx(_font, count.ToString(), new Vector2(x + 4, y + 4), 18, 0, Color.White);
            }

            if (hovered && _pickedUpItem.item is null && item is not null)
                tooltipItem = (item, count);

            if (hovered && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if (_pickedUpItem.item is null && item is not null)
                {
                    _pickedUpItem = Player.Inventory.Contents[i];
                    Player.Inventory.Contents[i] = (null, 0);
                    invEdited = true;
                }
                else if (_pickedUpItem.item is not null)
                {
                    if (Player.Inventory.Contents[i].Item1 is null)
                    {
                        Player.Inventory.Contents[i] = _pickedUpItem;
                        invEdited = true;
                        _pickedUpItem = (null, 0);
                    }
                    else if (Player.Inventory.Contents[i].Item1 == _pickedUpItem.item)
                    {
                        var space = _pickedUpItem.item.MaxStack - Player.Inventory.Contents[i].Item2;
                        if (space >= _pickedUpItem.count)
                        {
                            Player.Inventory.Contents[i] = (Player.Inventory.Contents[i].Item1,
                                Player.Inventory.Contents[i].Item2 + _pickedUpItem.count);
                            _pickedUpItem = (null, 0);
                        }
                        else
                        {
                            _pickedUpItem.count -= space;
                            Player.Inventory.Contents[i] = (Player.Inventory.Contents[i].Item1,
                                Player.Inventory.Contents[i].Item2 + space);
                        }
                        invEdited = true;
                    }
                    else
                    {
                        (Player.Inventory.Contents[i], _pickedUpItem) =
                            (_pickedUpItem, Player.Inventory.Contents[i]);
                        invEdited = true;
                    }
                }
            }
            else if (item is not null && hovered && Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                if (_pickedUpItem.item is null)
                {
                    int itemCount = Player.Inventory.Contents[i].Item2;
                    int a = (int)MathF.Floor(itemCount / 2.0f);
                    int b = itemCount - a;
                    Player.Inventory.Contents[i] = (Player.Inventory.Contents[i].Item1, a);
                    _pickedUpItem = (Player.Inventory.Contents[i].Item1, b);
                    if (Player.Inventory.Contents[i].Item2 == 0)
                        Player.Inventory.Contents[i] = (null, 0);
                    invEdited = true;
                }
                else
                {
                    if (Player.Inventory.Contents[i].Item1 == _pickedUpItem.item)
                    {
                        Player.Inventory.Contents[i] = (Player.Inventory.Contents[i].Item1,
                            Player.Inventory.Contents[i].Item2 + 1);
                        _pickedUpItem = (_pickedUpItem.item,
                            _pickedUpItem.count - 1);

                        if (_pickedUpItem.count == 0)
                            _pickedUpItem = (null, 0);
                        invEdited = true;
                    }
                }
            }
        }
        
        if (_pickedUpItem.item is not null)
        {
            var origin = mousePos - new Vector2(slotSize / 2.0f);
            Raylib.DrawTexturePro(_pickedUpItem.item.Texture,
                new Rectangle(0, 0, _pickedUpItem.item.Texture.Width, _pickedUpItem.item.Texture.Height),
                new Rectangle(origin, slotSize,
                    slotSize), Vector2.Zero, 0, Color.White);
            if (_pickedUpItem.count > 1)
                Raylib.DrawTextEx(_font, _pickedUpItem.count.ToString(), new Vector2(origin.X + 4, origin.Y + 4), 18, 0, Color.White);
        } else if (tooltipItem.item is not null)
        {
            var titleSize = Raylib.MeasureTextEx(_tooltipBoldFont, tooltipItem.item.Name, 18, 0);
            var descSize = string.IsNullOrWhiteSpace(tooltipItem.item.Description) ? Vector2.Zero : Raylib.MeasureTextEx(_tooltipFont, tooltipItem.item.Description, 18, 0);
            var tooltipRect = new Rectangle(mousePos.X + 20, mousePos.Y, titleSize.X + descSize.X + 40, titleSize.Y + descSize.Y + 20);
            Raylib.DrawTextureNPatch(_frame, FrameNPatch, tooltipRect, Vector2.Zero, 0, Color.White);
            Raylib.DrawTextEx(_tooltipBoldFont, tooltipItem.item.Name, new Vector2(mousePos.X + 40, mousePos.Y + 10), 18, 0, Color.White);
            Raylib.DrawTextEx(_tooltipFont, tooltipItem.item.Description, new Vector2(mousePos.X + 40, mousePos.Y + 10 + titleSize.Y), 18, 0, Color.Gray);
        }

        bool didCraft = false;
        Recipe? tooltipRecipe = null;
        
        x = 50;
        y += slotSize + 30;
        Raylib.DrawTextEx(_headerFont, "Crafting", new Vector2(x, y), 40, 0, Color.White);
        
        y += 50;
        foreach (var recipe in _craftable)
        {
            bool hovered = false;
            if (Raylib.CheckCollisionPointRec(mousePos, new Rectangle(x, y, 32, 32)))
            {
                tooltipRecipe = recipe;
                hovered = true;

                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    foreach (var requirement in recipe.Requirements)
                    {
                        Player.Inventory.RemoveItem(requirement.Item1, requirement.Item2);
                        int leftover = Player.Inventory.AddItem(recipe.Result, recipe.ResultCount);
                        if (leftover > 0)
                        {
                            ItemEntity itemEntity = new(recipe.Result, leftover)
                            {
                                Position = Player.Position
                            };
                            Player.World.Entities.Add(itemEntity);
                        }
                    }
                    didCraft = true;
                }
            }
            Raylib.DrawTexture(_frame, x, y, Color.White);
            Raylib.DrawTexturePro(recipe.Result.Texture, new Rectangle(0, 0, recipe.Result.Texture.Width, recipe.Result.Texture.Height), new Rectangle(x + (hovered ? 3 : 6), y + (hovered ? 3 : 6), 32 - (hovered ? 6 : 12), 32 - (hovered ? 6 : 12)), Vector2.Zero, 0, Color.White);
            x += 36;
        }

        x = 50;
        y += 50;
        Raylib.DrawTextEx(_headerFont, "Discovered", new Vector2(x, y), 40, 0, Color.White);

        y += 50;
        foreach (var recipe in _visible)
        {
            bool hovered = false;
            if (Raylib.CheckCollisionPointRec(mousePos, new Rectangle(x, y, 32, 32)))
            {
                tooltipRecipe = recipe;
                hovered = true;
            }
            Raylib.DrawTexture(_frame, x, y, Color.White);
            Raylib.DrawTexturePro(recipe.Result.Texture, new Rectangle(0, 0, recipe.Result.Texture.Width, recipe.Result.Texture.Height), new Rectangle(x + (hovered ? 3 : 6), y + (hovered ? 3 : 6), 32 - (hovered ? 6 : 12), 32 - (hovered ? 6 : 12)), Vector2.Zero, 0, Color.White);
            x += 36;
        }
        
        if (tooltipRecipe is not null)
        {
            Vector2 size = new(40, 20);
            List<(Font font, string text, Vector2 pos)> _queuedDraws = [];
            Vector2 pos = mousePos + new Vector2(40, 10);
            
            void DrawText(string text, Font font)
            {
                var sizeIncrease = string.IsNullOrWhiteSpace(text)
                    ? Vector2.Zero
                    : Raylib.MeasureTextEx(_tooltipFont, text, 18, 0);
                size.Y += sizeIncrease.Y;
                size.X = MathF.Max(size.X, sizeIncrease.X + 40);
                _queuedDraws.Add((font, text, pos));
                pos.Y += sizeIncrease.Y + 2;
            }
            
            DrawText($"{tooltipRecipe.ResultCount.ToString()} {tooltipRecipe.Result.Name}", _tooltipBoldFont);
            DrawText(tooltipRecipe.Result.Description, _tooltipFont);

            DrawText("Requires: ", _tooltipFont);
            foreach (var requirement in tooltipRecipe.Requirements)
                DrawText($"  {requirement.Item2} {requirement.Item1.Name}", _tooltipFont);
            if (tooltipRecipe.RequiredTile is not null)
                DrawText($"at a {tooltipRecipe.RequiredTile!.Name}", _tooltipFont);
            
            Raylib.DrawTextureNPatch(_frame, FrameNPatch, new Rectangle(mousePos + new Vector2(20, 0), size), Vector2.Zero, 0, Color.White);
            foreach (var (font, text, textPos) in _queuedDraws)
                Raylib.DrawTextEx(font, text, textPos, 18, 0, Color.White);
        }

        if (invEdited || didCraft)
        {
            _craftable.Clear();
            _visible.Clear();
            Player.Inventory.BuildRecipeLists(Player, _craftable, _visible);
        }
    }

    public override void OnClosed()
    {
        if (_pickedUpItem.item is not null)
        {
            if (Player.Inventory.CanPickUp(_pickedUpItem.item))
            {
                int remaining = Player.Inventory.AddItem(_pickedUpItem.item, _pickedUpItem.count);
                if (remaining > 0)
                {
                    _pickedUpItem = (_pickedUpItem.item, remaining);
                }
                else
                {
                    _pickedUpItem = (null, 0);
                }
            }
        }

        if (_pickedUpItem.count > 0)
        {
            ItemEntity itemEntity = new ItemEntity(_pickedUpItem.item!, _pickedUpItem.count);
            itemEntity.Position = Player.Position;
            Player.World.Entities.Add(itemEntity);
        }
    }
}
