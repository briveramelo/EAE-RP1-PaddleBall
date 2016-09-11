using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaddleBall {

    /// <summary>
    /// For drawing the background of a gamescreen
    /// </summary>
    public class GameScreen {

        Texture2D image;
        protected ContentManager content;
        string backgroundImagePath;

        public GameScreen(string backgroundImagePath) {
            this.backgroundImagePath = backgroundImagePath;
            if (true) { }
        }

        public void LoadContent(ContentManager Content) {
            content = Content;
            image = content.Load<Texture2D>(backgroundImagePath);
        }

        public virtual void UnloadContent() {
            content.Unload();
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }
    }
}
