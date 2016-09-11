using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {

    /// <summary>
    /// Displays the current round for the current game session
    /// </summary>
    public class RoundDisplay : GameObject {

        SpriteFont spriteFont;
        public RoundDisplay() : base() { }

        string displayText { get { return "ROUND " + (EnemySpawner.Instance.currentRound).ToString(); } }

        public override void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("DS-DIGIT");
        }

        public override void PostLoad() {
            //scale = Vector2.One * (4f / 10f);
            scale = Vector2.One * 0.50f;
            position = new Vector2(1520, 148);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, displayText, position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
