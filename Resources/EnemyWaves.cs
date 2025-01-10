using Godot;

[GlobalClass]
public partial class EnemyWaves : Resource
{
    [Export] private Godot.Collections.Array<EnemyWave> _waves = new();

    public EnemyWave GetWaveForWaveCount(int wc)
    {
        return _waves[wc % _waves.Count];
    }

    public bool WaveIsStart(int wc)
    {
        return wc % _waves.Count == 0;
    }
}
