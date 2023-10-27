using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class GodotParadiseProjectileComponent : Node
{
	[Signal]
	public delegate void FollowStartedEventHandler(Node2D target);
	[Signal]
	public delegate void FollowStoppedEventHandler(Node2D target);
	[Signal]
	public delegate void TargetSwappedEventHandler(Node2D CurrentTarget, Node2D PreviousTarget);
	[Signal]
	public delegate void HomingDistanceReachedEventHandler();
	[Signal]
	public delegate void BouncedEventHandler(Vector2 position);
	[Signal]
	public delegate void PenetratedEventHandler(int ReaminingPenetrations);
	[Signal]
	public delegate void PenetrationCompleteEventHandler();

	[ExportGroup("Speed")]
	[Export]
	public float MaxSpeed
	{
		get { return Mathf.Max(0, _maxSpeed - (SpeedReductionOnPenetration * PenetrationCount)); }
		set { _maxSpeed = value; }
	}

	private float _maxSpeed = 10.0f;

	[Export]
	public float Acceleration = 0.0f;

	[ExportGroup("Homing")]
	[Export]
	public float HomingDistance = 500.0f;
	[Export]
	public float HomingStrength = 20.0f;

	[ExportGroup("Penetration")]
	[Export]
	public int MaxPenetrations = 1;
	[Export]
	public float SpeedReductionOnPenetration = 0.0f;

	[ExportGroup("Bounce")]
	[Export]
	public bool BounceEnabled = false;
	[Export]
	public int MaxBounces = 10;

	public int PenetrationCount = 0;

	public Node2D Projectile;

	public Vector2 Direction
	{
		get { return _direction; }
		set { _direction = value.IsNormalized() ? value : value.Normalized(); }
	}

	private Vector2 _direction = Vector2.Zero;

	public Node2D Target;
	public bool FollowTarget
	{
		get { return _followTarget; }
		set
		{
			bool previousValue = _followTarget;
			_followTarget = value;

			if (value != previousValue)
			{
				if (value)
				{
					EmitSignal(SignalName.FollowStarted, Target);
				}
				else
				{
					EmitSignal(SignalName.FollowStopped, Target);
				}
			}
		}
	}

	public bool _followTarget = false;

	public Array<Vector2> BouncedPositions = new();

	public override void _Ready()
	{
		Projectile = (Node2D)GetParent();

		FollowStarted += OnFollowStarted;
	}

	public void Move(double? delta = null)
	{
		delta ??= GetPhysicsProcessDeltaTime();

		if (Projectile is not null)
		{

			if (FollowTarget)
			{
				UpdateFollowDirection(Target);
			}

			if (Acceleration > 0)
			{
				Projectile.GlobalPosition = Projectile.GlobalPosition.Lerp(Projectile.GlobalPosition + (Direction * MaxSpeed), Acceleration * (float)delta);
			}
			else
			{
				Projectile.GlobalPosition += Direction * MaxSpeed;
			}

			Projectile.LookAt(Direction + Projectile.GlobalPosition);
		}
	}

	public Vector2 UpdateFollowDirection(Node2D OnTarget)
	{
		Direction = Direction.Lerp(TargetPosition(), (float)(HomingStrength * GetPhysicsProcessDeltaTime()));

		return Direction;
	}

	public Vector2 TargetPosition()
	{
		if (Target is not null)
		{
			return Projectile.GlobalPosition.DirectionTo(Target.GlobalPosition);
		}

		return Vector2.Zero;
	}

	public bool TargetCanBeFollow(Node2D Target)
	{
		return Target.GlobalPosition.DistanceTo(Projectile.GlobalPosition) < HomingDistance;
	}

	public void BeginFollowTarget(Node2D NewTarget)
	{
		Target = NewTarget;
		FollowTarget = false;
	}

	public void SwapTarget(Node2D NextTarget)
	{
		EmitSignal(SignalName.TargetSwapped, Target, NextTarget);
		Target = NextTarget;
	}

	public void StopFollowTarget()
	{
		Target = null;
		FollowTarget = false;
	}

	public Vector2 Bounce(Vector2 NewDirection)
	{
		if (BouncedPositions.Count < MaxBounces)
		{
			BouncedPositions.Add(Projectile.GlobalPosition);
			Direction = Direction.Bounce(NewDirection);
			EmitSignal(SignalName.Bounced, BouncedPositions.Last());
		}

		return Direction;
	}

	public void OnFollowStarted(Node2D Target)
	{
		if (TargetCanBeFollow(Target))
		{
			UpdateFollowDirection(Target);
		}
		else
		{
			StopFollowTarget();
		}
	}
}
