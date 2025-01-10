using Godot;

[GlobalClass]
public partial class EnemyWaves : Resource
{
    [Export] private Godot.Collections.Array<EnemyWave> _waves = new();
}
