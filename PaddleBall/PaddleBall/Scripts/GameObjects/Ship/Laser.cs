using Microsoft.Xna.Framework;
using System;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {

    /// <summary>
    /// The laser the ship shoots 
    /// </summary>
    class Laser : GameObject {

        public CircleCollider myCollider;
        Vector2 velocity = Vector2.Zero;
        public static int numCannonBalls;
        Animation myAnimation;
        SpriteSheetSpecs mySpriteSheetSpecs;

        public Laser(Vector2 position) : base() {
            this.position = position;
            numCannonBalls++;
        }

        float scaleSize = 1.5f;
        public override void LoadContent(ContentManager Content) {
            mySpriteSheetSpecs = new SpriteSheetSpecs(108,66,9,4,3,0,0);
            myAnimation = new Animation(this, mySpriteSheetSpecs, "Images/Spritesheets/Laser");
            myAnimation.LoadContent(Content);
            myAnimation.PostLoad();
            //texturePath = "Images/laser";          
            //base.LoadContent(Content);
        }

        public override void PostLoad() {
            SetLayerDepth(0f);
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.CannonBall, this, 50);
            SetOriginInPixels(mySpriteSheetSpecs.width / 2, mySpriteSheetSpecs.height / 2);
            //base.PostLoad();
        }

        /// <summary>
        /// Launches the cannonball
        /// </summary>
        /// <param name="velocity"></param>
        public void Launch(Vector2 velocity) {
            this.velocity = velocity;
            rotation = (float)(Math.Atan2(velocity.Y, velocity.X));
        }

        float distanceToDestroy = 2000;
        public override void Update(GameTime gameTime) {
            position += velocity;
            CheckForCollision();

            if (Vector2.Distance(position, screenCenter) > distanceToDestroy) {
                ScoreBoard.Instance.ReportMiss();
                Destroy();
            }
            base.Update(gameTime);
        }

        void CheckForCollision() {
            CircleCollider enemyCollider = myCollider.GetOverlappingCollider();
            if (enemyCollider != null) {
                if (enemyCollider.layer == Layer.Enemy) {
                    ((Enemy)(enemyCollider.gameObject)).TakeDamage();                    
                    Destroy();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            myAnimation.Draw(spriteBatch);
        }

        public override void Destroy() {
            myCollider.Destroy();
            numCannonBalls--;
            base.Destroy();
        }
    }
}
