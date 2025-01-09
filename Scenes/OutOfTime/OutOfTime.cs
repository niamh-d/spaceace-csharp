using Godot;
using System;

public partial class OutOfTime : Node
{
    [Export] private float _lifeTime = 30.0f;
    [Export] private Timer _timer;

    public override void _Ready()
    {
        _timer.WaitTime = _lifeTime;
        _timer.Start();
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        GetParent().QueueFree();
    }
}
