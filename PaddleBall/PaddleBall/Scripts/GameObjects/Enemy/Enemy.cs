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
        Animation redBugAnimation, greenBugWalkAnimation;
        SpriteSheetSpecs mySpriteSheetSpecs;

        public Enemy() : base() { }

        float scaleSize = 0.5f;
        public override void LoadContent(ContentManager Content)
        {
            content = Content;
            mySpriteSheetSpecs = new SpriteSheetSpecs(264, 236, 12, 3, 3, 0, 0);
            redBugAnimation = new Animation(this, mySpriteSheetSpecs, "Images/Spritesheets/Bugs/RedBug_Walk");
            greenBugWalkAnimation = new Animation(this, mySpriteSheetSpecs, "Images/Spritesheets/Bugs/GreenBug_Walk");

            redBugAnimation.LoadContent(Content); greenBugWalkAnimation.LoadContent(Content);
            redBugAnimation.PostLoad(); greenBugWalkAnimation.PostLoad();

            //texturePath = "Images/bugEnemy1";
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.Enemy,this, 75 * scaleSize);
            //base.LoadContent(Content);
        }

        public override void PostLoad() {
            SetOriginInPixels(mySpriteSheetSpecs.width/2, mySpriteSheetSpecs.height/ 2); 
            //base.PostLoad();
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

        public override void Draw(SpriteBatch spriteBatch) {
            redBugAnimation.Draw(spriteBatch);
            //base.Draw(spriteBatch);
        }

        public void TakeDamage() {
            ScoreBoard.Instance.AddPoints(Vector2.Distance(position, screenCenter));
            AudioManager.Instance.PlaySound(SoundFX.DestroyEnemy);
            health--;
            if (health <= 0) {
                Destroy();
            }
        }

        public override void Destroy() {
            EnemySpawner.Instance.ReportEnemyDown();

            BugDeathAnimation bugDeathAnim = new BugDeathAnimation();
            bugDeathAnim.LoadContent(content);
            bugDeathAnim.PostLoad();
            bugDeathAnim.position = position;
            bugDeathAnim.rotation = rotation;
            bugDeathAnim.scale = scale;

            myCollider.Destroy();
            base.Destroy();
        }
    }
}