﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
namespace PaddleBall {

    public class Cannon : GameObject
    {

        bool isClockWise = true;
        float degPerSec = 1f;
        float radPerSec;


        float forwardOffsetRotation = (float)Math.PI / 2f;
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
        }

        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/Paddle";
            radPerSec = degPerSec * (float)Math.PI / 180f;
            SetLayerDepth(0f);
            position = screenCenter;
            base.LoadContent(Content);
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
                EnemyCreate();
            }


            lastState = state;
        }

        void SwitchDirection()
        {
            isClockWise = !isClockWise;
            radPerSec = -radPerSec;
        }

        float fireSpeed = 20f;
        void Fire()
        {
            CannonBall newBall = new CannonBall();
            newBall.LoadContent(content);
            newBall.PostLoad();
            GameObject.allGameObjects.Add(newBall);
            newBall.SetVelocity(forward * fireSpeed);
        }

        void EnemyCreate()
        {
            Enemy newEnemy = new Enemy();
            int x = GetRandomNumber(0, 2500);
            int y = GetRandomNumber(0, 1030);
            newEnemy.SetPosition(x,y);
            newEnemy .LoadContent(content);
            newEnemy.PostLoad();
            GameObject.allGameObjects.Add(newEnemy);
            Vector2 pos = (screenCenter - new Vector2(x, y));
            pos.Normalize();
            newEnemy.SetVelocity(pos);
        }

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }





    }
}
