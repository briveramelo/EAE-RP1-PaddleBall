using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall{
    public class MegaLaserAnimation : Animation {


        public MegaLaserAnimation(GameObject parentGameObject, SpriteSheetSpecs spriteSheetSpecs, string spriteSheetPath) : base(parentGameObject, spriteSheetSpecs, spriteSheetPath) {
        }

        Vector2 position;
        Vector2 scale;
        Vector2 originInPixels;
        public void SetPosition(Vector2 position) {
            this.position = position;
        }
        public void SetScale(Vector2 scale) {
            this.scale = scale;
        }
        public void SetOrigin() {
            originInPixels = new Vector2((spriteSheetSpecs.width * scale.X/2f), 0f);
            //originInPixels = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (gameFramesOnCurrentAnimFrame > spriteSheetSpecs.numGameFramesPerAnimFrame) {
                currentSourceRectIndex++;

                if (currentSourceRectIndex >= spriteSheetSpecs.numFrames) {
                    currentSourceRectIndex = 0;
                }
                if (currentSourceRectIndex == spriteSheetSpecs.numFrames - 1) {
                    loopNumber++;
                }
                gameFramesOnCurrentAnimFrame = 0;
            }
            spriteBatch.Draw(spriteSheet, position, sourceRectangles[currentSourceRectIndex], Color.White, parentGameObject.rotation, originInPixels, scale, parentGameObject.flip, parentGameObject.layerDepth);
            gameFramesOnCurrentAnimFrame++;
        }

    }
}
