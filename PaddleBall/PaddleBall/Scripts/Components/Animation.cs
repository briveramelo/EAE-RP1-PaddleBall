using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct SpriteSheetSpecs {
    public int width, height, numFrames, framesPerRow, numGameFramesPerAnimFrame, xGap, yGap;

    public SpriteSheetSpecs(int frameWidth, int frameHeight, int numFrames, int framesPerRow,int numGameFramesPerAnimFrame, int xGap, int yGap) {
        this.width = frameWidth;
        this.height= frameHeight;
        this.numFrames = numFrames;
        this.framesPerRow = framesPerRow;
        this.numGameFramesPerAnimFrame = numGameFramesPerAnimFrame;
        this.xGap = xGap;
        this.yGap = yGap;
    }
}

namespace PaddleBall {
    /// <summary>
    /// For uniform-size rectangular snapshots from a spritesheet
    /// </summary>
    public class Animation {
        protected Texture2D spriteSheet;
        protected Rectangle[] sourceRectangles;
        public int currentSourceRectIndex;
        protected ContentManager content;
        protected string spriteSheetPath;
        protected SpriteSheetSpecs spriteSheetSpecs;
        protected GameObject parentGameObject;
        public int loopNumber;

        public Animation(GameObject parentGameObject, SpriteSheetSpecs spriteSheetSpecs, string spriteSheetPath) {
            this.parentGameObject = parentGameObject;
            this.spriteSheetSpecs = spriteSheetSpecs;
            this.spriteSheetPath = spriteSheetPath;
        }

        public void LoadContent(ContentManager Content) {
            content = Content;
            spriteSheet = content.Load<Texture2D>(spriteSheetPath);
        }

        public void PostLoad() {
            loopNumber = 0;
            sourceRectangles = new Rectangle[spriteSheetSpecs.numFrames];
            int row = 0;
            for (int frame = 0; frame < spriteSheetSpecs.numFrames; frame++) {
                if (frame % spriteSheetSpecs.framesPerRow == 0 && frame!=0) {
                    row++;
                }
                sourceRectangles[frame] = new Rectangle((frame% spriteSheetSpecs.framesPerRow) * (spriteSheetSpecs.width+spriteSheetSpecs.xGap), row * (spriteSheetSpecs.height+spriteSheetSpecs.yGap), spriteSheetSpecs.width, spriteSheetSpecs.height);
            }
        }

        public void Reset() {
            currentSourceRectIndex = 0;
            loopNumber = 0;
            gameFramesOnCurrentAnimFrame = 0;
        }

        protected int gameFramesOnCurrentAnimFrame;
        public virtual void Draw(SpriteBatch spriteBatch) {
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
            spriteBatch.Draw(spriteSheet, parentGameObject.position, sourceRectangles[currentSourceRectIndex], Color.White, parentGameObject.rotation, parentGameObject.originInPixels, parentGameObject.scale, parentGameObject.flip, parentGameObject.layerDepth);
            gameFramesOnCurrentAnimFrame++;
        }

    }
}
