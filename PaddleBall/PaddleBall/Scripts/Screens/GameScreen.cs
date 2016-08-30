using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaddleBall {
    public class GameScreen {

        Texture2D image;
        protected ContentManager content;

        public virtual void LoadContent(string imagePath) {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            image = content.Load<Texture2D>(imagePath);
        }

        public virtual void UnloadContent() {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime) {

        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }
    }
}
