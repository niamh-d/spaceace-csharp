using Godot;
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
                        SignalManager.EmitOnCreatePowerUp(new Vector2(250, 250), (int)Defs.PowerUpType.Health);
                }
                if (Input.IsActionJustPressed("quit"))
                {
                        GameManager.LoadMainScene();
                }
        }
}
