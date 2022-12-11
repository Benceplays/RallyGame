using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using File = System.IO.File;

public class Menu : Node2D
{
	public AudioStreamPlayer2D MainMusic;
	public AudioStreamPlayer2D click;
	public Label max_time;
	public AllVariable allVariable;

	public override void _Ready()
	{
		string text = File.ReadAllText(@"save/options.json");
		var options = JsonConvert.DeserializeObject<ConfigBody>(text);
		string text2 = File.ReadAllText(@"save/times.json");
		var options2 = JsonConvert.DeserializeObject<ConfigBody>(text2);

		click = GetNode("Click") as AudioStreamPlayer2D;
		MainMusic = GetNode("MainMusic") as AudioStreamPlayer2D;
		MainMusic.VolumeDb = options.MusicVolume;

		allVariable = new AllVariable();
		
		switch (options.display_index)
		{
			case 0:
				OS.WindowResizable = true;
				OS.WindowMaximized = false;
				OS.WindowFullscreen = false;
				break;
			case 1:
				OS.WindowFullscreen = true;
				OS.WindowMaximized = false;
				OS.WindowBorderless = false;
				break;
			case 2:
				OS.WindowMaximized = true;
				OS.WindowFullscreen = false;
				OS.WindowBorderless = false;
				break;

		}
	}

	public void _on_PlayButton_pressed()
	{
		click.Play();
		GetTree().ChangeScene("res://scenes/Game.tscn");
	}

	public void _on_PlayButton2_pressed()
	{
		click.Play();
		GetTree().ChangeScene("res://scenes/GameMultiplayer.tscn");
	}

	public void _on_OptionsButton_pressed()
	{
		click.Play();
		GetTree().ChangeScene("res://scenes/Options.tscn");
	}
	public void _on_ExitButton_pressed()
	{
		GetTree().Quit();
	}

	public override void _Process(float delta)
	{
	}
}
