using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PaddleBall {
    class ScoreBoard {

        static int score=0;
        static int hitStreak;
        SpriteFont spriteFont;
        Vector2 position = new Vector2(ScreenManager.Instance.Dimensions.X/2, 100);
        ContentManager content;
        Vector2 scale = Vector2.One * 10;

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

        public void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("scoreboard");
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, score.ToString(), position, Color.White,0,Vector2.Zero,scale, SpriteEffects.None, 0f);
        }


    }
}
