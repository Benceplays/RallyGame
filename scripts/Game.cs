using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using File = System.IO.File;

public partial class Game : Node2D
{
	[Export] public int speed = 5;

	[Export] public int rotationspeed = 1;
	public Label Fps;
	public bool fps_is_on = false;
	public AudioStreamPlayer2D engine;
	public AudioStreamPlayer2D music;
	public Area2D busstop;
	public Node2D car;
	public Timer timer;
	public Label time;
	public TextureProgressBar nitrousbar;
	public TimeSpan t;
	public AllVariable allVariable;
	public Random rnd;
	public Camera2D camera;
	public Sprite2D carsprite;
	public int first;
	public int second;
	public int shake = 1;
	public Panel pausepanel;
	public bool pauseon = false;
	public string text = File.ReadAllText(@"save/options.json");

	public int gyorsasag = 5;

	public override void _Ready()
	{
		allVariable = new AllVariable();
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
		
		car = GetNode("Car") as Node2D;
		engine = GetNode("Car/HUD/Engine") as AudioStreamPlayer2D;
		music = GetNode("Car/HUD/Music") as AudioStreamPlayer2D;
		busstop = GetNode("/root/Game/Bus_stop/Area2D") as Area2D;
		timer = GetNode("Timer") as Timer;
		time = GetNode("Car/HUD/Time") as Label;
		nitrousbar = GetNode("Car/HUD/NitrousBar") as TextureProgressBar;
		camera = GetNode("/root/Game/Car/CharacterBody2D/Camera2D") as Camera2D;
		pausepanel = GetNode("Car/HUD/PausePanel") as Panel;
		carsprite = GetNode("/root/Game/Car/CharacterBody2D/Sprite2D") as Sprite2D;

		allVariable.hp = 100;
		allVariable.nitrous = 0;
		
		if(get_options.vsync_is_on == true)OS.VsyncEnabled = true; else OS.VsyncEnabled = false;

		if (get_options.fps_is_on)
		{
			fps_is_on = true;
			Fps = GetNode("Car/HUD/Fps") as Label;

			switch (get_options.fps_target)
			{
				case 0:
					Engine.TargetFps = 30;
					break;
					
				case 1:
					Engine.TargetFps = 60;
					break;
					
				case 2:
					Engine.TargetFps = 120;
					break;
					
				case 3:
					Engine.TargetFps = 240;
					break;
					
				case 4:
					Engine.TargetFps = 360;
					break;
					
				case 5:
					Engine.TargetFps = 10000;
					break;
					
			}
		}
		else
		{
			fps_is_on = false;
		}
		music.VolumeDb = get_options.MusicVolume;
		engine.VolumeDb = get_options.SoundEffectVolume;
	}
	public override void _Input(InputEvent esemeny)
	{
		if (Input.IsActionPressed("nitrous") && allVariable.nitrous >= 1)
		{
				carsprite.Texture2D = (Texture2D)ResourceLoader.Load("res://assets/Images/nitrouscar.png");
				allVariable.nitrous--;
				nitrousbar.Value = allVariable.nitrous;
				allVariable.speed = 1500;
				rnd = new Random();
				first = rnd.Next(-5, 5);
				second = rnd.Next(-5, 5);
				camera.SetOffset(new Vector2(first * shake, second * shake));
		}
		else
		{
			//allVariable.speed = 400;
			carsprite.Texture2D = (Texture2D)ResourceLoader.Load("res://assets/Images/car.png");
		}
		
	}
	public void _on_quit_pressed()
	{
        GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}

    public void _on_return_pressed()
    {
        pausepanel.Visible = false;
        allVariable.pauseon = false;
        GetTree().Paused = false;
    }
	public override void _Process(float delta)
	{
		if (fps_is_on == true)
		{
			Fps.Text = $"{Convert.ToInt32(1 / delta)} FPS";
		}
		t = TimeSpan.FromSeconds((int)timer.WaitTime - (int)timer.TimeLeft);
		time.Text = Convert.ToString(t);
	}
}
