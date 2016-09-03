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
            ScoreBoardDisplay.Instance.AddHighScore(new Score(11, ScoreBoard.Instance.GetScore(), "NEW"));
        }
    }
}
