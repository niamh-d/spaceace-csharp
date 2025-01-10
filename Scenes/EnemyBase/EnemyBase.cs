using Godot;

public partial class EnemyBase : PathFollow2D
{
	[Export] float _speed = 50.0f;

	public override void _Ready()
	{
	}


	public override void _Process(double delta)
	{
		Progress += _speed * (float)delta;
		if (ProgressRatio > 0.99f)
		{
			QueueFree();
		}

	}
}
