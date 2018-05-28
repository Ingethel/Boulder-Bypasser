# Boulder-Bypasser

Semi-3D Flight game.</p>

The player is given control of a spaceship inside a cave and he has to progress through it, avoid obstacles and pass through the way-points to increase his score and speed. 
A custom character controller gives the initial feel that was envisioned and then some minor options allow the player to adjust to his preferences. 
These include input (touch or accelerometer), adjusting the sensitivity, the character transparency or even changing the view point.

Objects are low polygon made in 3DS MAX, simple enough to serve their purpose. 
In-game they are being generated using a clustering algorithm to determine their initial position, size and speed. 
Unity's standard assets for cell shading fitted perfectly with the atmosphere and low polygon objects achieving simple cartoon graphics.	
Procedural mesh generation and editing gives the feeling of movement through the cave. 
Using a single chunk of terrain which is continuously altered using Perlin noise, the cave is continuously updated based on player's speed. 
The mesh points are being modified using Compute Shaders where supported, otherwise the terrain is split in four parts and use the standard .Net threads, achieving real-time update of both the mesh and its collider.
        
The game is for android and freely available on Google Play
