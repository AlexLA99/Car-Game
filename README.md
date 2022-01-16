# Crazy Cars

## Game Description

Race against your friends in this fast-paced multiplayer racing game!

## Members

- [Victor Nisa](https://github.com/VictorNisa)
- [Alex Lopez](https://github.com/AlexLA99)
- [Daniel Ruiz](https://github.com/xsiro)

## Controls

- W --> Accelerate
- A --> Turn left
- D --> Turn right
- S --> Break
- R --> Restart lap (only position not time)
- SPACEBAR --> Brakes

## How to play

If in unity: Open the scene "Assets\Networking\LobyScene" and press the play button
To download a build use this link: [Crazy Cars Build](https://github.com/AlexLA99/Car-Game/releases/tag/1.0)

First, you will need to enter your name. Then, you can either create a room, join a random room or search for a specific room. Once all your friends have joined the room and clicked ready, the game will start! The first to complete 2 laps wins!

## Net Work
- Daniel --> QA and bugfix
- Alex --> PUN gameplay implementation
- Victor --> PUN server setup / lobby creation 

## Features and possible improvements

### Gameplay

- Polished gameplay with checkpoint system
- Minimap
- Finish scene
- Audio

### Networking

- PUN server implementation
- Lag mitigation (shortened the amount of network calls from last delivery)

### What we could improve

- Lobby is too basic
- Lag mitigation could still be more optimized (path prediction)
- UI bugfixes
- More than 2 player gameplay

## Previous delivery work

- Daniel --> Helping to fix the problems between connections
- Alex --> Sending vehicle data to the server and back
- Victor --> UDP connection between client and server, tried to add join and leave events but they are still a work in progress

## Previous delivery features and possible improvements

- The velocimeter works right as the UI
	The car has some problems but works correctly
	Colliders work fine

- Features tried but incomplete
	- Accepting more than 2 players
		- Each new client connected will get an ID

	- Players join and leave events (incomplete)

	- GameEvents
		- There are no checkpoints or a lap system. He should pass the last checkpoint and then calculate the distance to the second one in order to detect the order of the cars

	- World state replication
		- Managed to send car speed to the server, but it was very buggy, we should send position and rotation sometimes in order to fix the online car.

	- Timer
		- As we don't have events for players joining, the game doesn't wait until ther are two players connected to start, so each player will have a different timer.

- Important: The game is not prepared for 3 people, so adding a third client could break the game as there isn't anything preventing that the third client could connect or not.


- Link: https://github.com/AlexLA99/Car-Game
