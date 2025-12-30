using Akamah.Engine.Core.Engine;
using Akamah.Engine.Engine.Core;
using Akamah.Engine.Engine.Scenes;
using Akamah.Engine.Scenes;

Game.Register([
  new GameScene(),
  new InventoryScene()
]);
Game.Scenes.SwitchTo<GameScene>();

var loop = new Loop();
loop.Run();
