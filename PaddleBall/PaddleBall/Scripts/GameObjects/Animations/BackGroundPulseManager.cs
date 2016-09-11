using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {

    /// <summary>
    /// Preloads and pools several background pulse animations to prevent the hit in performance during a load for these massive file loads
    /// The files are about 1.5MB for the pulse animation, so these need to be memory managed for performance
    /// This class handles that
    /// </summary>
    public class BackGroundPulseManager {

        private static BackGroundPulseManager instance;
        public static BackGroundPulseManager Instance {
            get {
                if (instance == null) {
                    instance = new BackGroundPulseManager();
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
