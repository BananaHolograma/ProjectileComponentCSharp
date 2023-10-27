#if TOOLS
using Godot;
using System;

[Tool]
public partial class GodotParadiseProjectileComponentPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		AddCustomType("GodotParadiseProjectileComponent",
			"Node",
			GD.Load<Script>("res://addons/projectile_component/GodotParadiseProjectileComponent.cs"),
			GD.Load<Texture2D>("res://addons/projectile_component/bow.png")
		);
	}

	public override void _ExitTree() => RemoveCustomType("GodotParadiseProjectileComponent");
}
#endif