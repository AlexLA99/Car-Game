- Team members: Victor Nisa, Alex Lopez, Daniel Ruiz

- Crazy cars: This is a racing game which 2 players will compete against them in a lake circuit. 

- Instructions: 
W --> Accelerate
A --> Turn left
D --> Turn right
S --> Break
SPACE --> in case of a crash, it fixes it.

In order to start, open the scene Client and the scene Networking/NetworkServerTest in additive. The second Client can be a Build (we have one already created in Build folder)

- Work: 	
	Daniel --> Helping to fix the problems between connections
	Alex --> Sending vehicle data to the server and back
	Victor --> UDP connection between client and server, tried to add join and leave events but they are still a work in progress
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
