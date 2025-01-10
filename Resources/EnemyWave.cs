using Godot;

[GlobalClass]
public partial class EnemyWave : Resource
{
    [Export] public PackedScene EnemyScene { get; private set; }
    [Export] public float Speed { get; private set; }
    [Export] public float Gap { get; private set; }
    [Export] public int NumberOfEnemies { get; private set; }
}
