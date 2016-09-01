using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

using System.Collections;
using System.Collections.Generic;


namespace PaddleBall
{
    class Enemy : GameObject
    {
        int health = 1;
        Vector2 velocity = Vector2.Zero;
        CircleCollider myCollider;

        public Enemy() : base() { }

        float scaleSize = 0.5f;
        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/bugEnemy1";
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.Enemy,this, 75 * scaleSize);
            base.LoadContent(Content);
        }


        public void SetVelocity(Vector2 moveDir, float moveSpeed)
        {
            Vector2 normalizedMoveDir = Vector2.Normalize(moveDir);
            this.velocity = normalizedMoveDir *moveSpeed;
            float degToRad = (float)Math.PI / 180f;
            rotation = (float)Math.Atan2(normalizedMoveDir.Y, normalizedMoveDir.X) - 90f* degToRad;
        }
       
        public override void Update(GameTime gameTime)
        {
            position += velocity;
        }

        public void TakeDamage() {
            ScoreBoard.Instance.AddPoints();
            health--;
            if (health <= 0) {
                AudioManager.Instance.PlaySound(SoundFX.Explode);
                myCollider.Destroy();
                Destroy();
            }
        }

        public override void Destroy() {
            EnemySpawner.Instance.ReportEnemyDown();
            base.Destroy();
        }



    }
}