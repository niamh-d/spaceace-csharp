using Godot;
public partial class EnemyBase : PathFollow2D
{
	[Export] private Timer _laserTimer;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private AnimatedSprite2D _animatedSprite2D;

	[Export] private bool _shoots { get; set; } = false;
	[Export] private bool _aimsAtPlayer { get; set; } = false;
	[Export] private Defs.BulletType _bulletType { get; set; } = Defs.BulletType.Enemy;
	[Export] private float _bulletSpeed { get; set; } = 120.0f;
	[Export] private Vector2 _bulletDirection { get; set; } = Vector2.Down;
	[Export] private float _bulletWaitTime { get; set; } = 2.0f;
	[Export] private float _bulletWaitTimeVar { get; set; } = 0.05f;
	[Export] private float _speed = 50.0f;

	private Player _playerRef;

	public override void _Ready()
	{
		_playerRef = GetTree().GetFirstNodeInGroup(Defs.GROUP_PLAYER) as Player;
		if (_playerRef == null)
		{
			QueueFree();
			return;
		}
		_laserTimer.Timeout += LaserTimerTimeout;
		SpaceUtils.PlayRandomAnimation(_animatedSprite2D);
		StartShootTimer();
	}

	public override void _Process(double delta)
	{
		Progress += _speed * (float)delta;
		if (ProgressRatio > 0.99f)
		{
			QueueFree();
		}
	}

	private void StartShootTimer()
	{
		SpaceUtils.SetAndStartTimer(_laserTimer, _bulletWaitTime, _bulletWaitTimeVar);
	}

	private void UpdateBulletDirection()
	{
		if (!_aimsAtPlayer || !IsInstanceValid(_playerRef))
			return;

		_bulletDirection = GlobalPosition.DirectionTo(_playerRef.GlobalPosition);
	}

	private void Shoot()
	{
		if (!_shoots)
			return;

		UpdateBulletDirection();
		SignalManager.EmitOnCreateBullet(GlobalPosition, _bulletDirection, _bulletSpeed, (int)_bulletType);
		SoundManager.PlayLaserRandom(_sound);
		StartShootTimer();
	}

	private void LaserTimerTimeout()
	{
		Shoot();
	}
}