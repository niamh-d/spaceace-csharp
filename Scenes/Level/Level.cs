using Godot;
using System;

public partial class Level : Node2D
{
        public override void _Ready()
        {
                GetTree().Paused = false;
        }

        public override void _Process(double delta)
        {
                if (Input.IsActionJustPressed("test"))
                {
                }
                if (Input.IsActionJustPressed("quit"))
                {
                        GameManager.LoadMainScene();
                }
        }
}
