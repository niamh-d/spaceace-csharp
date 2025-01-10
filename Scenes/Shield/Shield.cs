using Godot;

public partial class Shield : Area2D
{

	[Export] private float _startHealth = 5;

	[Export] private AnimationPlayer _animationPlayer;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private CollisionShape2D _collisionShape2D;
	[Export] private Timer _timer;

	private float _health;

	public override void _Ready()
	{
		_timer.Timeout += OnTimerTimeout;
		AreaEntered += OnAreaEntered;

		DisableShield();
	}

	public void EnableShield()
	{
		_animationPlayer.Play("RESET");
		Show();
		_health = _startHealth;
		_collisionShape2D.CallDeferred(CollisionShape2D.MethodName.SetDisabled, false);
		_timer.Start();
		SoundManager.PlayPowerUpSound(_sound, Defs.PowerUpType.Shield);
	}

	private void DisableShield()
	{
		Hide();
		_collisionShape2D.CallDeferred(CollisionShape2D.MethodName.SetDisabled, true);
		_timer.Stop();
	}

	private void Hit()
	{
		_health--;
		_animationPlayer.Play("hit");
		if (_health <= 0)
		{
			DisableShield();
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		Hit();
	}

	private void OnTimerTimeout()
	{
		DisableShield();
	}
}


