using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    public class TextInputManager {

        private static TextInputManager instance;
        public static TextInputManager Instance {
            get {
                if (instance == null) {
                    instance = new TextInputManager();
                }
                return instance;
            }
        }

        public void AcceptText() {
            HighScoreDisplay.Instance.AddHighScore(new ScoreDisplay(new Score(11, ScoreBoard.Instance.GetScore(), "NEW")));
        }
    }
}
