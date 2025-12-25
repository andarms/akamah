using Akamah.Engine.Core.Engine;
using Akamah.Engine.Core.Scene;
using Akamah.Engine.Scenes;

SceneController.AddScene(new GameScene());
SceneController.SwitchTo<GameScene>();

var game = new Game();
game.Run();
