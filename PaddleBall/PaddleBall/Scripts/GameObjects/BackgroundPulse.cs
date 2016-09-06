using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {
    class BackgroundPulse : GameObject {

        Animation screenPulseAnimation;

        public override void LoadContent(ContentManager Content) {
            SpriteSheetSpecs spriteSheet = new SpriteSheetSpecs(1920, 1080, 24, 4, 30, 0, 0);
            screenPulseAnimation = new Animation(this, spriteSheet, "Images/powersurgetest");
            screenPulseAnimation.LoadContent(Content);
            screenPulseAnimation.PostLoad();
        }

        public override void PostLoad() {
            //base.PostLoad();
        }


        public override void Draw(SpriteBatch spriteBatch) {
            screenPulseAnimation.Draw(spriteBatch);
        }
    }
}
