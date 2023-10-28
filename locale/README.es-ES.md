<p align="center">
	<img width="256px" src="https://github.com/GodotParadise/ProjectileComponentCSharp/blob/main/icon.jpg" alt="GodotParadiseProjectileComponentCsharp logo" />
	<h1 align="center">Godot Paradise ProjectileComponentCSharp</h1>
	
[![LastCommit](https://img.shields.io/github/last-commit/GodotParadise/ProjectileComponentCSharp?cacheSeconds=600)](https://github.com/GodotParadise/ProjectileComponentCSharp/commits)
[![Stars](https://img.shields.io/github/stars/godotparadise/ProjectileComponentCSharp)](https://github.com/GodotParadise/ProjectileComponentCSharp/stargazers)
[![Total downloads](https://img.shields.io/github/downloads/GodotParadise/ProjectileComponentCSharp/total.svg?label=Downloads&logo=github&cacheSeconds=600)](https://github.com/GodotParadise/ProjectileComponentCSharp/releases)
[![License](https://img.shields.io/github/license/GodotParadise/ProjectileComponentCSharp?cacheSeconds=2592000)](https://github.com/GodotParadise/ProjectileComponentCSharp/blob/main/LICENSE.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat&logo=github)](https://github.com/godotparadise/ProjectileComponentCSharp/pulls)
[![](https://img.shields.io/discord/1167079890391138406.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/XqS7C34x)
</p>

[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/GodotParadise/ProjectileComponentCSharp/blob/main/README.md)

- - -

Imagine que coloca este componente en cualquier escena que represente un proyectil en su videojuego, y estará listo para la acción. Proporciona funcionalidades comunes para proyectiles estándar.
- [Requerimientos](#requerimientos)
- [✨Instalacion](#instalacion)
	- [Automatica (Recomendada)](#automatica-recomendada)
	- [Manual](#manual)
	- [GDScript](#gdscript)
- [Como empezar](#como-empezar)
- [Parámetros exportados](#parámetros-exportados)
	- [Speed](#speed)
	- [Homing](#homing)
	- [Penetration](#penetration)
	- [Bounce](#bounce)
- [Variables normales accessibles](#variables-normales-accessibles)
- [Una vez entra en el scene tree](#una-vez-entra-en-el-scene-tree)
- [Funciones](#funciones)
	- [Move(double? delta = null)](#movedouble-delta--null)
	- [SwapTarget(Node2D NextTarget)](#swaptargetnode2d-nexttarget)
	- [StopFollowTarget()](#stopfollowtarget)
	- [BeginFollowTarget(Node2D NewTarget)](#beginfollowtargetnode2d-newtarget)
	- [Vector2 TargetPosition()](#vector2-targetposition)
	- [Vector2 UpdateFollowDirection(Node2D OnTarget)](#vector2-updatefollowdirectionnode2d-ontarget)
	- [Vector2 Bounce(Vector2 NewDirection)](#vector2-bouncevector2-newdirection)
- [Señales](#señales)
- [✌️Eres bienvenido a](#️eres-bienvenido-a)
- [🤝Normas de contribución](#normas-de-contribución)
- [📇Contáctanos](#contáctanos)

# Requerimientos
📢 No ofrecemos soporte para Godot 3+ ya que nos enfocamos en las versiones futuras estables a partir de la versión 4.
* Godot 4+

# ✨Instalacion
## Automatica (Recomendada)
Puedes descargar este plugin desde la [Godot asset library](https://godotengine.org/asset-library/asset/2039) oficial usando la pestaña AssetLib de tu editor Godot. Una vez instalado, estás listo para empezar
## Manual 
Para instalar manualmente el plugin, crea una carpeta **"addons"** en la raíz de tu proyecto Godot y luego descarga el contenido de la carpeta **"addons"** de este repositorio
## GDScript
Este plugin también ha sido escrito en GDScript y puedes encontrarlo en [ProjectileComponent](https://github.com/GodotParadise/ProjectileComponent)

# Como empezar
Este nodo funciona como los demás, sirviendo como hijo de otro nodo. En este caso, no está restringido sólo a `CharacterBody2D`, ya que las balas son típicamente nodos `Area2D`.

# Parámetros exportados
## Speed
- Max speed
- Acceleration
La **velocidad máxima**, como su nombre indica, define el límite máximo alcanzable de velocidad que se aplicará a la velocidad.
El getter de la velocidad modifica el valor basado en el parámetro `SpeedReductionOnPenetration`.

La **aceleración** hace más suave alcanzar la velocidad máxima si quieres aumentar la fluidez del movimiento. En caso de que no la quieras simplemente asígnale un valor cero y el nodo alcanzará la velocidad máxima inmediatamente.

## Homing
- Homing distance
- Homing strength
La **homing distance** define la distancia máxima a la que el proyectil perseguirá al objetivo si éste está definido. **Déjalo a cero** si no hay límite de distancia.
El propósito de `HomingStrength` es proporcionar un control preciso sobre la agresividad con la que el proyectil se acerca al objetivo.

## Penetration
- Max penetrations
- Speed reduction on penetration
El **max penetrations** es un valor entero que determina cuántas veces puede penetrar el proyectil antes de emitir la señal `PenetrationComplete`. No liberamos la cola del nodo ya que dejamos el comportamiento al usuario, permitiéndole manejarlo conectándose a la señal.
La **speed reduction on penetration** disminuye la velocidad en esta cantidad de vuelta a la velocidad máxima original cada vez que se produce una penetración.

## Bounce
- Bounce enabled
- Bounce times
El parámetro **bounce enabled** permite que el proyectil rebote en caso de colisión u otro tipo de evento. 
El parámetro **bounce times** determina el número de veces que este proyectil puede rebotar.

# Variables normales accessibles
- projectile: Node2D
- direction: Vector2
- target: Node2D
- follow_target: bool
- penetration_count
- bounced_positions: Array[Vector2]

# Una vez entra en el scene tree
- Se conecta a la señal `FollowStarted`.
Cuando este proyectil empieza a seguir un objetivo, el componente comprueba si el objetivo puede ser seguido. Esto significa
1. El objetivo no es nulo.
2. La distancia al objetivo es menor que el valor definido.
Si se cumplen estas condiciones, comienza el comportamiento de homing. Dependiendo de **`HomingStrength`**, el proyectil empieza a seguir al objetivo suavemente.

# Funciones
## Move(double? delta = null)
El proyectil comienza a moverse en la `Direction` y a la `MaxSpeed` definidas como parámetros. En este estado, el proyectil utiliza `LookAt` para orientarse hacia la dirección indicada.
## SwapTarget(Node2D NextTarget)
El objetivo se intercambia con el proporcionado como parámetro y emite la señal `TargetSwapped`.
## StopFollowTarget()
El proyectil deja de seguir al objetivo si éste está definido. Esta función establece el `Target` a null y `FollowTarget` a false
## BeginFollowTarget(Node2D NewTarget)
El proyectil comienza a seguir al objetivo y se activa este comportamiento. Esta función establece el objetivo a `NewTarget` y `FollowTarget` a true
## Vector2 TargetPosition()
Si se define un `Target`, esta función devuelve la dirección normalizada desde este proyectil hasta el objetivo. Devuelve `Vector2.ZERO` cuando el `Target` es nulo.
## Vector2 UpdateFollowDirection(Node2D OnTarget)
Actualiza la dirección para seguir al objetivo si el proyectil está en modo seguimiento.
## Vector2 Bounce(Vector2 NewDirection)
El proyectil rebota en la dirección definida por el parámetro `NewDirection` si el rebote está activado y quedan rebotes para este proyectil. Cada rebote exitoso añade la posición a la matriz `BouncedPositions`.
Normalmente, se pasa la `WallNormal` como parámetro para rebotar en la dirección opuesta, pero no queremos restringir las cosas; la `NewDirection` es flexible. Esta acción también emite la señal `Bounced`.

# Señales
- *FollowStarted(Node2D target)* 
- *FollowStopped(Node2D target)* 
- *TargetSwapped(Node2D CurrentTarget, Node2D PreviousTarget)* 
- *HomingDistanceReached()*
- *Bounced(Vector2 position)*
- *Penetrated(int RemainingPenetrations)* 
- *PenetrationComplete()*

# ✌️Eres bienvenido a
- [Give feedback](https://github.com/GodotParadise/ProjectileComponentCSharp/pulls)
- [Suggest improvements](https://github.com/GodotParadise/ProjectileComponentCSharp/issues/new?assignees=BananaHolograma&labels=enhancement&template=feature_request.md&title=)
- [Bug report](https://github.com/GodotParadise/ProjectileComponentCSharp/issues/new?assignees=BananaHolograma&labels=bug%2C+task&template=bug_report.md&title=)

GodotParadise esta disponible de forma gratuita.

Si estas agradecido por lo que hacemos, por favor, considera hacer una donación. Desarrollar los plugins y contenidos de GodotParadise requiere una gran cantidad de tiempo y conocimiento, especialmente cuando se trata de Godot. Incluso 1€ es muy apreciado y demuestra que te importa. ¡Muchas Gracias!

- - -
# 🤝Normas de contribución
**¡Gracias por tu interes en GodotParadise!**

Para garantizar un proceso de contribución fluido y colaborativo, revise nuestras [directrices de contribución](https://github.com/godotparadise/ProjectileComponentCSharp/blob/main/CONTRIBUTING.md) antes de empezar. Estas directrices describen las normas y expectativas que mantenemos en este proyecto.

**📓Código de conducta:** En este proyecto nos adherimos estrictamente al [Código de conducta de Godot](https://godotengine.org/code-of-conduct/). Como colaborador, es importante respetar y seguir este código para mantener una comunidad positiva e inclusiva.
- - -


# 📇Contáctanos
Si has construido un proyecto, demo, script o algun otro ejemplo usando nuestros plugins haznoslo saber y podemos publicarlo en este repositorio para ayudarnos a mejorar y saber que lo que hacemos es útil.
