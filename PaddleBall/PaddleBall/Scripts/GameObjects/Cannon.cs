using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System;
namespace PaddleBall {

    public class Cannon : GameObject
    {

        bool isClockWise = true;
        float degPerSec = 1f;
        float radPerSec;

        CircleCollider myCollider;
        float forwardOffsetRotation = (float)Math.PI / 2f;
        CannonBall cannonBall;

        public override Vector2 forward {
            get {
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
        }

        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/Paddle";
            radPerSec = degPerSec * (float)Math.PI / 180f;
            SetLayerDepth(0f);
            position = screenCenter;
            myCollider = new CircleCollider(Layer.Cannon, this, 120);
            base.LoadContent(Content);
        }

        public override void PostLoad() {
            cannonBall = new CannonBall();
            cannonBall.LoadContent(content);
            cannonBall.PostLoad();
            GameObject.allGameObjects.Add(cannonBall);
            base.PostLoad();
        }

        KeyboardState lastKeyboardState;
        MouseState lastMouseState;
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            //rotate clockwise by default
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.D)) && lastKeyboardState != keyboardState) {
                SwitchDirection();
            }
            rotation += radPerSec;
            SetRotation(rotation);
            if ((keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState != keyboardState) ||
                (mouseState.LeftButton == ButtonState.Pressed && lastMouseState!=mouseState)){

                Fire();
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
                    AudioManager.Instance.PlaySound(SoundFX.Hit);
                    overlappingCollider.gameObject.Destroy();
                    overlappingCollider.Destroy();
                    Lose();
                }
            }
        }
        void Lose() {
            //TODO set lose state
        }

        void SwitchDirection(){
            isClockWise = !isClockWise;
            radPerSec = -radPerSec;
        }
       
        float fireSpeed = 65f;
        void Fire() {
            if (cannonBall.isAttached) {
                AudioManager.Instance.PlaySound(SoundFX.Launch);
                cannonBall.Launch(forward * fireSpeed, isClockWise);
            }
        }

    }
}
