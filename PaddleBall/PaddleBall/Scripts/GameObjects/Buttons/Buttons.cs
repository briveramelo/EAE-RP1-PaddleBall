using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {
    public class Button : GameObject {

        public RectangleD myRec;
        public Screen screenToLoad;
        List<Texture2D> buttonTextures;
        string[] texturePaths;

        public Button(Screen screenToLoad, Vector2 position, string[] texturePaths) :base() {
            this.screenToLoad = screenToLoad;
            this.position = position;
            this.texturePaths = texturePaths;
        }

        protected MouseState lastMouseState;
        protected bool isPressed;

        public override void LoadContent(ContentManager Content) {
            content = Content;
            buttonTextures = new List<Texture2D>();
            foreach (string texturePath in texturePaths) {
                buttonTextures.Add(content.Load<Texture2D>(texturePath));
            }
        }

        public override void PostLoad() {
            Vector2 topLeft = position - new Vector2(buttonTextures[0].Width / 2, buttonTextures[0].Height / 2);
            Vector2 bottomRight = position + new Vector2(buttonTextures[0].Width / 2, buttonTextures[0].Height / 2);
            myRec = new RectangleD(topLeft, bottomRight);
            SetOriginInPixels(buttonTextures[0].Width / 2, buttonTextures[0].Height/ 2);
        }


        public override void Update(GameTime gameTime) {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = mouseState.Position.ToVector2();
                        
            if (mouseState.LeftButton == ButtonState.Pressed) {
                if (myRec.IsPointWithin(mousePosition)) {
                    OnPress();
                }
                else {
                    isPressed = false;
                }
            }
            if (mouseState.LeftButton == ButtonState.Released) {
                if (lastMouseState.LeftButton == ButtonState.Pressed) {
                    if (myRec.IsPointWithin(mousePosition)) {
                        OnSelect();                        
                    }
                }
                OnRelease();
            }

            lastMouseState = mouseState;
            base.Update(gameTime);
        }
        public virtual void OnSelect() {
            GameManager.Instance.LoadNewScreen(screenToLoad);
        }

        public void OnPress() {            
            isPressed = true;
        }

        public void OnRelease() {
            isPressed = false;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(buttonTextures[isPressed ? 0 : 1], position, null, color, rotation, originInPixels, scale, flip, layerDepth);
            //base.Draw(spriteBatch);
        }
    }
}
