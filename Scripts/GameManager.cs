using Godot;
using System.Collections.Generic;

public partial class GameManager : Node
{
	[Export]
	public PackedScene BlockScene;

	[Export]
	private Crane _crane;
	
	[Export]
	private Node2D _world;

	[Export]
	private Camera2D _camera;

	private Block _currentBlock;
	private List<Block> _tower = new();
	private bool _waitingForLanding = false;

	private float _cameraTargetY;

	public override void _Ready()
	{
		_cameraTargetY = _camera.GlobalPosition.Y;

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

		Vector2 cameraPos = _camera.GlobalPosition;

	cameraPos.Y = Mathf.Lerp(
		cameraPos.Y,
		_cameraTargetY,
		3f * (float)delta
	);

	_camera.GlobalPosition = cameraPos;
	}

	public override void _PhysicsProcess(double delta)
	{
		
		if (!_waitingForLanding)
			return;
		
		//GD.Print($"Velocity: {_currentBlock.LinearVelocity.Length()}  Contacts: {_currentBlock.GetContactCount()}");
		
		if (_currentBlock.LinearVelocity.Length() < 1f &&
			_currentBlock.GetContactCount() > 0)
		{
			_waitingForLanding = false;

			_tower.Add(_currentBlock);
			//GD.Print($"Tower Height: {_tower.Count}");
			_cameraTargetY = Mathf.Min(_cameraTargetY, _currentBlock.GlobalPosition.Y);

			SpawnBlock();
		}
	}
}
