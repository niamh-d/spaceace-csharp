using System;
using Godot;

public partial class GameOver : Control
{
	[Export] private Label _scoreLabel;
	[Export] private Timer _timer;

	public override void _Ready()
	{
		Hide();
		SetProcess(false);

		SignalManager.Instance.OnPlayerDied += OnPlayerDied;
		_timer.Timeout += OnTimerTimeout;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnPlayerDied -= OnPlayerDied;
	}

	private void OnPlayerDied()
	{
		GetTree().Paused = true;
		_scoreLabel.Text = $"Score: {ScoreManager.GetScore()} (Best: {ScoreManager.GetHighScore()})";
		Show();
		_timer.Start();
	}

	private void OnTimerTimeout()
	{
		SetProcess(true);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			GameManager.LoadMainScene();
		}
	}
}
