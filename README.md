Game Controls

- WASD or Arrow Keys to move the ship around the scene.
- Escape key or P to pause the game, you can exit from this menu.
- Use the left mouse click you fire a bullet. You can either click to fire a single shot, or hold for multiple shots fired per second.

The `Game.cs` file is the main point of contact in the game and it is in charge of the game state.

There are three main components to make the game run, Asteroids, Bullets and the Ship. 

Ship
- There is only a single instance of the ship in the game. It's meta data is stored in the Game.cs script since this references the player direction.
- Two scripts are attached to the view of the ship
- The ShipView handles collisions on the ship. In future it would also be used to single a hit to the players ship via some sort of visual effect.
  It does not handle what to do with the player health in anyway, this is handle by the Ship.cs script which is instanced in the Game.cs script.

Bullets
- Bullets are handled by the `BulletManager.cs` script.
- When fired, a bullet will be visualized infront of the ship, and it's movement direction determined by the ships direction.
- Their movement is updated in the BulletView.cs script. If they hit either an asteroid or exit the play radius they are destroyed.

Asteroid
- Asteroids are handled by the `AsteroidManager.cs` script.
- There is a minimum required number of asteroids at a single time, as well as a maximum number.
- Asteroids will be spawn every x number of seconds as determined by the `mSpawnTime` variable.
- When destroyed, larger asteroids will spawn two new smaller ones. The smallest asteroid will not spawn any new ones.


The Bullets and Asteroids objects are handled in an `ObjectContainer` instance. This class is just to simplify the creation of the objects, instead of destroying and instantiating 
new objects all the time, we simply deactivate objects and will recycle them when we can. Only if there are no deactive objects available will be instantiate a new one.
