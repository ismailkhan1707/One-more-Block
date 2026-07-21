using Godot;

public partial class Crane : Node2D
{
	public Marker2D BlockSpawn =>
	GetNode<Marker2D>("Hook/BlockSpawn");
	
	[Export]
	public float Speed = 250f;

	[Export]
	public float LeftLimit = -500f;

	[Export]
	public float RightLimit = +500f;

	private Node2D _hook;
	private int _direction = 1;

	public override void _Ready()
	{
		_hook = GetNode<Node2D>("Hook");
	}

	public override void _Process(double delta)
	{
		Vector2 position = _hook.Position;

		position.X += Speed * _direction * (float)delta;

		if (position.X >= RightLimit)
		{
			position.X = RightLimit;
			_direction = -1;
		}
		else if (position.X <= LeftLimit)
		{
			position.X = LeftLimit;
			_direction = 1;
		}

		_hook.Position = position;
	}
}
