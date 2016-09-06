using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    public class RoundDisplay : GameObject {

        SpriteFont spriteFont;
        public RoundDisplay() : base() { }

        string displayText { get { return "Round " + (EnemySpawner.Instance.currentRound).ToString(); } }

        public override void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("scoreboard");
        }

        public override void PostLoad() {
            scale = Vector2.One * (4f / 10f);
            position = new Vector2(1520, 100);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, displayText, position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
