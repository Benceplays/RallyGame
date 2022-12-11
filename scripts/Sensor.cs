using Godot;
using System;

public class Sensor : Node2D
{
    public string type;
    private AllVariable allVariable = new AllVariable();
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Area2D_body_entered(object body){
        if (body is KinematicBody2D){
            allVariable.speed = 800;
        }
    }

    public void _on_Area2D_body_exited(object body){
        if (body is KinematicBody2D){
            allVariable.speed = 100;
            GD.Print(allVariable.speed);
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
