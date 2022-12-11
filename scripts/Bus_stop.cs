using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using File = System.IO.File;

public class Bus_stop : Node2D
{
	public Node2D car;
	public KinematicBody2D car_body;
	public Random rng;
	public Timer timer;
	public int t;
	public int max_sec;
	private AllVariable allVariable;
	public override void _Ready()
	{
		setPos();
	}

	public void setPos()
	{
		allVariable = new AllVariable();
		//GD.Print(allVariable.maplength);
		car = GetNode("/root/Game/Car") as Node2D;
		timer = GetNode("/root/Game/Timer") as Timer;
		car_body = car.GetNode("KinematicBody2D") as KinematicBody2D;
	}
	public void _on_Area2D_body_entered(object body)
	{	
			string text = File.ReadAllText(@"save/times.json");
			var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
			allVariable = new AllVariable();
			setPos();
			//GD.Print(timer.WaitTime - timer.TimeLeft);
			t = (int)timer.WaitTime - (int)timer.TimeLeft;
			GD.Print(t);
			allVariable.time = t;
			if (get_options.max_sec > t) { max_sec = t; }else { max_sec = get_options.max_sec; }
			game_end();
	}
	public void game_end()
	{
		string text = File.ReadAllText(@"save/times.json");
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
		JObject options = new JObject(
				new JProperty("max_sec", (int)max_sec),
				new JProperty("last_sec", (int)t));
			File.WriteAllText(@"save/times.json", options.ToString());

			using (StreamWriter file = File.CreateText(@"save/times.json"))
			using (JsonTextWriter writer = new JsonTextWriter(file))
			{
				options.WriteTo(writer);
			}

		GetTree().ChangeScene("res://scenes/Timeout.tscn");
	}

	public override void _Process(float delta)
	{
	}
}



