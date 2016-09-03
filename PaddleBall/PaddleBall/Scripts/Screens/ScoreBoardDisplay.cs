using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall{

    [Serializable]
    public class Score : IComparable<Score> {
        public int score;
        public int rank;
        public string name;

        ContentManager content;
        Vector2 position { get { return new Vector2(720, 200 + rank * 70); } }
        Vector2 scale;
        SpriteFont spriteFont;

        string displayText { get { return rank.ToString() + ". " + name + " " + score; } }

        public Score(int rank, int score, string name) {
            this.rank = rank;
            this.score = score;
            this.name = name;
        }

        public int CompareTo(Score other) {
            if (other == null) {
                return 1;
            }
            int scoreDif = (int)other.score - (int)score;

            if (scoreDif != 0) {
                return scoreDif;
            }
            else {
                return other.score - score;
            }
        }        

        public void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("scoreboard");

            scale = Vector2.One * (4f / 10f);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, displayText, position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }

    public class ScoreBoardDisplay {

        int maxHighScores = 10;
       
        List<Score> highScores = new List<Score>() {
            new Score(1, 1500, "CDE"),
            new Score(2, 900, "KNZ"),
            new Score(3, 800, "MNL"),
            new Score(4, 700, "HDK"),
            new Score(5, 600, "AKS"),
            new Score(6, 500, "BRM"),
            new Score(7, 400, "BOB"),
            new Score(8, 300, "ASH"),
            new Score(9, 200, "RYN"),
            new Score(10, 100, "JSE")
        };
        private static ScoreBoardDisplay instance;
        public static ScoreBoardDisplay Instance {
            get {
                if (instance == null) {
                    instance = new ScoreBoardDisplay();
                }
                return instance;
            }
        }

        ContentManager content;
        Vector2 position;
        Vector2 scale;
        SpriteFont spriteFont;
        public void LoadContent(ContentManager Content) {
            content = Content;
            position = new Vector2(520, 100);
            scale = Vector2.One * (7f/10f);
            spriteFont = content.Load<SpriteFont>("scoreboard");
            highScores.ForEach(score => score.LoadContent(Content));
        }

        public void AddHighScore(Score newScore) {
            highScores.Sort();
            bool isNewHighScore = highScores.LastOrDefault().CompareTo(newScore) > 0;
            if (isNewHighScore) {
                highScores.RemoveAt(maxHighScores - 1);
                highScores.Add(newScore);
                highScores.Sort();
            }
            for (int i = 0; i < maxHighScores; i++) {
                highScores[i].rank = i + 1;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, "HIGH SCORES", position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            for (int i = highScores.Count - 1; i >= 0; i--) {
                highScores[i].Draw(spriteBatch);
            }
        }

    }
}
