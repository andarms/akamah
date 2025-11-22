using Akamah.Engine;
using Akamah.Engine.Scenes;

// Add scenes to the scene manager
SceneManager.AddScene(new MenuScene());
SceneManager.AddScene(new GameScene());

// Switch to the initial scene
SceneManager.SwitchTo<MenuScene>();

var game = new Game();
game.Run();
