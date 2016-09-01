using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {
    class ScoreBoard : GameObject {

        static int score=0;
        static int hitStreak;
        SpriteFont spriteFont;

        private static ScoreBoard instance;
        public static ScoreBoard Instance {
            get {
                if (instance == null) {
                    instance = new ScoreBoard();
                }
                return instance;
            }
        }

        public void AddPoints() {
            hitStreak += 1;
            score += 10 * hitStreak;
        }

        public void ReportMiss() {
            hitStreak = 0;
        }

        public override void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("scoreboard");
            position = new Vector2(100, 70);
            scale = Vector2.One * 6;
        }

        public override void PostLoad() {}
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, score.ToString(), position, Color.White,0,Vector2.Zero,scale, SpriteEffects.None, 0f);
        }


    }
}
