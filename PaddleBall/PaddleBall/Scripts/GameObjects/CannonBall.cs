﻿using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PaddleBall {

    class CannonBall : GameObject {

        Vector2 initialVelocity = Vector2.Zero;
        Vector2 gravitationalVelocity = Vector2.Zero;
        float gravConst=.0025f;
        public bool isAttached = true;
        bool isPausedForLaunch = false;

        public CannonBall() : base() { }

        public override void LoadContent(ContentManager Content) {
            texturePath = "Images/CannonBall";
            SetLayerDepth(0f);
            position = screenCenter;
            base.LoadContent(Content);
        }

        /// <summary>
        /// Velocity in pixels/updateTime
        /// </summary>
        /// <param name="velocity"></param>
        public void Launch(Vector2 velocity) {
            gravitationalVelocity = Vector2.Zero;
            this.initialVelocity = velocity;
            isAttached = false;
            StartCoroutine(PauseAttachment());
        }

        IEnumerator PauseAttachment() {
            float timeToPause = 0.1f;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            isPausedForLaunch = true;
            while (true) {
                if (stopwatch.ElapsedMilliseconds >= timeToPause * 1000) {
                    break;
                }
                yield return null;
            }
            isPausedForLaunch = false;
        }

        float distanceToDestroy = 2000;
        public override void Update(GameTime gameTime) {
            if (!isAttached) {
                FloatOut();
                if (Vector2.Distance(position, screenCenter) < distanceOffset && !isPausedForLaunch) {
                    isAttached = true;
                    AudioManager.Instance.PlaySound(SoundFX.ReAttach);
                }
            }
            else {
                StayAttachedToPaddle();
            }            


            if (Vector2.Distance(position, screenCenter) > distanceToDestroy) {
                Destroy();
            }
            base.Update(gameTime);
        }

        Random random = new Random();
        float collisionVelocity = 20f;
        public void Collide(Vector2 newDirection) {
            initialVelocity = Vector2.Normalize(new Vector2(random.Next(0, 100), random.Next(0, 100))) * collisionVelocity;
        }
        

        void FloatOut() {
            position += initialVelocity + gravitationalVelocity;
            gravitationalVelocity += (screenCenter - position) * gravConst;
        }

        float distanceOffset = 100;
        void StayAttachedToPaddle() {
            position = Cannon.Instance.forward * distanceOffset + screenCenter;
        }
        

    }
}
