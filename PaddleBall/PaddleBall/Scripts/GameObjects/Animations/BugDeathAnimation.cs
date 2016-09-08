using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    class BugDeathAnimation : GameObject {

        Animation bugDeathAnimation;
        SpriteSheetSpecs spriteSheetSpecs;

        public BugDeathAnimation() : base() { }

        public override void LoadContent(ContentManager Content) {
            spriteSheetSpecs = new SpriteSheetSpecs(330, 330, 14, 6, 2, 0, 0);
            bugDeathAnimation = new Animation(this, spriteSheetSpecs, "Images/Spritesheets/Bugs/RedBug_Death");
            bugDeathAnimation.LoadContent(Content);
            bugDeathAnimation.PostLoad();
        }

        public override void PostLoad() {
            SetOriginInPixels(spriteSheetSpecs.width / 2, spriteSheetSpecs.height / 2);
            //base.PostLoad();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            bugDeathAnimation.Draw(spriteBatch);
            if (bugDeathAnimation.loopNumber == 1) {
                Destroy();
            }
        }
    }
}
