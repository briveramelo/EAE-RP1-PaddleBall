using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {


    /// <summary>
    /// Animates the electricity when hit with a bug
    /// </summary>
    class BackgroundPulse : GameObject {

        Animation screenPulseAnimation;

        public BackgroundPulse() : base() { }

        public int myPulseIndex;
        static int totalPulseIndex;

        public override void LoadContent(ContentManager Content) {
            SpriteSheetSpecs spriteSheet = new SpriteSheetSpecs(1920, 1080, 24, 4, 3, 0, 0);
            screenPulseAnimation = new Animation(this, spriteSheet, "Images/Spritesheets/PowerSurge");
            screenPulseAnimation.LoadContent(Content);
            screenPulseAnimation.PostLoad();
        }

        public override void PostLoad() {
            //base.PostLoad();
        }

        public void Animate() {
            isPulseAnimating = true;
        }

        public void Reset() {
            screenPulseAnimation.Reset();
        }

        bool isPulseAnimating;
        public override void Draw(SpriteBatch spriteBatch) {            
            if (isPulseAnimating) {
                screenPulseAnimation.Draw(spriteBatch);
                if (screenPulseAnimation.loopNumber==1) {
                    isPulseAnimating = false;
                    Destroy();
                }
            }
        }

    }
}
