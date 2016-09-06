using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace PaddleBall {
    class ScoreBoard : GameObject {

        static int score;
        static int hitStreak;
        SpriteFont spriteFont;

        public ScoreBoard() : base() { }

        public int GetScore() {
            return score;
        }

        private static ScoreBoard instance;
        public static ScoreBoard Instance {
            get {
                if (instance == null) {
                    instance = new ScoreBoard();
                }
                return instance;
            }
            set { instance = value; }
        }

        public void AddPoints(float distanceAway) {
            hitStreak += 1;
            //float minDistanceForFullPoints = 440;
            //float distanceMultiplier = distanceAway / minDistanceForFullPoints;
            //if (distanceMultiplier > 1) {
            //    distanceMultiplier = 1;
            //}
            float distanceMultiplier = 1f;
            score += (int)((10 * hitStreak) * distanceMultiplier);
        }

        public void ReportMiss() {
            hitStreak = 0;
        }

        public override void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("scoreboard");
        }

        public override void PostLoad() {
            position = new Vector2(100, 70);
            scale = Vector2.One * (6f / 10f);
            score = 0;
            hitStreak = 0;
        }
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, score.ToString(), position, Color.White,0,Vector2.Zero,scale, SpriteEffects.None, 0f);
        }

        public override void Destroy() {
            Instance = null;
            base.Destroy();
        }


    }
}
