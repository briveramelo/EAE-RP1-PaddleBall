using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {

    class CannonBall : GameObject {

        public CircleCollider myCollider;
        Vector2 velocity = Vector2.Zero;
        Vector2 linearVelocity = Vector2.Zero;
        Vector2 gravitationalVelocity = Vector2.Zero;
        Vector2 curveVelocity = Vector2.Zero;
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
        float distanceOffset = 100;
        public bool isAttached = true;
        bool isPausedForLaunch = false;
        Rectangle ballRec = new Rectangle(0, 0, 300, 300);

        public CannonBall() : base() { }



        public override void LoadContent(ContentManager Content) {
            texturePath = "Images/TempBall";
            SetLayerDepth(0f);
            myCollider = new CircleCollider(Layer.CannonBall, this, 50);
            base.LoadContent(Content);
        }

        /// <summary>
        /// Velocity in pixels/updateTime
        /// </summary>
        /// <param name="velocity"></param>
        public void Launch(Vector2 velocity, bool isClockwise) {
            isAttached = false;
            gravitationalVelocity = Vector2.Zero;
            curveVelocity = Vector2.Zero;
            this.linearVelocity = (onFront ? 1:-1) * velocity;
            isPausedForLaunch = true;
            hitEnemy = false;
            myCoroutiner.StopAllCoroutines();
            myCoroutiner.StartCoroutine(PauseAttachment());
            //StartCoroutine(CurveTheBall(isClockwise));
        }

        IEnumerator CurveTheBall(bool isClockwise) {
            float timeToCurve = .3f;
            float curveFactor = 0.01f;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Vector3 initialVelocity3 = new Vector3(linearVelocity.X, linearVelocity.Y, 0f);
            Vector3 curvDir3 = Vector3.Cross(initialVelocity3, (isClockwise ? -1 : 1)*Vector3.UnitZ);
            Vector2 curvDir2 = new Vector2(curvDir3.X, curvDir3.Y);
            while (stopwatch.ElapsedMilliseconds < timeToCurve * 1000) {
                curveVelocity += curvDir2 * curveFactor;
                yield return null;
            }
        }

        IEnumerator PauseAttachment() {            
            float timeToPause = .1f;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds < timeToPause * 1000) {
                yield return null;
            }
            stopwatch.Stop();
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
                    Debug.WriteLine("Hit an Enemy!");
                    ((Enemy)(enemyCollider.gameObject)).TakeDamage();                    
                    Collide();
                }
            }
        }

        void Attach(bool isOnFront) {
            onFront = isOnFront;
            isAttached = true;
            position = (onFront ? frontSpot : backSpot);
            if (hitEnemy) {
                ScoreBoard.Instance.ReportMiss();
            }
            AudioManager.Instance.PlaySound(SoundFX.ReAttach);
        }

        Random random = new Random();
        float collisionSpeedMultiplier = 1f;
        public void Collide() {
            bool movingAway = Math.Sign(velocity.X) == Math.Sign(position.X - screenCenter.X);
            float bounceSpeed = (movingAway ? 1 : -1) * Vector2.Distance(Vector2.Zero, velocity) * collisionSpeedMultiplier;
            hitEnemy = true;
            linearVelocity = Vector2.Normalize(screenCenter - position) * bounceSpeed;
        }
        

        void GravitateBack() {
            velocity = linearVelocity + gravitationalVelocity + curveVelocity;
            position += velocity;
            float dist = Vector2.Distance(screenCenter, position);
            gravitationalVelocity += Vector2.Normalize(screenCenter - position) * (dist * dist) * gravConst;
        }
        

        
        void StayAttachedToPaddle() {
            position = (onFront ? frontSpot : backSpot);
        }

    }
}
