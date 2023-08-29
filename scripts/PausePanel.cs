using Godot;
using System;

public partial class PausePanel : Panel
{
    
    public Panel pausepanel;
    public override void _Ready()
    {
        pausepanel = GetNode("../PausePanel") as Panel;
        
    }
    public override void _Input(InputEvent esemeny)
	{
		if (Input.IsActionJustPressed("back"))
		{
            AllVariable allVariable = new AllVariable();
			if (allVariable.pauseon == false) { pausepanel.Visible = true; allVariable.pauseon = true; GetTree().Paused = true; }
			else { pausepanel.Visible = false; allVariable.pauseon = false; GetTree().Paused = false; }
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
