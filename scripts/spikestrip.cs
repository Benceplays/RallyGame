using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using File = System.IO.File;

public partial class spikestrip : Node2D
{
    public AllVariable allVariable;
    public TextureProgressBar hpbar;
    public Camera2D camera;
    public Timer maintimer;
    public int shake = 1;
    public Random rnd;
    public int t;
    public int first;
    public int second;
    public float time;
    public int max_sec;
    public Node2D car;
    public CharacterBody2D car_body;

    public override void _Ready()
    {
        allVariable = new AllVariable();
        hpbar = GetNode("/root/Game/Car/HUD/HpBar") as TextureProgressBar;
        camera = GetNode("/root/Game/Car/CharacterBody2D/Camera2D") as Camera2D;
        maintimer = GetNode("/root/Game/Timer") as Timer;


    }
    public void _on_Area2D_body_entered(object body)
    {
        allVariable.hp -= 34;
        time = 0;
        time++;
        hpbar.Value = allVariable.hp;
    }

    public override void _Process(float delta)
    {
        if (time > 0 && time < 30)
        {
            time++;
            Vibration();
        }
        if (allVariable.hp <= 0)
        {
            string text = File.ReadAllText(@"save/times.json");
            var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
            allVariable = new AllVariable();
            t = (int)maintimer.WaitTime - (int)maintimer.TimeLeft;
            allVariable.time = t;
            if (get_options.max_sec > t) { max_sec = t; }else { max_sec = get_options.max_sec; }
            game_end();
        }
        
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

        GetTree().ChangeSceneToFile("res://scenes/Timeout.tscn");
    }

    public void Vibration()
    {
        rnd = new Random();
        first = rnd.Next(-10, 10);
        second = rnd.Next(-10, 10);
        camera.SetOffset(new Vector2(first * shake, second * shake));
    }
}
