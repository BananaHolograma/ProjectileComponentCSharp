<p align="center">
	<img width="256px" src="https://github.com/GodotParadise/ProjectileComponentCSharp/blob/main/icon.jpg" alt="GodotParadiseProjectileComponentCSharp logo" />
	<h1 align="center">Godot Paradise ProjectileComponentCSharp</h1>
	
[![LastCommit](https://img.shields.io/github/last-commit/GodotParadise/ProjectileComponentCSharp?cacheSeconds=600)](https://github.com/GodotParadise/ProjectileComponentCSharp/commits)
[![Stars](https://img.shields.io/github/stars/godotparadise/ProjectileComponentCSharp)](https://github.com/GodotParadise/ProjectileComponentCSharp/stargazers)
[![Total downloads](https://img.shields.io/github/downloads/GodotParadise/ProjectileComponentCSharp/total.svg?label=Downloads&logo=github&cacheSeconds=600)](https://github.com/GodotParadise/ProjectileComponentCSharp/releases)
[![License](https://img.shields.io/github/license/GodotParadise/ProjectileComponentCSharp?cacheSeconds=2592000)](https://github.com/GodotParadise/ProjectileComponentCSharp/blob/main/LICENSE.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat&logo=github)](https://github.com/godotparadise/ProjectileComponentCSharp/pulls)
[![](https://img.shields.io/discord/1167079890391138406.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/XqS7C34x)
[![Kofi](https://badgen.net/badge/icon/kofi?icon=kofi&label)](https://ko-fi.com/bananaholograma)
</p>

[![es](https://img.shields.io/badge/lang-es-yellow.svg)](https://github.com/GodotParadise/ProjectileComponentCSharp/blob/main/locale/README.es-ES.md)

- - -

Imagine placing this component in any scene representing a projectile in your video game, and it is ready for action. It provides common functionalities for standard projectiles.

- [Requirements](#requirements)
- [✨Installation](#installation)
	- [Automatic (Recommended)](#automatic-recommended)
	- [Manual](#manual)
- [Getting started](#getting-started)
- [Exported parameters](#exported-parameters)
	- [Speed](#speed)
	- [Homing](#homing)
	- [Penetration](#penetration)
	- [Bounce](#bounce)
- [Accessible normal parameters](#accessible-normal-parameters)
- [Once upon the scene tree](#once-upon-the-scene-tree)
- [Functions](#functions)
	- [Move(double? delta = null)](#movedouble-delta--null)
	- [SwapTarget(Node2D NextTarget)](#swaptargetnode2d-nexttarget)
	- [StopFollowTarget()](#stopfollowtarget)
	- [BeginFollowTarget(Node2D NewTarget)](#beginfollowtargetnode2d-newtarget)
	- [Vector2 TargetPosition()](#vector2-targetposition)
	- [Vector2 UpdateFollowDirection(Node2D OnTarget)](#vector2-updatefollowdirectionnode2d-ontarget)
	- [Vector2 Bounce(Vector2 NewDirection)](#vector2-bouncevector2-newdirection)
- [Signals](#signals)
- [✌️You are welcome to](#️you-are-welcome-to)
- [🤝Contribution guidelines](#contribution-guidelines)
- [📇Contact us](#contact-us)


# Requirements
📢 We don't currently give support to Godot 3+ as we focus on future stable versions from version 4 onwards
* Godot 4+

# ✨Installation
## Automatic (Recommended)
You can download this plugin from the official [Godot asset library](https://godotengine.org/asset-library/asset/2039) using the AssetLib tab in your godot editor. Once installed, you're ready to get started
##  Manual 
To manually install the plugin, create an **"addons"** folder at the root of your Godot project and then download the contents from the **"addons"** folder of this repository
## GDScript version
This plugin has also been written in GDScript and you can find it on [ProjectileComponent](https://github.com/GodotParadise/ProjectileComponent)

# Getting started
This node functions like the others, serving as a child of another node. In this case, it is not restricted to only `CharacterBody2D`, as bullets are typically `Area2D` objects.

# Exported parameters
## Speed
- Max speed
- Acceleration
The **max speed** as the name say, defines the maximum reachable limit of speed that will be apply to the velocity. 

The getter of the speed modify the value based on `SpeedReductionOnPenetration` parameter.

The **acceleration** makes smoother to reach the maximum speed if you want to increase the juiciness of the movement. In case you don't want it just assign a zero value to it and the node will reach the maximum speed immediately

## Homing
- Homing distance
- Homing strength
The **homing distance** defines the maximum distance the projectile will pursue the target if the target is defined. **Leave it at zero** if there is no distance limit.
The purpose of `HomingStrength` is to provide fine-grained control over how aggressively the projectile homes in on the target.

## Penetration
- Max penetrations
- Speed reduction on penetration
The **max penetrations** is an integer value that determines how many times the projectile can penetrate before emitting the `PenetrationComplete` signal. We do not queue-free the node as we leave the behavior to the user, allowing them to handle it by connecting to the signal.
The **speed reduction on penetration** decreases the speed by this amount back to the original maximum speed each time a penetration occurs.

## Bounce
- Bounce enabled
- Bounce times
The **bounce enabled** setting allows the projectile to bounce upon collision or another type of event. 
The **bounce times** parameter determines the number of times this projectile can bounce.

# Accessible normal parameters
- projectile: Node2D
- direction: Vector2
- target: Node2D
- follow_target: bool
- penetration_count
- bounced_positions: Array[Vector2]

# Once upon the scene tree
- Connects to the signal `FollowStarted`
When this projectile starts to follow a target, the component checks if the target can be followed. This means:
1. The target is not null.
2. The homing distance to the target is less than the defined value.
If these conditions are met, the homing behavior begins. Depending on the **`HomingStrength`**, the projectile starts to follow the target smoothly.

# Functions
## Move(double? delta = null)
The projectile starts moving in the `Direction` and at the `MaxSpeed` defined as parameters. In this state, the projectile uses `LookAt` to orient itself towards the provided direction.
## SwapTarget(Node2D NextTarget)
The target is swapped to the provided one as parameter and emit the signal `TargetSwapped`
## StopFollowTarget()
The projectile stop following the target if this is defined. This function set the `Target` to null and `FollowTarget` to false
## BeginFollowTarget(Node2D NewTarget)
The projectile begins to follow the target, and this behavior is activated. This function set the target to `NewTarget` and `FollowTarget` to true
## Vector2 TargetPosition()
If a `Target` is defined, this function returns the normalized direction from this projectile to the target. It returns `Vector2.ZERO` when the `Target` is null.
## Vector2 UpdateFollowDirection(Node2D OnTarget)
Update the direction to follow the target if the projectile is on following mode.
## Vector2 Bounce(Vector2 NewDirection)
The projectile bounces in the direction defined by the `NewDirection` parameter if bouncing is enabled and there are remaining bounces for this projectile. Every succesfull bounce append the position to the `BouncedPositions` array
Typically, you pass the `WallNormal` as a parameter to bounce in the opposite direction, but we don't want to restrict things; the `NewDirection` is flexible. This action also emits the `Bounced` signal.


# Signals
- *FollowStarted(Node2D target)* 
- *FollowStopped(Node2D target)* 
- *TargetSwapped(Node2D CurrentTarget, Node2D PreviousTarget)* 
- *HomingDistanceReached()*
- *Bounced(Vector2 position)*
- *Penetrated(int RemainingPenetrations)* 
- *PenetrationComplete()*

# ✌️You are welcome to
- [Give feedback](https://github.com/GodotParadise/ProjectileComponentCSharp/pulls)
- [Suggest improvements](https://github.com/GodotParadise/ProjectileComponentCSharp/issues/new?assignees=BananaHolograma&labels=enhancement&template=feature_request.md&title=)
- [Bug report](https://github.com/GodotParadise/ProjectileComponentCSharp/issues/new?assignees=BananaHolograma&labels=bug%2C+task&template=bug_report.md&title=)

GodotParadise is available for free.

If you're grateful for what we're doing, please consider a donation. Developing GodotParadise requires massive amount of time and knowledge, especially when it comes to Godot. Even $1 is highly appreciated and shows that you care. Thank you!

- - -
# 🤝Contribution guidelines
**Thank you for your interest in Godot Paradise!**

To ensure a smooth and collaborative contribution process, please review our [contribution guidelines](https://github.com/GodotParadise/ProjectileComponentCSharp/blob/main/CONTRIBUTING.md) before getting started. These guidelines outline the standards and expectations we uphold in this project.

**📓Code of Conduct:** We strictly adhere to the [Godot code of conduct](https://godotengine.org/code-of-conduct/) in this project. As a contributor, it is important to respect and follow this code to maintain a positive and inclusive community.

- - -

# 📇Contact us
If you have built a project, demo, script or example with this plugin let us know and we can publish it here in the repository to help us to improve and to know that what we do is useful.
