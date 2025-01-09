using Godot;

public partial class Main : Control
{
	[Export] private UiButton _playBtn;
	[Export] private UiButton _quitBtn;

	public override void _Ready()
	{
		GetTree().Paused = false;
		_quitBtn.Pressed += () => { GetTree().Quit(); };
		_playBtn.Pressed += () => { GameManager.LoadGameScene(); };
	}
}
