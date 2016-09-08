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

    public class Cannon : GameObject
    {

        float baseDegPerSec = 4f;
        float slowDegPerSec = 2f;
        float radPerSec;
        int maxBalls;
        public bool checkLaserState = false;
        CircleCollider myCollider;
        float forwardOffsetRotation = (float)Math.PI / 2f;
        CannonBall cannonBall;

        public Cannon() : base() { }

        public override Vector2 forward
        {
            get
            {
                float endRotation = rotation + forwardOffsetRotation;
                return new Vector2((float)Math.Cos(endRotation), (float)Math.Sin(endRotation));
            }
        }

        private static Cannon instance;
        public static Cannon Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Cannon();
                }
                return instance;
            }
            set { instance = value; }
        }

        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/Paddle";
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

            base.PostLoad();
        }

        KeyboardState lastKeyboardState;
        MouseState lastMouseState;
        float timeUntilMegaLaser = 5000;
        double elaspedTime = 0;
        public override void Update(GameTime gameTime)
        {
            elaspedTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();


            if (checkLaserState==true && ( (keyboardState.IsKeyDown(Keys.LeftAlt)) || mouseState.RightButton == ButtonState.Pressed))
            {
                timeUntilMegaLaser -= (float)elaspedTime;
            }

            if (timeUntilMegaLaser <= 0)
                checkLaserState = false;
       
            HandleRotation(keyboardState);


            if ((keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState != keyboardState) ||
                (mouseState.LeftButton == ButtonState.Pressed && lastMouseState != mouseState))
            {
                if (CannonBall.numCannonBalls < maxBalls && !isPausedForDelay)
                {
                    Fire();
                }
            }


            if ((keyboardState.IsKeyDown(Keys.LeftAlt)) ||
                (mouseState.RightButton == ButtonState.Pressed ))
            {
                if(!isPausedForDelay)
                FireLaser();
            }

            CheckForCollision();

            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            base.Update(gameTime);
        }

        void HandleRotation(KeyboardState keyboardState)
        {
            if ((keyboardState.IsKeyDown(Keys.LeftControl) && lastKeyboardState.IsKeyUp(Keys.LeftControl)) ||
                (keyboardState.IsKeyDown(Keys.LeftShift) && lastKeyboardState.IsKeyUp(Keys.LeftShift)))
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
        void Fire()
        {
            cannonBall = new CannonBall(position + forward * 75);
            cannonBall.LoadContent(content);
            cannonBall.PostLoad();
            cannonBall.Launch(forward * fireSpeed);

            myCoroutiner.StartCoroutine(PauseForFireDelay());
            AudioManager.Instance.PlaySound(SoundFX.Launch);
        }
        void FireLaser()
        {
            if (checkLaserState == true)
            {
                Laser[] laser = new Laser[21];
                for (int i = 0; i < 20; i++)
                {
                    laser[i] = new Laser(position + ForwardDist(i));
                    laser[i].LoadContent(content);
                    laser[i].PostLoad();
                    laser[i].objToLaserNumber = i;
                }
            }
            //myCoroutiner.StartCoroutine(PauseForFireDelay());
            AudioManager.Instance.PlaySound(SoundFX.Launch);
        }

        void Lose()
        {
            AudioManager.Instance.PlaySound(SoundFX.PaddleDeath);
            AudioManager.Instance.StopMusic();
            GameManager.Instance.Lose();
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

    }
}