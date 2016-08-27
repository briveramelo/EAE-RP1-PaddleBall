using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Text;

namespace PaddleBall {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PaddleBall : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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

            GameObject.allGameObjects.Add(Cannon.Instance);
            GameObject.allGameObjects.ForEach(gameobject => gameobject.LoadContent(Content));
            ScreenManager.Instance.LoadContent(Content);


            //Post Load
            for (int i = GameObject.allGameObjects.Count - 1; i >= 0; i--) {
                GameObject.allGameObjects[i].PostLoad();
            }

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            ScreenManager.Instance.Update(gameTime);
            for (int i = GameObject.allGameObjects.Count - 1; i >= 0; i--) { 
                GameObject.allGameObjects[i].Update(gameTime);
            }

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            ScreenManager.Instance.Draw(spriteBatch);
            for (int i = GameObject.allGameObjects.Count - 1; i >= 0; i--) {
                GameObject.allGameObjects[i].Draw(spriteBatch);
            }            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            ScreenManager.Instance.UnloadContent();
        }
    }
}
