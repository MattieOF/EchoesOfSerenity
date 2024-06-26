using System.Numerics;
using EchoesOfSerenity.Core;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.Core.Tilemap;
using EchoesOfSerenity.World.Entity;
using EchoesOfSerenity.World.Gen;
using EchoesOfSerenity.World.Tiles;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace EchoesOfSerenity.Layers;

public class DebugLayer : ILayer
{
    private bool _fpsVisible;
    private const int FontSize = 18;
    private Font _font, _boldFont;
    private Vector2 _titleSize;
    private bool _debugMenuVisible;
    private int _tileX, _tileY;
    private Vector2 _tileLocation;
    private int _tilemapChunkPreviewIndex = 0;

    public void OnAttach()
    {
        _font = ContentManager.GetFont("Content/Fonts/OpenSans-Regular.ttf");
        _boldFont = ContentManager.GetFont("Content/Fonts/OpenSans-Bold.ttf");
        _titleSize = Raylib.MeasureTextEx(_boldFont, "EoS Stats", 18, 1);
    }

    public void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F1))
            _fpsVisible = !_fpsVisible;
        if (Raylib.IsKeyPressed(KeyboardKey.F2))
            _debugMenuVisible = !_debugMenuVisible;
    }

    public void RenderUI()
    {
        if (_fpsVisible)
        {
            // Proper UI system would make this way better, but this is a jam baybee
            string fpsString = $"FPS: {Raylib.GetFPS()}\nEntities: {Echoes.EchoesInstance.World.GetEntityCount()}";
            Vector2 size = Raylib.MeasureTextEx(_font, fpsString, FontSize, 1);
            Raylib.DrawRectangle(15, 15, (int)(MathF.Max(size.X, _titleSize.X) + 20), (int)(size.Y + _titleSize.Y + 15),
                new Color(0, 0, 0, 130));
            Raylib.DrawTextEx(_boldFont, "EoS Stats", new Vector2(25, 20), FontSize, 1, Color.White);
            Raylib.DrawTextEx(_font, fpsString, new Vector2(25, 25 + _titleSize.Y), FontSize, 1, Color.White);
        }

        if (_debugMenuVisible)
        {
            Tiles.TerrainTileset.RenderTile((int)_tileLocation.X, (int)_tileLocation.Y, _tileX, _tileY);
        }
    }

    public void RenderImGUI()
    {
        if (_debugMenuVisible)
        {
            ImGui.Begin("EoS Debug Menu", ref _debugMenuVisible);

            if (ImGui.Button("Close"))
                Game.Instance.CloseGame();

            if (ImGui.CollapsingHeader("Camera Tools"))
            {
                ImGui.Text($"Camera position: {Game.Instance.CameraTarget}");
                float camZoom = Game.Instance.CameraZoom;
                if (ImGui.DragFloat("Camera Zoom", ref camZoom, 0.1f, 4f))
                    Game.Instance.CameraZoom = camZoom;
                ImGui.SliderFloat("Camera Lerp Speed", ref Game.Instance.CameraLerpSpeed, 0, 1);
                ImGui.DragFloat("Target Zoom for Spawn Anim", ref PlayerEntity.IntroAnimTargetZoom, 0.1f, 0.1f, 5f);
                ImGui.DragFloat("Speed for Spawn Anim", ref PlayerEntity.IntroAnimZoomSpeed, 0.01f, 0, 1);
                ImGui.DragFloat("Delay for Spawn Anim", ref PlayerEntity.IntroAnimZoomDelay, 0.1f, 0, 3);
                
                if (ImGui.Button("Use Debugging Camera"))
                {
                    Game.Instance.CameraZoom = 0.1f;
                    Game.Instance.CameraLerpSpeed = 0;
                    PlayerEntity.IntroAnimTargetZoom = 0.1f;
                    PlayerEntity.IntroAnimZoomSpeed = 0;
                    PlayerEntity.IntroAnimZoomDelay = 0;
                }
            }

            if (ImGui.CollapsingHeader("Tileset Debugging"))
            {
                if (ImGui.CollapsingHeader("View Tileset"))
                {
                    rlImGui.Image(Tiles.TerrainTileset.TilesetTexture);
                }

                ImGui.SliderInt("Tile tile X", ref _tileX, 0, Tiles.TerrainTileset.TileColumns - 1);
                ImGui.SliderInt("Test tile Y", ref _tileY, 0, Tiles.TerrainTileset.TileRows - 1);

                ImGui.SliderFloat2("Tile Location", ref _tileLocation, 0, 1000);
            }

            if (ImGui.CollapsingHeader("Tilemap Debugging"))
            {
                void TilemapDebugger(string name, Tilemap tilemap)
                {
                    if (ImGui.CollapsingHeader(name))
                    {
                        ImGui.Text($"Rendered Chunks: {tilemap.RenderedChunks}");
                        if (ImGui.Button("Rerender"))
                            tilemap.RerenderAll();
                        if (ImGui.CollapsingHeader("Tilemap Chunk Preview"))
                        {
                            ImGui.SliderInt("Chunk Index", ref _tilemapChunkPreviewIndex, 0, tilemap.Chunks.Count - 1);
                            rlImGui.ImageSize(tilemap.Chunks[_tilemapChunkPreviewIndex].Texture, 256, 256);
                        }
                    }
                }
                
                TilemapDebugger("Base Layer", Echoes.EchoesInstance.World.BaseLayer);
                TilemapDebugger("Top Layer", Echoes.EchoesInstance.World.TopLayer);
                
                if (ImGui.Button("Rerender World"))
                    Echoes.EchoesInstance.World.RerenderAll();
#if DEBUG
                ImGui.Checkbox("Draw Chunk Outlines", ref Tilemap.DrawChunkOutlines);
                ImGui.Checkbox("Enable Random Rotation", ref Tilemap.EnableRandomRotation);
#endif
            }

            if (ImGui.CollapsingHeader("World Gen Debugging"))
            {
                if (ImGui.CollapsingHeader("Generation Settings"))
                {
                    ImGui.SliderFloat("Island Threshold", ref WorldGen.IslandThreshold, 0, 1);
                    ImGui.InputFloat("Island Noise Frequency", ref WorldGen.IslandNoiseFrequency, 0, 1);
                    ImGui.SliderFloat("Island Noise Mix", ref WorldGen.IslandNoiseMix, 0, 1);
                    ImGui.InputFloat("Beach Bias", ref WorldGen.BeachBias, 0.02f, 0.1f);
                    ImGui.InputFloat("Beach Size", ref WorldGen.BeachSize, 0.01f, 0.1f);
                    ImGui.InputFloat("Lake Threshold", ref WorldGen.LakeThreshold, 0.02f, 0.1f);
                    ImGui.InputFloat("Deep Lake Threshold", ref WorldGen.DeepLakeThreshold, 0.02f, 0.1f);
                    ImGui.InputFloat("Sand Threshold", ref WorldGen.SandThreshold, 0.02f, 0.1f);
                    ImGui.InputFloat("Main Noise 1 Frequency", ref WorldGen.MainNoiseFreq, 0.02f, 0.1f);
                    ImGui.InputFloat("Main Noise 2 Frequency", ref WorldGen.MainNoise2Freq, 0.02f, 0.1f);
                    ImGui.InputFloat("Main Noise 3 Frequency", ref WorldGen.MainNoise3Freq, 0.02f, 0.1f);
                    ImGui.InputFloat("Cave Noise Frequency", ref WorldGen.CaveNoiseFreq, 0.02f, 0.1f);
                    ImGui.InputFloat("Cave Noise Threshold", ref WorldGen.CaveNoiseThreshold, 0.02f, 0.1f);
                    ImGui.InputFloat("Cave Wall Thickness", ref WorldGen.CaveWallThickness, 0.02f, 0.1f);
                    ImGui.InputFloat("Flowery Grass Threshold", ref WorldGen.FloweryGrassWeight, 0.02f, 0.1f);
                    ImGui.InputInt("Flowery Grass Chance", ref WorldGen.FloweryGrassChance, 1);
                    ImGui.InputInt("Rock Chance", ref WorldGen.RockChance, 1);
                    ImGui.InputInt("Pebble Chance", ref WorldGen.PebbleChance, 1);
                }
                
                if (ImGui.Button("Regenerate Level"))
                {
                    WorldGen.RegenerateWorld(Echoes.EchoesInstance.World);
                }
                
                if (ImGui.Button("View Full Map"))
                {
                    Game.Instance.SetCameraTarget(Echoes.EchoesInstance.World.GetCenterPoint());
                    Game.Instance.CameraZoom = 0.1f;
                }
            }

            ImGui.End();
        }
    }
}
