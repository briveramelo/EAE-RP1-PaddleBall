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
        string displayText;
        SpriteFont spriteFont;
        Vector2 textPosition;
        Vector2 textScale;

        public Button(Screen screenToLoad, Vector2 position) :base() {
            this.screenToLoad = screenToLoad;
            this.position = position;
            
            displayText = screenToLoad.ToString();
        }

        MouseState lastMouseState;
        public override void Update(GameTime gameTime) {
            MouseState mouseState = Mouse.GetState();
            
            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != ButtonState.Pressed) {
                Vector2 mousePosition = mouseState.Position.ToVector2();
                if (myRec.IsPointWithin(mousePosition)) {
                    OnClick();
                }
            }

            lastMouseState = mouseState;
            base.Update(gameTime);
        }

        public override void LoadContent(ContentManager Content) {
            texturePath = "Images/Button";
            base.LoadContent(Content);
            spriteFont = content.Load<SpriteFont>("scoreboard");
        }

        public override void PostLoad() {
            base.PostLoad();
            Vector2 topLeft = position - new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 bottomRight = position + new Vector2(texture.Width / 2, texture.Height / 2);
            textScale = Vector2.One * (6f / 10f);
            myRec = new RectangleD(topLeft, bottomRight);
            textPosition = topLeft + new Vector2(0,25);
        }

        public void OnClick() {
            GameManager.Instance.LoadNewScreen(screenToLoad);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(spriteFont, displayText, textPosition, Color.White, 0, Vector2.Zero, textScale, SpriteEffects.None, 0f);
        }
    }
}
