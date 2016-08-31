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
        float moveSpeed = 1f;
        CircleCollider myCollider;

        public Enemy() : base() { }
        
        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/bugEnemy1";
            myCollider = new CircleCollider(Layer.Enemy,this, 75);
            base.LoadContent(Content);
        }


        public void SetVelocity(Vector2 moveDir)
        {
            this.velocity = Vector2.Normalize(moveDir) * moveSpeed;
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