using Microsoft.Xna.Framework;
using System;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall
{
    class MegaLaserCollider : GameObject
    {
        public CircleCollider myCollider;
        Vector2 velocity = Vector2.Zero;
        public MegaLaserCollider(Vector2 position) : base() {
            this.position = position;
        }
        float scaleSize = 1f;
        public override void LoadContent(ContentManager Content){}

        public override void PostLoad(){
            SetLayerDepth(0f);
            scale = Vector2.One * scaleSize;
            float radius = 50;
            myCollider = new CircleCollider(Layer.CannonBall, this, radius * scaleSize);
            SetOriginInPixels(radius / 2, radius / 2);
        }

        /// <summary>
        /// Launches the cannonball
        /// </summary>
        /// <param name="velocity"></param>
        public void Launch(Vector2 velocity){
            this.velocity = velocity;
        }
        
        float distanceToDestroy = 2000;
        float timeToDestroy = 0;
        public override void Update(GameTime gameTime){
            position += velocity;
            CheckForCollision();

            if (Vector2.Distance(position, screenCenter) > distanceToDestroy){
                ScoreBoard.Instance.ReportMiss();
                Destroy();
            }
            base.Update(gameTime);
        }

        void CheckForCollision(){
            CircleCollider enemyCollider = myCollider.GetOverlappingCollider();
            if (enemyCollider != null){
                if (enemyCollider.layer == Layer.Enemy){
                    ((Enemy)(enemyCollider.gameObject)).TakeDamage();
                    Destroy();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {}

        public override void Destroy(){
            myCollider.Destroy();
            base.Destroy();
        }
    }

}

