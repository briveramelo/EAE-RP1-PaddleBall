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
    public class Score {
        public int score;
        public int rank;
        public string name;
        public bool isNewScore;

        public Score(int rank, int score, string name, bool isNewScore) {
            this.rank = rank;
            this.score = score;
            this.name = name;
            this.isNewScore = isNewScore;
        }
    }


    public class ScoreDisplay : IComparable<ScoreDisplay> {

        public Score myScore;
        ContentManager content;
        public Vector2 position { get { return new Vector2(720, 200 + myScore.rank * 70); } }
        Vector2 scale;
        SpriteFont spriteFont;
        Color displayColor { get { return myScore.isNewScore ? Color.Red : Color.White; } }

        string displayText { get { return myScore.rank.ToString() + ". " + myScore.name + " " + myScore.score; } }

        public ScoreDisplay(Score score) {
            myScore = score;
        }
        public void MarkAsNewScore() {
            myScore.isNewScore = true;
        }

        public int CompareTo(ScoreDisplay other) {
            if (other == null) {
                return 1;
            }
            int scoreDif = (int)other.myScore.score - (int)myScore.score;

            if (scoreDif != 0) {
                return scoreDif;
            }
            return other.myScore.score - myScore.score;
        }

        public int CompareToSimple(int otherScore) {            
            int scoreDif = otherScore - myScore.score;

            if (scoreDif != 0) {
                return scoreDif;
            }
            return otherScore - myScore.score;
        }

        public void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("scoreboard");            
        }

        public void PostLoad() {
            scale = Vector2.One * (4f / 10f);
        }

        public void OnGameOpen() {
            MarkAsOldScore();
        }

        public void MarkAsOldScore() {
            myScore.isNewScore = false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, displayText, position, displayColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }

    public class HighScoreDisplay {

        int maxHighScores = 10;

        List<ScoreDisplay> highScores;
        List<Score> GetAsScores() {
            List<Score> scores = new List<Score>();
            highScores.ForEach(score => scores.Add(score.myScore));
            return scores;
        }

        private static HighScoreDisplay instance;
        public static HighScoreDisplay Instance {
            get {
                if (instance == null) {
                    instance = new HighScoreDisplay();
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
            spriteFont = content.Load<SpriteFont>("scoreboard");
            LoadSavedScores();
            highScores.ForEach(score => score.LoadContent(Content));
        }

        public void OnGameOpen() {
            highScores.ForEach(score => score.OnGameOpen());
        }

        public void PostLoad() {
            position = new Vector2(520, 100);
            scale = Vector2.One * (7f/10f);
            highScores.ForEach(score => score.PostLoad());
        }

        void LoadSavedScores() {
            highScores = new List<ScoreDisplay>();
            List<Score> scores = SaveDataManager.CopyCurrentDataSave().GetHighScores();
            scores.ForEach(score => highScores.Add(new ScoreDisplay(score)));
            SortScores();
        }

        public bool IsNewHighScore(int newScore) {
            return highScores.LastOrDefault().CompareToSimple(newScore) > 0;
        }

        public void AddHighScore(ScoreDisplay newScore) {
            SortScores();
            highScores.ForEach(score => score.MarkAsOldScore());
            newScore.MarkAsNewScore();
            highScores.RemoveAt(maxHighScores - 1);
            highScores.Add(newScore);
            SortScores();
            SaveDataManager.Save(new DataSave(GetAsScores()));
        }

        void SortScores() {
            highScores.Sort();
            int i = 0;
            highScores.ForEach(score => {score.myScore.rank = i + 1; i++;});
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, "HIGH SCORES", position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            for (int i = highScores.Count - 1; i >= 0; i--) {
                highScores[i].Draw(spriteBatch);
            }
        }

    }
}
