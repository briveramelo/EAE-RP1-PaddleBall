using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Collections;

namespace PaddleBall
{
    /// <summary>
    /// Your trusty ship!
    /// Launches lasers and megalasercolliders
    /// Also handles input for rotating and firing
    /// </summary>
    public class Ship : GameObject
    {

        float baseDegPerSec = 4f;
        float slowDegPerSec = 2f;
        float radPerSec;
        int maxBalls;
        public bool isMegaLaserFirable = false;
        CircleCollider myCollider;
        float forwardOffsetRotation = (float)Math.PI / 2f;
        Laser cannonBall;
        MegaLaserAnimation megaLaserAnimation;

        public Ship() : base() { }

        public override Vector2 forward
        {
            get
            {
                float endRotation = rotation + forwardOffsetRotation;
                return new Vector2((float)Math.Cos(endRotation), (float)Math.Sin(endRotation));
            }
        }

        private static Ship instance;
        public static Ship Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Ship();
                }
                return instance;
            }
            set { instance = value; }
        }

        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/Ship/Ship";
            base.LoadContent(Content);
        }

        public override void PostLoad()
        {
            float scaleSize = 0.5f;
            radPerSec = baseDegPerSec * (float)Math.PI / 180f;
            position = screenCenter;
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.Cannon, this, 120 * scaleSize);
            maxBalls = 2;

            SpriteSheetSpecs megaLaserSpriteSheetSpecs = new SpriteSheetSpecs(100,100,9,5,3,0,0);
            megaLaserAnimation = new MegaLaserAnimation(this, megaLaserSpriteSheetSpecs, "Images/spritesheets/MegaLaser");
            megaLaserAnimation.LoadContent(content);
            megaLaserAnimation.PostLoad();
            megaLaserAnimation.SetScale(new Vector2(1, 12));
            megaLaserAnimation.SetOrigin();

            base.PostLoad();
        }

        KeyboardState lastKeyboardState;
        MouseState lastMouseState;
        float timeUntilMegaLaser = 5000;
        double elaspedTime = 0;
        bool isFiringLaser;
        public override void Update(GameTime gameTime)
        {
            elaspedTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();


            if (isMegaLaserFirable && ( (keyboardState.IsKeyDown(Keys.LeftAlt)) || mouseState.RightButton == ButtonState.Pressed))
            {
                timeUntilMegaLaser -= (float)elaspedTime;
            }

            if (timeUntilMegaLaser <= 0)
                isMegaLaserFirable = false;
       
            HandleRotation(keyboardState);


            if ((keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState != keyboardState) ||
                (mouseState.LeftButton == ButtonState.Pressed && lastMouseState != mouseState))
            {
                if (Laser.numCannonBalls < maxBalls && !isPausedForDelay)
                {
                    Fire();
                }
            }


            if ((keyboardState.IsKeyDown(Keys.LeftControl)) ||
                (mouseState.RightButton == ButtonState.Pressed )){
                if (isMegaLaserFirable) {
                    isFiringLaser = true;
                    FireMegaLaser();
                }
                else {
                    isFiringLaser = false;
                }
            }
            else {
                isFiringLaser = false;
            }

            CheckForCollision();

            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            base.Update(gameTime);
        }

        void HandleRotation(KeyboardState keyboardState)
        {
            if ((keyboardState.IsKeyDown(Keys.LeftShift) && lastKeyboardState.IsKeyUp(Keys.LeftShift)))
            {
                radPerSec = slowDegPerSec * (float)Math.PI / 180f;
            }
            else if ((keyboardState.IsKeyUp(Keys.LeftControl) && lastKeyboardState.IsKeyDown(Keys.LeftControl)) ||
                (keyboardState.IsKeyUp(Keys.LeftShift) && lastKeyboardState.IsKeyDown(Keys.LeftShift)))
            {
                radPerSec = baseDegPerSec * (float)Math.PI / 180f;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                rotation += radPerSec;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                rotation -= radPerSec;
            }
        }

        void CheckForCollision()
        {
            CircleCollider overlappingCollider = myCollider.GetOverlappingCollider();
            if (overlappingCollider != null)
            {
                if (overlappingCollider.layer == Layer.Enemy)
                {
                    overlappingCollider.gameObject.Destroy();
                    overlappingCollider.Destroy();
                    Lose();
                }
            }
        }

        float fireSpeed = 65f;
        float megaLaserSpeed = 100f;
        void Fire()
        {
            cannonBall = new Laser(position + forward * 75);
            cannonBall.LoadContent(content);
            cannonBall.PostLoad();
            cannonBall.Launch(forward * fireSpeed);

            myCoroutiner.StartCoroutine(PauseForFireDelay());
            AudioManager.Instance.PlaySound(SoundFX.Launch);
        }

        public void ActivateMegaLaser() {
            isMegaLaserFirable = true;
        }

        void FireMegaLaser()
        {            
            MegaLaserCollider[] laser = new MegaLaserCollider[21];
            for (int i = 0; i < laser.Length; i++)
            {
                laser[i] = new MegaLaserCollider(position + ForwardDist(i));
                laser[i].LoadContent(content);
                laser[i].PostLoad();
                laser[i].Launch(forward * megaLaserSpeed);
            }
            //myCoroutiner.StartCoroutine(PauseForFireDelay());
            AudioManager.Instance.PlaySound(SoundFX.Launch);
        }

        void Lose()
        {
            AudioManager.Instance.PlaySound(SoundFX.PaddleDeath);
            AudioManager.Instance.StopMusic();
            GameManager.Instance.Lose();


            ShipExplosion shipExp = new ShipExplosion();
            shipExp.LoadContent(content);
            shipExp.PostLoad();
            shipExp.position = position;

            Destroy();
        }

        bool isPausedForDelay;
        IEnumerator PauseForFireDelay()
        {
            isPausedForDelay = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float fireDelay = 0.1f;
            while (stopwatch.ElapsedMilliseconds < fireDelay * 1000)
            {
                yield return null;
            }
            stopwatch.Stop();
            isPausedForDelay = false;
        }

        public override void Destroy()
        {
            Instance = null;
            base.Destroy();
        }

        public Vector2 ForwardDist(int i)
        {

            float endRotation = rotation + forwardOffsetRotation;
            return new Vector2((float)(75 * i * Math.Cos(endRotation)), (75 * i * (float)Math.Sin(endRotation)));
        }

        public override void Draw(SpriteBatch spriteBatch) {

            if (isFiringLaser) {
                megaLaserAnimation.SetPosition(position + forward*20f);                
                megaLaserAnimation.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

    }
}