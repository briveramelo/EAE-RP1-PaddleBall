﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Collections;

namespace PaddleBall {

    public class Cannon : GameObject
    {

        bool isClockWise = true;
        float degPerSec = 3.5f;
        float radPerSec;
        int maxBalls;

        CircleCollider myCollider;
        float forwardOffsetRotation = (float)Math.PI / 2f;
        CannonBall cannonBall;

        public Cannon() : base() { }

        public override Vector2 forward {
            get {
                float endRotation = rotation + forwardOffsetRotation;
                return new Vector2((float)Math.Cos(endRotation), (float)Math.Sin(endRotation));
            }
        }

        private static Cannon instance;
        public static Cannon Instance{
            get{
                if (instance == null){
                    instance = new Cannon();
                }
                return instance;
            }
            set { instance = value; }
        }

        float scaleSize = 0.5f;
        public override void LoadContent(ContentManager Content){
            texturePath = "Images/Paddle";            
            base.LoadContent(Content);
        }

        public override void PostLoad() {
            radPerSec = degPerSec * (float)Math.PI / 180f;
            SetLayerDepth(0f);
            position = screenCenter;
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.Cannon, this, 120 * scaleSize);
            maxBalls = 2;

            base.PostLoad();
        }

        KeyboardState lastKeyboardState;
        MouseState lastMouseState;
        public override void Update(GameTime gameTime){
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            //rotate clockwise by default
            //if ( (((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && isClockWise) ||
            //    (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && !isClockWise) && lastKeyboardState != keyboardState) {
            //    SwitchDirection();
            //}
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                rotation += radPerSec;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)){
                rotation -= radPerSec;
            }

            if ((keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState != keyboardState) ||
                (mouseState.LeftButton == ButtonState.Pressed && lastMouseState!=mouseState)){

                if (CannonBall.numCannonBalls<maxBalls && !isPausedForDelay) {
                    Fire();
                }
            }

            CheckForCollision();

            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            base.Update(gameTime);
        }

        void CheckForCollision() {
            CircleCollider overlappingCollider = myCollider.GetOverlappingCollider();
            if (overlappingCollider != null) {
                if (overlappingCollider.layer == Layer.Enemy) {
                    overlappingCollider.gameObject.Destroy();
                    overlappingCollider.Destroy();
                    Lose();
                }
            }
        }
        void Lose() {
            AudioManager.Instance.PlaySound(SoundFX.PaddleDeath);
            AudioManager.Instance.StopMusic();            
            GameManager.Instance.Lose();
            Destroy();
        }

        void SwitchDirection(){
            isClockWise = !isClockWise;
            radPerSec = -radPerSec;
        }
       
        float fireSpeed = 65f;
        void Fire() {
            cannonBall = new CannonBall(position + forward * 75);
            cannonBall.LoadContent(content);
            cannonBall.PostLoad();
            cannonBall.Launch(forward * fireSpeed);

            myCoroutiner.StartCoroutine(PauseForFireDelay());
            AudioManager.Instance.PlaySound(SoundFX.Launch);
        }

        bool isPausedForDelay;
        IEnumerator PauseForFireDelay() {
            isPausedForDelay = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float fireDelay = 0.1f;
            while (stopwatch.ElapsedMilliseconds < fireDelay * 1000) {
                yield return null;
            }
            stopwatch.Stop();
            isPausedForDelay =false;
        }

        public override void Destroy() {
            Instance = null;
            base.Destroy();
        }

    }
}
