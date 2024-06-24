using EchoesOfSerenity.Core.Content;
using Raylib_cs;
using rlImGui_cs;

namespace EchoesOfSerenity.Core;

public class Game
{
    public static Game Instance = null!;
    
    private readonly List<Layer> _layers = [];
    private readonly List<Layer> _layersToDetach = [];
    
    public Game()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Instance is not null)
            Utility.WriteLineColour(ConsoleColor.Red, "Instance of Game created when one already exists!");
        
        Instance = this;
    }

    public void Run()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(1280, 600, "Echoes of Serenity");
        Raylib.InitAudioDevice();
        Raylib.SetExitKey(0);
        
        rlImGui.Setup();

        ContentManager.LoadContent();
        OnInit();
        
        while (!Raylib.WindowShouldClose())
        {
            SoundManager.Update();
            foreach (var layer in _layers)
                layer.Update();
            OnUpdate();
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            
            foreach (var layer in _layers)
                layer.Render();
            OnRender();
            
            rlImGui.Begin();
            foreach (var layer in _layers)
                layer.RenderImGUI();
            OnRenderImGUI();
            rlImGui.End();
            
            Raylib.EndDrawing();
            
            // Post frame stuff
            // Perform queued destructions
            foreach (var layer in _layersToDetach)
            {
                layer.OnDetach();
                _layers.Remove(layer);
            }
            _layersToDetach.Clear();
        }
        
        OnShutdown();
        foreach (var layer in _layers)
            layer.OnDetach();
        ContentManager.UnloadAssets();
        rlImGui.Shutdown();
        Raylib.CloseWindow();
    }

    public int GetLayerCount() => _layers.Count;
    
    public void AttachLayer(Layer layer, int index = 0)
    {
        _layers.Insert(index, layer);
        layer.OnAttach();
    }

    public T ConstructLayer<T>(int index = 0) where T : Layer, new()
    {
        T layer = new();
        AttachLayer(layer, index);
        return layer;
    }
    
    public void QueueDetach(Layer layer)
    {
        if (!_layers.Contains(layer))
            Utility.WriteLineColour(ConsoleColor.Red, "Attempt to detach non-attached layer");
        else
            _layersToDetach.Add(layer);
    }

    protected virtual void OnInit() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnRender() { }
    protected virtual void OnRenderImGUI() { }
    protected virtual void OnShutdown() { }
}
