using Godot;

public partial class GameManager : Node
{
	[Export]
	public PackedScene BlockScene;

	[Export]
	private Crane _crane;

	[Export]
	private Node2D _world;

	private Block _currentBlock;
	private bool _waitingForLanding = false;

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

	public override void _Process(double delta)
	{
		if (_currentBlock != null && _currentBlock.Freeze)
		{
			_currentBlock.Follow(_crane.BlockSpawn.GlobalPosition);

			if (Input.IsActionJustPressed("drop_block"))
			{
				_currentBlock.Release();
				_waitingForLanding = true;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		
		if (!_waitingForLanding)
			return;
		GD.Print($"Velocity: {_currentBlock.LinearVelocity.Length()}  Contacts: {_currentBlock.GetContactCount()}");
		if (_currentBlock.LinearVelocity.Length() < 1f &&
			_currentBlock.GetContactCount() > 0)
		{
			_waitingForLanding = false;
			SpawnBlock();
		}
	}
}
