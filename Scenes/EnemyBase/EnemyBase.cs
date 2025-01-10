using System;
using Godot;
public partial class EnemyBase : PathFollow2D
{

	[Export] private Timer _laserTimer;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private Area2D _hitBox;
	[Export] private HealthBar _healthBar;
	[Export] private Node2D _booms;

	[Export] private bool _shoots { get; set; } = false;
	[Export] private bool _aimsAtPlayer { get; set; } = false;
	[Export] private Defs.BulletType _bulletType { get; set; } = Defs.BulletType.Enemy;
	[Export] private float _bulletSpeed { get; set; } = 120.0f;
	[Export] private Vector2 _bulletDirection { get; set; } = Vector2.Down;
	[Export] private float _bulletWaitTime { get; set; } = 2.0f;
	[Export] private float _bulletWaitTimeVar { get; set; } = 0.05f;
	[Export] float _powerUpChance = 0.8f;
	[Export] public int _killMeScore { get; set; } = 10;

	private Player _playerRef;
	private bool _dead = false;
	private float _speed = 50.0f;

	public override void _Ready()
	{
		_playerRef = GetTree().GetFirstNodeInGroup(Defs.GROUP_PLAYER) as Player;
		if (_playerRef == null)
		{
			QueueFree();
			return;
		}
		_laserTimer.Timeout += LaserTimerTimeout;
		_healthBar.OnDied += HealthBarOnDied;
		_hitBox.AreaEntered += HitBoxOnAreaEntered;

		SpaceUtils.PlayRandomAnimation(_animatedSprite2D);
		StartShootTimer();
	}

	private void HitBoxOnAreaEntered(Area2D area)
	{
		if (area is BaseBullet)
		{
			_healthBar.TakeDamage((area as BaseBullet).GetDamage());
		}
	}

	private void CreatePowerUp()
	{
		if (GD.Randf() < _powerUpChance)
		{
			SignalManager.EmitOnCreateRandomPowerUp(GlobalPosition);
		}
	}

	private void MakeBooms()
	{
		foreach (Node2D b in _booms.GetChildren())
		{
			SignalManager.EmitOnCreateExplosion(b.GlobalPosition, (int)Defs.ExplosionType.Boom);
		}
	}

	private void DieCleanup()
	{
		_healthBar.OnDied -= HealthBarOnDied;
		_dead = true;
		SetProcess(false);
		MakeBooms();
		CreatePowerUp();
		ScoreManager.IncrementScore(_killMeScore);
		CallDeferred(MethodName.QueueFree);
	}

	private void HealthBarOnDied()
	{
		if (_dead)
			return;
		DieCleanup();
	}

	public override void _Process(double delta)
	{
		Progress += _speed * (float)delta;
		if (ProgressRatio > 0.99f)
		{
			QueueFree();
		}
	}

	public void Setup(float speed)
	{
		_speed = speed;
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