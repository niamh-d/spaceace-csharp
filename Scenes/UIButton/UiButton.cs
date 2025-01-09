using Godot;

public partial class UiButton : TextureButton
{
	[Export] private Label _label;
	[Export] private string _labelText;

	public override void _Ready()
	{
		_label.Text = _labelText;
	}

	public void SetText(string text)
	{
		_label.Text = text;
	}
}
