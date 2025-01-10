using Godot;

public partial class GameUi : Control
{
	[Export] private Label _scoreLabel;
	[Export] private HealthBar _healthBar;
	[Export] private AudioStreamPlayer2D _sound;

	private bool _dead = false;

	public override void _Ready()
	{
		SignalManager.Instance.OnScoreUpdated += OnScoreUpdated;
		SignalManager.Instance.OnPlayerHealthBonus += OnPlayerHealthBonus;
		SignalManager.Instance.OnPlayerHit += OnPlayerHit;

		_healthBar.OnDied += OnHealthBarOnDied;
		ScoreManager.ResetScore();
	}

	private void OnScoreUpdated(int val)
	{
		_scoreLabel.Text = val.ToString("D6");
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnPlayerHealthBonus -= OnPlayerHealthBonus;
		SignalManager.Instance.OnPlayerHit -= OnPlayerHit;
		SignalManager.Instance.OnScoreUpdated -= OnScoreUpdated;
	}

	private void OnHealthBarOnDied()
	{
		if (_dead)
			return;
		_dead = true;
		SignalManager.EmitOnPlayerDied();
	}

	private void OnPlayerHit(int val)
	{
		_healthBar.TakeDamage(val);
	}

	private void OnPlayerHealthBonus(int val)
	{
		_healthBar.ModifyHealth(val, false);
		SoundManager.PlayPowerUpSound(_sound, Defs.PowerUpType.Health);
	}
}
