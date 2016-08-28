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


        float forwardOffsetRotation = (float)Math.PI / 2f;
        List<CannonBall> cannonBalls = new List<CannonBall>();

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
            base.LoadContent(Content);
        }

        public override void PostLoad() {
            CannonBall newBall = new CannonBall();
            newBall.LoadContent(content);
            newBall.PostLoad();
            GameObject.allGameObjects.Add(newBall);
            cannonBalls.Add(newBall);
            base.PostLoad();
        }

        KeyboardState lastState;
        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            //rotate clockwise by default
            if ((state.IsKeyDown(Keys.Right) && isClockWise) || (state.IsKeyDown(Keys.Left) && !isClockWise))
            {
                SwitchDirection();
            }
            rotation += radPerSec;
            SetRotation(rotation);
            if (state.IsKeyDown(Keys.Space) && lastState != state)
            {
                Fire();
            }


            lastState = state;
            base.Update(gameTime);
        }

        void SwitchDirection()
        {
            isClockWise = !isClockWise;
            radPerSec = -radPerSec;
        }
       
        float fireSpeed = 50f;
        void Fire() {
            for (int i = cannonBalls.Count-1; i >=0; i--) {
                if (cannonBalls[i].isAttached) {
                    cannonBalls[i].Launch(forward * fireSpeed);
                    return;
                }
            }

        }




    }
}
