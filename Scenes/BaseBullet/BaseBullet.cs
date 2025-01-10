using Godot;

public partial class BaseBullet : HitBox
{

	[Export] private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

	private Vector2 _dir = Vector2.Up;
	private float _speed = 50.0f;

	public override void _Ready()
	{
		base._Ready();
		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExited;
	}

	public override void _Process(double delta)
	{
		Position += _dir * _speed * (float)delta;
	}

	public void Setup(Vector2 direction, float speed)
	{
		_dir = direction;
		_speed = speed;
	}

	public void BlowUp()
	{
		SignalManager.EmitOnCreateExplosion(GlobalPosition, (int)Defs.ExplosionType.Explosion);
		SetProcess(false);
		QueueFree();
	}

	protected override void OnAreaEntered(Area2D area)
	{
		BlowUp();
	}

	private void OnScreenExited()
	{
		QueueFree();
	}
}
