using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    public class PulseManager {

        private static PulseManager instance;
        public static PulseManager Instance {
            get {
                if (instance == null) {
                    instance = new PulseManager();
                }
                return instance;
            }
        }
        List<BackgroundPulse> backgroundPulses;
        int currentPulseIndex;
        public void LoadContent(ContentManager Content) {
            
            backgroundPulses = new List<BackgroundPulse>();
            for (int i = 0; i < 4; i++) {
                backgroundPulses.Add(new BackgroundPulse());
                backgroundPulses[i].LoadContent(Content);
            }
        }

        public void Reset() {
            currentPulseIndex = 0;
            backgroundPulses.ForEach(pulse => pulse.Reset());
        }

        public void Animate() {
            backgroundPulses[currentPulseIndex].Animate();
            currentPulseIndex++;
        }

        public void Draw(SpriteBatch spriteBatch) {
            backgroundPulses.ForEach(pulse => pulse.Draw(spriteBatch));
        }

    }
}
