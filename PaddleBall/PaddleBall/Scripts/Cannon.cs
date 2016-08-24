using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    public class Cannon {

        bool isClockWise = true;
        Texture2D image;
        ContentManager content;

        private static Cannon instance;
        public static Cannon Instance {
            get {
                if (instance == null) {
                    instance = new Cannon();
                }
                return instance;
            }
        }

        public void LoadContent(ContentManager Content) {
            content = Content;
            string path = "Images/Cannon";
            image = content.Load<Texture2D>(path);
        }

        void SwitchDirection() {
            isClockWise = !isClockWise;
            Debug.WriteLine("SwitchedDirections!!!");
        }

        public void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();
            //rotate clockwise by default
            if ((state.IsKeyDown(Keys.Right) && isClockWise) || (state.IsKeyDown(Keys.Left) && !isClockWise)) {
                SwitchDirection();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }
    }
}
