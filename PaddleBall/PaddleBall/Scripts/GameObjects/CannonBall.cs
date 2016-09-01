using Microsoft.Xna.Framework;
using System;
using System.Collections;
using Microsoft.Xna.Framework.Content;

namespace PaddleBall {

    class CannonBall : GameObject {

        public CircleCollider myCollider;
        Vector2 velocity = Vector2.Zero;
        Vector2 linearVelocity = Vector2.Zero;
        Vector2 gravitationalVelocity = Vector2.Zero;
        Vector2 frontOffset {
            get {
                return Cannon.Instance.forward * distanceOffset;
            }
        }
        Vector2 backOffset {
            get {
                return -Cannon.Instance.forward * distanceOffset;
            }
        }
        Vector2 frontSpot { get { return frontOffset + screenCenter; } }
        Vector2 backSpot { get { return backOffset + screenCenter; } }

        float gravConst=0.000005f;
        float distanceOffset = 75;
        public bool isAttached = true;
        bool isPausedForLaunch = false;

        public CannonBall() : base() { }


        float scaleSize = 1f;
        public override void LoadContent(ContentManager Content) {
            texturePath = "Images/TempBall";
            SetLayerDepth(0f);
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.CannonBall, this, 50 * scaleSize);
            base.LoadContent(Content);
        }

        /// <summary>
        /// Launches the cannonball
        /// </summary>
        /// <param name="velocity"></param>
        public void Launch(Vector2 velocity, bool isClockwise) {
            isAttached = false;
            gravitationalVelocity = Vector2.Zero;
            this.linearVelocity = (onFront ? 1:-1) * velocity;
            isPausedForLaunch = true;
            hitEnemy = false;
            myCoroutiner.StopAllCoroutines();
            myCoroutiner.StartCoroutine(PauseAttachment());
        }

        IEnumerator PauseAttachment() {            
            yield return null;
            isPausedForLaunch = false;
        }

        float distanceToDestroy = 2000;
        float distanceCheckSpacing = 120;
        bool onFront = true;
        bool hitEnemy = false;
        public override void Update(GameTime gameTime) {
            if (!isAttached) {
                GravitateBack();
                if (!isPausedForLaunch) {
                    if (Vector2.Distance(position, frontSpot) < distanceCheckSpacing) {
                        Attach(true);
                    }
                    else if (Vector2.Distance(position, backSpot) < distanceCheckSpacing) {
                        Attach(false);
                    }
                }
            }
            else {
                StayAttachedToPaddle();
            }

            if (!isAttached) {
                CheckForCollision();
            }

            if (Vector2.Distance(position, screenCenter) > distanceToDestroy) {
                Destroy();
            }
            base.Update(gameTime);
        }

        void CheckForCollision() {
            CircleCollider enemyCollider = myCollider.GetOverlappingCollider();
            if (enemyCollider != null) {
                if (enemyCollider.layer == Layer.Enemy) {
                    ((Enemy)(enemyCollider.gameObject)).TakeDamage();                    
                    Collide();
                }
            }
        }

        /// <summary>
        /// Attaches back to the cannon/paddle. Will attach to the nearest side.
        /// </summary>
        /// <param name="isOnFront"></param>
        void Attach(bool isOnFront) {
            onFront = isOnFront;
            isAttached = true;
            position = (onFront ? frontSpot : backSpot);
            if (!hitEnemy) {
                ScoreBoard.Instance.ReportMiss();
            }
            AudioManager.Instance.PlaySound(SoundFX.ReAttach);
        }

        Random random = new Random();
        float collisionSpeedMultiplier = 1f;
        /// <summary>
        /// Bounces straight back to the cannon when moving away, or bounces a bit away when moving back
        /// </summary>
        public void Collide() {
            bool movingAway = Math.Sign(velocity.X) == Math.Sign(position.X - screenCenter.X);
            float bounceSpeed = (movingAway ? 1 : -1) * Vector2.Distance(Vector2.Zero, velocity) * collisionSpeedMultiplier;
            hitEnemy = true;
            linearVelocity = Vector2.Normalize(screenCenter - position) * bounceSpeed;
        }
        
        /// <summary>
        /// adds a gravitational velocity for a magnetic effect
        /// </summary>
        void GravitateBack() {
            velocity = linearVelocity + gravitationalVelocity;
            position += velocity;
            float dist = Vector2.Distance(screenCenter, position);
            gravitationalVelocity += Vector2.Normalize(screenCenter - position) * (dist * dist) * gravConst;
        }        
        
        /// <summary>
        /// Sets position to the front or back of the paddle
        /// </summary>
        void StayAttachedToPaddle() {
            position = (onFront ? frontSpot : backSpot);
        }

    }
}
