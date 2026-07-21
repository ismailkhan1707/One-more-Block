using Godot;

public partial class GameManager : Node
{
	[Export]
	public PackedScene BlockScene;

	[Export]
	private Crane _crane;

	[Export]
	private Node2D _world;

	[Export]
	private Timer _spawnTimer;
	
	private Block _currentBlock;	

	public override void _Ready()
	{
		SpawnBlock();
	}

	private void SpawnBlock()
	{
		_currentBlock = BlockScene.Instantiate<Block>();

		_world.AddChild(_currentBlock);

		_currentBlock.GlobalPosition = _crane.BlockSpawn.GlobalPosition;
	}
	private void _on_spawn_timer_timeout()
	{
		SpawnBlock();
	}
	public override void _Process(double delta)
	{
		if (_currentBlock != null && _currentBlock.Freeze)
		{
			_currentBlock.Follow(_crane.BlockSpawn.GlobalPosition);

			if (Input.IsActionJustPressed("drop_block"))
			{
				_currentBlock.Release();
				_spawnTimer.Start();
			}
		}
	}
}
