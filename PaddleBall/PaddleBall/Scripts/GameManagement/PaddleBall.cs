using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Text;
using System;
using System.Collections.Generic;
using System.Collections;

namespace PaddleBall {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public class PaddleBall : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Coroutiner myCoroutiner = new Coroutiner();

        private static PaddleBall instance;
        public static PaddleBall Instance {
            get {
                if (instance == null) {
                    instance = new PaddleBall();
                }
                return instance;
            }
        }
        public void Lose() {            
            LoadNewScreen(Screen.Title);
        }

        //Vector2 testEnemyLoc = new Vector2(8, 8);
        public PaddleBall() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content);
            AudioManager.Instance.LoadContent(Content);
            SaveDataManager.LoadContent(Content);
            LoadScores();

            LoadGame();
        }


        #region Load Screens
        public void LoadNewScreen(Screen newScreen) {
            AudioManager.Instance.StopMusic();
            switch (newScreen) {
                case Screen.Title:
                    myCoroutiner.StartCoroutine(PauseAndSwitchToTitleScreen());
                    break;
                case Screen.Scores:
                    ClearScreen();
                    LoadScores();
                    break;
                case Screen.Game:
                    ClearScreen();
                    LoadGame();
                    break;
            }
        }
        IEnumerator PauseAndSwitchToTitleScreen() {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float timeToPause = 3.8f;
            while (stopwatch.ElapsedMilliseconds < 1.1f * 1000) {
                yield return null;
            }
            AudioManager.Instance.PlaySound(SoundFX.PlayerLoss);
            while (stopwatch.ElapsedMilliseconds < timeToPause * 1000) {
                yield return null;
            }
            TextInputManager.Instance.AcceptText();
            stopwatch.Stop();
            ClearScreen();
            LoadTitle();
        }

        void ClearScreen() {
            GameObject.ClearGameObjects();
            CircleCollider.ClearColliders();
        }

        void LoadTitle() {
            ScreenManager.Instance.SetCurrentScreen(Screen.Title);
            AudioManager.Instance.PlayBackgroundMusic(Screen.Title);
        }

        void LoadScores() {
            HighScoreDisplay.Instance.LoadContent(Content);
            HighScoreDisplay.Instance.PostLoad();
            ScreenManager.Instance.SetCurrentScreen(Screen.Scores);
            AudioManager.Instance.PlayBackgroundMusic(Screen.Scores);
        }

        void LoadGame() {
            Cannon.Instance = new Cannon();
            Shield.Instance = new Shield();
            ScoreBoard.Instance = new ScoreBoard();

            GameObject.allGameObjects.ForEach(gameobject => gameobject.LoadContent(Content));            
            ScoreBoard.Instance.LoadContent(Content);
            EnemySpawner.Instance.LoadContent(Content);

            Cannon.Instance.PostLoad();
            Shield.Instance.PostLoad();
            ScoreBoard.Instance.PostLoad();

            AudioManager.Instance.PlayBackgroundMusic(Screen.Game);
            ScreenManager.Instance.SetCurrentScreen(Screen.Game);
        }
        #endregion


        KeyboardState lastKeyState;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape)) {
                Exit();
            }

            ///////////////////////////////////////////////
            //placeholder logic for moving between screens
            if (keyboardState.IsKeyDown(Keys.R) && !lastKeyState.IsKeyDown(Keys.R)) {
                LoadNewScreen(Screen.Game);
            }
            if (keyboardState.IsKeyDown(Keys.T) && !lastKeyState.IsKeyDown(Keys.T)) {
                LoadNewScreen(Screen.Scores);
            }
            //////////////////////////////////////////////

            UpdateCurrentScreen(gameTime);
            myCoroutiner.Update();

            lastKeyState = Keyboard.GetState();
            base.Update(gameTime);
        }

        #region Update Screens
        void UpdateCurrentScreen(GameTime gameTime) {
            
            switch (ScreenManager.Instance.GetCurrentScreen()) {
                case Screen.Title:
                    RunTitle(gameTime);
                    break;
                case Screen.Scores:
                    RunScores(gameTime);
                    break;
                case Screen.Game:
                    RunGame(gameTime);
                    break;
            }
            for (int i = GameObject.allGameObjects.Count - 1; i >= 0; i--) {
                if (i< GameObject.allGameObjects.Count) {
                    GameObject.allGameObjects[i].Update(gameTime);
                }
            }
        }

        void RunTitle(GameTime gameTime) {

        }

        void RunScores(GameTime gameTime) {
            
        }

        void RunGame(GameTime gameTime) {
            EnemySpawner.Instance.Update();
            //AudioManager.Instance.Update();        
        }
        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            DrawCurrentScreen();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Draw Screens
        void DrawCurrentScreen() {
            ScreenManager.Instance.Draw(spriteBatch);

            switch (ScreenManager.Instance.GetCurrentScreen()) {
                case Screen.Title:
                    DrawTitle();
                    break;
                case Screen.Scores:
                    DrawScores();
                    break;
                case Screen.Game:
                    DrawGame();
                    break;
            }
            for (int i = GameObject.allGameObjects.Count - 1; i >= 0; i--) {
                GameObject.allGameObjects[i].Draw(spriteBatch);
            }
        }

        void DrawTitle() {
            
        }

        void DrawScores() {
            HighScoreDisplay.Instance.Draw(spriteBatch);
        }

        void DrawGame() {

        }
        #endregion


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            ScreenManager.Instance.UnloadContent();
        }
    }
}
