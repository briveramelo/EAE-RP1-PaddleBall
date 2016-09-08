using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    public class ShipExplosion : GameObject {

        Animation shipExplosionAnimation;
        SpriteSheetSpecs spriteSheetSpecs;

        public ShipExplosion() : base() { }

        public override void LoadContent(ContentManager Content) {
            spriteSheetSpecs = new SpriteSheetSpecs(330, 330, 14, 6, 2, 0, 0);
            shipExplosionAnimation = new Animation(this, spriteSheetSpecs, "Images/Spritesheets/Bugs/RedBug_Death");
            shipExplosionAnimation.LoadContent(Content);
            shipExplosionAnimation.PostLoad();
        }

        public override void PostLoad() {
            SetOriginInPixels(spriteSheetSpecs.width / 2, spriteSheetSpecs.height / 2);
            //base.PostLoad();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            shipExplosionAnimation.Draw(spriteBatch);
            if (shipExplosionAnimation.loopNumber == 1) {
                Destroy();
            }
        }
    }
}
