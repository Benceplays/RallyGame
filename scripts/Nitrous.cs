using Godot;
using System;

public partial class Nitrous : Node2D
{
	public AllVariable allVariable;
	public TextureProgressBar nitrousbar;
	public TextureProgressBar nitrousbar2;
	public Node2D car;
	public CharacterBody2D car_body;
	public Node2D car2;
	public CharacterBody2D car_body2;
	public override void _Ready()
	{
		allVariable = new AllVariable();
		nitrousbar = GetNode("/root/Game/Car/HUD/NitrousBar") as TextureProgressBar;
		car = GetNode("/root/Game/Car") as Node2D;
		car_body = car.GetNode("CharacterBody2D") as CharacterBody2D;
	}
	public void _on_Nitrous_body_entered(object body)
	{
		allVariable.nitrous = 100;
		nitrousbar.Value = allVariable.nitrous;
	}
	
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

