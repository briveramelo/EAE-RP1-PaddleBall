using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {

    public enum Screen {
        Title=0,
        Scores=1,
        Game =2,
        Controls=3
    }
    /// <summary>
    /// Manages which screen current is selected and which texture to load for each
    /// </summary>
    public class ScreenManager {

        public Vector2 Dimensions = new Vector2(1920, 1080);
        public ContentManager Content;

        Screen currentScreen = Screen.Scores;
        public void SetCurrentScreen(Screen newScreen) {
            currentScreen = newScreen;
        }
        public Screen GetCurrentScreen() {
            return currentScreen;
        }
        Dictionary<Screen, GameScreen> gameScreens;
        private static ScreenManager instance;
        public static ScreenManager Instance {
            get {
                if (instance == null) {
                    instance = new ScreenManager();
                }
                return instance;
            }
        }

        public void LoadContent(ContentManager Content) {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            gameScreens = new Dictionary<Screen, GameScreen>() {
                {  Screen.Title, new GameScreen("Images/Backgrounds/Title") },
                {  Screen.Scores, new GameScreen("Images/Backgrounds/HighScores") },
                {  Screen.Game, new GameScreen("Images/Backgrounds/Game") },
                {  Screen.Controls, new GameScreen("Images/Backgrounds/Controls") }
            };
            for (int i = 0; i < 4; i++){
                gameScreens[(Screen)i].LoadContent(Content);
            }            
        }

        public void UnloadContent() {
            gameScreens[currentScreen].UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch) {
            gameScreens[currentScreen].Draw(spriteBatch);            
        }
    }
}
