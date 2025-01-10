using Godot;

public partial class GameUi : Control
{
	[Export] private Label _scoreLabel;
	[Export] private HealthBar _healthBar;
	[Export] private AudioStreamPlayer2D _sound;

	private bool _dead = false;

	public override void _Ready()
	{
		SignalManager.Instance.OnPlayerHealthBonus += OnPlayerHealthBonus;
		SignalManager.Instance.OnPlayerHit += OnPlayerHit;
		_healthBar.OnDied += OnHealthBarOnDied;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnPlayerHealthBonus -= OnPlayerHealthBonus;
		SignalManager.Instance.OnPlayerHit -= OnPlayerHit;
	}

	private void OnHealthBarOnDied()
	{
		if (_dead)
			return;
		_dead = true;
		GD.Print("GameUi.OnHealthBarOnDied()");
		SignalManager.EmitOnPlayerDied();
	}

	private void OnPlayerHit(int v)
	{
		_healthBar.TakeDamage(v);
	}

	private void OnPlayerHealthBonus(int v)
	{
		_healthBar.ModifyHealth(v, false);
		SoundManager.PlayPowerUpSound(_sound, Defs.PowerUpType.Health);
	}
}
