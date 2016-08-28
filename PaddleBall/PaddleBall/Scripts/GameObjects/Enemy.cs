using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

namespace PaddleBall
{
    class Enemy : GameObject
    {
        float health = 1.0f;
        Vector2 velocity = Vector2.Zero;
        //  Vector2 position = Vector2.Zero;

        public Enemy() : base()
        { }

        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/Enemy_one";
            SetLayerDepth(0f);
          //  position = new Vector2(100,100);
            base.LoadContent(Content);
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (Vector2.Distance(position, screenCenter)<100)
            {
                Destroy();
            }
        }

        /*
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

*/
    }
}
