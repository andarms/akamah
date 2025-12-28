using Akamah.Engine.Core.Engine;
using Akamah.Engine.Engine.Scene;
using Akamah.Engine.Scenes;


SceneController.AddScene(new GameScene());
SceneController.AddScene(new InventoryScene());
var game = new Game();

game.Run();
