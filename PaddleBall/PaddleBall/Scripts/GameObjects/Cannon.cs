using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
namespace PaddleBall {

    public class Cannon : GameObject {

        bool isClockWise = true;
        float degPerSec = 1f;
        float radPerSec;
        ContentManager content;
        Vector2 screenCenter = new Vector2(ScreenManager.Instance.Dimensions.X, ScreenManager.Instance.Dimensions.Y) / 2f;

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
            texture = content.Load<Texture2D>(path);
            radPerSec = degPerSec * (float)Math.PI * 2f / 360f;
            SetLayerDepth(0f);
            SetOriginInPixels(texture.Width/ 2, texture.Height / 2);
            position = screenCenter;
        }

        void SwitchDirection() {
            isClockWise = !isClockWise;
            radPerSec = -radPerSec;
            Debug.WriteLine("SwitchedDirections!!!");
        }

        public override void Update(GameTime gameTime) {
            KeyboardState state = Keyboard.GetState();
            //rotate clockwise by default
            if ((state.IsKeyDown(Keys.Right) && isClockWise) || (state.IsKeyDown(Keys.Left) && !isClockWise)) {
                SwitchDirection();
            }
            rotation += radPerSec;
            Debug.WriteLine(rotation);
            SetRotation(rotation);
        }
    }
}
