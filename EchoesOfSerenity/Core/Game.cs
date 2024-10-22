using System.Numerics;
using System.Runtime.CompilerServices;
using EchoesOfSerenity.Core.Content;
using EchoesOfSerenity.World.Entity;
using Raylib_cs;
using rlImGui_cs;

namespace EchoesOfSerenity.Core;

public class Game
{
    public static Game Instance = null!;
    public static readonly Vector2 DefaultScreenSize = new(1280, 600);
    
    public bool IsRunning { get; private set; } = true;
    public bool IsPaused { get; set; } = false;
    public Camera2D Camera;
    public Vector2 CameraTarget = Vector2.Zero;
    public float CameraLerpSpeed = 0.05f;
    public Rectangle CameraBounds { get; private set; }

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
    private readonly List<(ILayer, int)> _layersToAttach = [];
    private readonly List<ILayer> _layersToDetach = [];
    private int _lastWindowWidth, _lastWindowHeight;
    
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
        Raylib.InitWindow((int)DefaultScreenSize.X, (int)DefaultScreenSize.Y, "Echoes of Serenity");
        Raylib.InitAudioDevice();
        Raylib.SetExitKey(0);
        
        rlImGui.Setup();
        
        // Initialize camera
        Camera.Zoom = 1f;
        Camera.Target = new Vector2(0, 0);
        Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);
        Camera.Rotation = 0f;
        
        ContentManager.LoadContent();
        Raylib.SetWindowIcon(ContentManager.GetImage("Content/UI/IMGAppIcon.png"));
        Spritesheets.Init();
        OnInit();
        
        while (!Raylib.WindowShouldClose() && IsRunning)
        {
            int windowWidth = Raylib.GetScreenWidth();
            int windowHeight = Raylib.GetScreenHeight();
            if (windowWidth != _lastWindowWidth
                || windowHeight != _lastWindowHeight)
            {
                Camera.Offset = new Vector2(windowWidth / 2f, windowHeight / 2f);
                _baseCameraZoom = windowWidth / DefaultScreenSize.X;
                Camera.Zoom = _baseCameraZoom * CameraZoom;
                _lastWindowWidth = windowWidth;
                _lastWindowHeight = windowHeight;
                
                foreach (var layer in _layers)
                    layer.OnWindowResized();
            }

            // CameraZoom = Math.Clamp(CameraZoom + Raylib.GetMouseWheelMoveV().Y * 0.2f, 0.1f, 5f);
            
            // Move camera towards target
            Camera.Target = (Camera.Target - CameraTarget) * float.Pow(CameraLerpSpeed, Raylib.GetFrameTime()) + CameraTarget;
            
            // Make camera rectangle
            var mat = Raylib.GetCameraMatrix2D(Game.Instance.Camera);
            mat = Raymath.MatrixInvert(mat);
            var topLeft = Raymath.Vector2Transform(new(0, 0), mat);
            var bottomRight = Raymath.Vector2Transform(new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()), mat);
            CameraBounds = new(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            
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
            foreach (var (layer, index) in _layersToAttach)
            {
                _layers.Insert(index, layer);
                layer.OnAttach();
            }
            _layersToAttach.Clear();
            
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
    
    public void SetCameraTarget(Vector2 target)
    {
        CameraTarget = target;
        Camera.Target = CameraTarget;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 ScreenPosToWorld(Vector2 screenPos)
    {
        return Raymath.Vector2Transform(screenPos, Raymath.MatrixInvert(Raylib.GetCameraMatrix2D(Camera)));
    }

    public int GetLayerCount() => _layers.Count;
    
    public void AttachLayer(ILayer layer, int index = 0)
    {
        _layersToAttach.Add((layer, index));
    }

    public T ConstructLayer<T>(int index = 0) where T : ILayer, new()
    {
        T layer = new();
        _layersToAttach.Add((layer, index));
        return layer;
    }

    public void DetachLayer(ILayer layer)
    {
        _layersToDetach.Add(layer);
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
