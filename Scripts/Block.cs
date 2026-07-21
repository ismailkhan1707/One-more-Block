using Godot;

public partial class Block : RigidBody2D
{
	public override void _Ready()
	{
		Freeze = true;
	}

	public void Follow(Vector2 position)
	{
		GlobalPosition = position;
	}

	public void Release()
	{
		Freeze = false;
	}
}
