using Godot;

public partial class Explosion : AnimatedSprite2D
{
	const string ANIMATION_EXPLOSION = "explosion";
	const string ANIMATION_BOOM = "boom";

	[Export] private AudioStreamPlayer2D _sound;

	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished;
		SoundManager.PlayExplosionRandom(_sound);
		Play();
	}

	public void Setup(Defs.ExplosionType explosionType)
	{
		Animation = explosionType == Defs.ExplosionType.Boom ? ANIMATION_BOOM : ANIMATION_EXPLOSION;
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}
}
