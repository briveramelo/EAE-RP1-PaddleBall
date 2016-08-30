using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {

    class SplashScreen : GameScreen {
        Texture2D image;

        public override void LoadContent() {
            base.LoadContent();
            string path = "Images/SplashScreen";
            image = content.Load<Texture2D>(path);
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }
    }
}
