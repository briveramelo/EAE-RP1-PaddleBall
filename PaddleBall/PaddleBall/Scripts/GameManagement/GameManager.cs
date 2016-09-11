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
    public class GameManager : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Coroutiner myCoroutiner = new Coroutiner();

        private static GameManager instance;
        public static GameManager Instance {
            get {
                if (instance == null) {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public void Lose() {
            LoadNewScreen(Screen.Title);
        }

        public GameManager() {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
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
            BackGroundPulseManager.Instance.LoadContent(Content);
            LoadScores();
            HighScoreDisplay.Instance.OnGameOpen();
            LoadTitleScreen();
        }


        #region Load Screens
        public void LoadNewScreen(Screen newScreen) {
            switch (newScreen) {
                case Screen.Title:
                    if (ScreenManager.Instance.GetCurrentScreen() == Screen.Game) {
                        AudioManager.Instance.StopMusic();
                        myCoroutiner.StartCoroutine(TransitionFromGameToTitleScreen());
                    }
                    else {
                        LoadTitleScreen();
                    }
                    break;
                case Screen.Scores:
                    LoadScoreScreen();
                    break;
                case Screen.Game:
                    LoadGameScreen();
                    break;
                case Screen.Controls:
                    LoadControlsScreen();
                    break;
            }
        }
        IEnumerator TransitionFromGameToTitleScreen() {

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
            if (HighScoreDisplay.Instance.IsNewHighScore(ScoreBoard.Instance.GetScore())) {
                TextInputManager.Instance.LoadContent(Content);
                TextInputManager.Instance.PostLoad();

                while (TextInputManager.Instance.IsTyping()) {
                    TextInputManager.Instance.Update();
                    yield return null;
                }
            }
            stopwatch.Stop();
            LoadTitleScreen();
        }

        void ClearScreen() {
            GameObject.ClearGameObjects();
            CircleCollider.ClearColliders();
            Laser.numCannonBalls = 0;
            Debugger.Instance = null;
        }

        void LoadTitleScreen() {
            ClearScreen();
            lastKeyBoardState = new KeyboardState(Keys.Enter);
            Vector2 startButtonPosition = new Vector2(960f, 600f);
            Vector2 scoresButtonPosition = new Vector2(960f, 875f);
            Vector2 controlsButtonPosition = new Vector2(960f, 980f);

            new MouseCursor();
            new Button(Screen.Game, startButtonPosition, new string[] { "Images/Buttons/Start", "Images/Buttons/Start" });
            new Button(Screen.Scores, scoresButtonPosition, new string[] { "Images/Buttons/Scores", "Images/Buttons/Scores" });
            new Button(Screen.Controls, controlsButtonPosition, new string[] { "Images/Buttons/Controls", "Images/Buttons/Controls" });

            for (int i = 0; i < 4; i++) {
                BackgroundPulse bg = new BackgroundPulse();
                bg.myPulseIndex = i;
            }


            GameObject.allGameObjects.ForEach(gameobject => gameobject.LoadContent(Content));
            GameObject.allGameObjects.ForEach(gameobject => gameobject.PostLoad());
            if (!(ScreenManager.Instance.GetCurrentScreen() == Screen.Scores || ScreenManager.Instance.GetCurrentScreen() == Screen.Controls)) {
                AudioManager.Instance.PlayBackgroundMusic(Screen.Title);
            }
            ScreenManager.Instance.SetCurrentScreen(Screen.Title);
        }

        void LoadScoreScreen() {
            ClearScreen();
            new MouseCursor();
            Vector2 buttonPos = new Vector2(200, ScreenManager.Instance.Dimensions.Y - 100);
            new Button(Screen.Title, buttonPos, new string[] { "Images/Buttons/MainMenu_NotDepressed", "Images/Buttons/MainMenu_Depressed" });

            LoadScores();
            GameObject.allGameObjects.ForEach(gameobject => gameobject.LoadContent(Content));
            GameObject.allGameObjects.ForEach(gameobject => gameobject.PostLoad());
            ScreenManager.Instance.SetCurrentScreen(Screen.Scores);
        }

        void LoadScores() {
            HighScoreDisplay.Instance.LoadContent(Content);
            HighScoreDisplay.Instance.PostLoad();
        }

        void LoadGameScreen() {
            ClearScreen();
            BackGroundPulseManager.Instance.Reset();
            Ship.Instance = new Ship();
            Shield.Instance = new Shield();
            ScoreBoard.Instance = new ScoreBoard();
            new RoundDisplay();

            GameObject.allGameObjects.ForEach(gameobject => gameobject.LoadContent(Content));
            ScoreBoard.Instance.LoadContent(Content);
            EnemySpawner.Instance.LoadContent(Content);
            Debugger.Instance.LoadContent(Content);

            Ship.Instance.PostLoad();
            Shield.Instance.PostLoad();
            ScoreBoard.Instance.PostLoad();
            GameObject.allGameObjects.ForEach(gameobject => gameobject.PostLoad());

            AudioManager.Instance.PlayBackgroundMusic(Screen.Game);
            ScreenManager.Instance.SetCurrentScreen(Screen.Game);
        }

        void LoadControlsScreen() {
            ClearScreen();
            new MouseCursor();
            Vector2 buttonPos = new Vector2(200, ScreenManager.Instance.Dimensions.Y - 100);
            new Button(Screen.Title, buttonPos, new string[] { "Images/Buttons/MainMenu_NotDepressed", "Images/Buttons/MainMenu_Depressed" });

            GameObject.allGameObjects.ForEach(gameobject => gameobject.LoadContent(Content));
            GameObject.allGameObjects.ForEach(gameobject => gameobject.PostLoad());
            ScreenManager.Instance.SetCurrentScreen(Screen.Controls);
        }
        #endregion


        KeyboardState lastKeyBoardState;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            KeyboardState keyboardState = Keyboard.GetState();
            
            
            UpdateCurrentScreen(gameTime, keyboardState);
            myCoroutiner.Update();

            lastKeyBoardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        #region Update Screens
        void UpdateCurrentScreen(GameTime gameTime, KeyboardState keyboardState) {
            switch (ScreenManager.Instance.GetCurrentScreen()) {
                case Screen.Title:
                    UpdateTitleScreen(keyboardState);
                    break;
                case Screen.Scores:
                    UpdateScoreScreen(keyboardState);
                    break;
                case Screen.Game:
                    UpdateGameScreen(keyboardState);
                    break;
                case Screen.Controls:
                    UpdateControlsScreen(keyboardState);
                    break;
            }
            for (int i = GameObject.allGameObjects.Count - 1; i >= 0; i--) {
                if (i< GameObject.allGameObjects.Count) {
                    GameObject.allGameObjects[i].Update(gameTime);
                }
            }
        }

        void UpdateTitleScreen(KeyboardState keyboardState) {
            if (keyboardState.IsKeyDown(Keys.Enter) && lastKeyBoardState!=keyboardState) {
                LoadNewScreen(Screen.Game);
            }
            if (keyboardState.IsKeyDown(Keys.Escape) && lastKeyBoardState != keyboardState) {
                Exit();
            }
            if (keyboardState.IsKeyDown(Keys.P) && lastKeyBoardState != keyboardState) {
                LoadNewScreen(Screen.Controls);
            }
        }

        void UpdateScoreScreen(KeyboardState keyboardState) {
            if ((keyboardState.IsKeyDown(Keys.Back) || keyboardState.IsKeyDown(Keys.Escape)) && lastKeyBoardState != keyboardState) {
                LoadNewScreen(Screen.Title);
            }           
        }

        void UpdateGameScreen(KeyboardState keyboardState) {
            EnemySpawner.Instance.Update();
            Debugger.Instance.Update();
            if ((keyboardState.IsKeyDown(Keys.Back) || keyboardState.IsKeyDown(Keys.Escape)) && !TextInputManager.Instance.IsTyping()) {
                LoadTitleScreen();
            }
        }

        void UpdateControlsScreen(KeyboardState keyboardState) {
            if ((keyboardState.IsKeyDown(Keys.Back) || keyboardState.IsKeyDown(Keys.Escape)) && lastKeyBoardState != keyboardState) {
                LoadNewScreen(Screen.Title);
            }
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
                case Screen.Controls:
                    DrawControls();
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
            if (TextInputManager.Instance.IsTyping()) {
                TextInputManager.Instance.Draw(spriteBatch);
            }
            BackGroundPulseManager.Instance.Draw(spriteBatch);
            Debugger.Instance.Draw(spriteBatch);
        }

        void DrawControls() {

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
