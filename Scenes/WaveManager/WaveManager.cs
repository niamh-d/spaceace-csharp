using Godot;

public partial class WaveManager : Node2D
{
	[Export] private EnemyWaves _enemyWavesResource;
	[Export] private Node2D _pathsContainer;
	[Export] private Timer _spawnTimer;

	private int _waveCount = 0;
	private Godot.Collections.Array<Path2D> _path2Ds = new();
	private float _waveGap = 4.0f;
	private float _speedFactor = 1.0f;

	public override void _Ready()
	{
		_spawnTimer.Timeout += OnSpawnTimerTimeout;
		SetupPaths();
		CallDeferred(MethodName.SpawnWave);
	}

	private void SetupPaths()
	{
		foreach (Node path in _pathsContainer.GetChildren())
		{
			if (path is Path2D)
			{
				_path2Ds.Add((Path2D)path);
			}
		}
	}

	private EnemyBase CreateEnemy(EnemyWave wave)
	{
		var newEnemy = (EnemyBase)wave.EnemyScene.Instantiate();
		newEnemy.Setup(wave.Speed * _speedFactor);
		return newEnemy;
	}

	private void UpdateSpeeds()
	{
		if (_enemyWavesResource.WaveIsStart(_waveCount))
		{
			_speedFactor *= 1.05f;
			_waveGap *= 0.95f;
			GD.Print($"update_speeds(): _wave_count: {_waveCount}, _speed_factor: {_speedFactor}, _wave_gap: {_waveGap}");
		}
	}

	private void StartSpawnTimer()
	{
		_spawnTimer.WaitTime = _waveGap;
		_spawnTimer.Start();
	}

	private async void SpawnWave()
	{
		GD.Print($"spawn_wave() _waveCount {_waveCount}");

		var path = _path2Ds.PickRandom();
		var wave = _enemyWavesResource.GetWaveForWaveCount(_waveCount);

		GD.Print($"wave() {_waveCount} spawning {wave.NumberOfEnemies} enemies on path {path.Name}");

		for (int i = 0; i < wave.NumberOfEnemies; i++)
		{
			path.AddChild(CreateEnemy(wave));
			await ToSignal(GetTree().CreateTimer(wave.Gap), "timeout");
		}

		GD.Print($"wave() {_waveCount} spawned, waiting {wave.Gap}");
		_waveCount++;
		UpdateSpeeds();
		StartSpawnTimer();
	}

	private void OnSpawnTimerTimeout()
	{
		SpawnWave();
	}
}