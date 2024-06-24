using System.Numerics;
using EchoesOfSerenity.Core.Content;
using Raylib_cs;
using rlImGui_cs;

namespace EchoesOfSerenity.Core;

public class Game
{
    public static Game Instance = null!;
    public bool IsRunning { get; private set; } = true;
    public Camera2D Camera;
    public static readonly Vector2 ScreenSize = new(1280, 600);

    public float CameraZoom
    {
        get => _cameraZoom;
        set
        {
            _cameraZoom = value;
            Camera.Zoom = _baseCameraZoom * value;
        }
    }

    private float _baseCameraZoom = 1, _cameraZoom = 1;
    private readonly List<ILayer> _layers = [];
    private readonly List<ILayer> _layersToDetach = [];
    
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
        Raylib.InitWindow((int)ScreenSize.X, (int)ScreenSize.Y, "Echoes of Serenity");
        Raylib.InitAudioDevice();
        Raylib.SetExitKey(0);
        
        rlImGui.Setup();
        
        // Initialize camera
        Camera.Zoom = 1f;
        Camera.Target = new Vector2(0, 0);
        Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);;
        Camera.Rotation = 0f;
        
        ContentManager.LoadContent();
        OnInit();
        
        while (!Raylib.WindowShouldClose() && IsRunning)
        {
            if (Raylib.IsWindowResized())
            {
                Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);
                _baseCameraZoom = Raylib.GetScreenWidth() / ScreenSize.X;
                Camera.Zoom = _baseCameraZoom * CameraZoom;
            }

            // Temp camera movement
            float moveSpeed = 100;
            if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
                moveSpeed *= 3;
            if (Raylib.IsKeyDown(KeyboardKey.A))
                Camera.Target -= new Vector2(moveSpeed * Raylib.GetFrameTime(), 0);
            if (Raylib.IsKeyDown(KeyboardKey.D))
                Camera.Target += new Vector2(moveSpeed * Raylib.GetFrameTime(), 0);
            if (Raylib.IsKeyDown(KeyboardKey.W))
                Camera.Target -= new Vector2(0, moveSpeed * Raylib.GetFrameTime());
            if (Raylib.IsKeyDown(KeyboardKey.S))
                Camera.Target += new Vector2(0, moveSpeed * Raylib.GetFrameTime());
            
            SoundManager.Update();
            foreach (var layer in _layers)
                layer.Update();
            OnUpdate();
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            
            foreach (var layer in _layers)
                layer.PreRender();
            OnPreRender();
            
            Raylib.BeginMode2D(Camera);
            foreach (var layer in _layers)
                layer.Render();
            OnRender();
            Raylib.EndMode2D();
            
            foreach (var layer in _layers)
                layer.RenderUI();
            OnRenderUI();
            
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
    
    public void AttachLayer(ILayer layer, int index = 0)
    {
        _layers.Insert(index, layer);
        layer.OnAttach();
    }

    public T ConstructLayer<T>(int index = 0) where T : ILayer, new()
    {
        T layer = new();
        AttachLayer(layer, index);
        return layer;
    }
    
    public void QueueDetach(ILayer layer)
    {
        if (!_layers.Contains(layer))
            Utility.WriteLineColour(ConsoleColor.Red, "Attempt to detach non-attached layer");
        else
            _layersToDetach.Add(layer);
    }
    
    public void CloseGame() => IsRunning = false;

    protected virtual void OnInit() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPreRender() { }
    protected virtual void OnRender() { }
    protected virtual void OnRenderUI() { }
    protected virtual void OnRenderImGUI() { }
    protected virtual void OnShutdown() { }
}
