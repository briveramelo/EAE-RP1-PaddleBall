using Microsoft.Xna.Framework;
using System;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PaddleBall
{
    class Laser:GameObject
    {
        public CircleCollider myCollider;
        Vector2 velocity = Vector2.Zero;
        public static int numLaser = 0;
        public int objToLaserNumber = 0;
        public Laser(Vector2 position) : base() {
            this.position = position;
            numLaser++;
        }
        float scaleSize = 1f;
        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/TempBall";
            base.LoadContent(Content);
        }

        public override void PostLoad()
        {
            SetLayerDepth(0f);
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.CannonBall, this, 50 * scaleSize);
            base.PostLoad();
        }

        /// <summary>
        /// Launches the cannonball
        /// </summary>
        /// <param name="velocity"></param>
        public void Launch(Vector2 velocity)
        {

            this.velocity = velocity;
        }


        
        float distanceToDestroy = 2000;
        float timeToDestroy = 0;
        public override void Update(GameTime gameTime)
        {
            double elaspedTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            timeToDestroy = timeToDestroy + (float) elaspedTime;
            position += Cannon.Instance.ForwardDist(objToLaserNumber);
            CheckForCollision();
          

            if (Vector2.Distance(position, screenCenter) > distanceToDestroy)
            {
                ScoreBoard.Instance.ReportMiss();
                Destroy();
            }

            if (timeToDestroy > 400)
            {

                Destroy();
            }
            base.Update(gameTime);
        }

        void CheckForCollision()
        {
            CircleCollider enemyCollider = myCollider.GetOverlappingCollider();
            if (enemyCollider != null)
            {
                if (enemyCollider.layer == Layer.Enemy)
                {
                    ((Enemy)(enemyCollider.gameObject)).TakeDamage();
                   // Destroy();
                }
            }
        }


        public override void Destroy()
        {
            myCollider.Destroy();
           // Cannon.Instance.checkLaserState = false;
            numLaser--;
            base.Destroy();
        }
    }

}

