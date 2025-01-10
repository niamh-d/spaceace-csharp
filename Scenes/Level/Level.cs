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
                        SignalManager.EmitOnCreateExplosion(new Vector2(200, 200), (int)Defs.ExplosionType.Explosion);
                }
                if (Input.IsActionJustPressed("quit"))
                {
                        GameManager.LoadMainScene();
                }
        }
}
