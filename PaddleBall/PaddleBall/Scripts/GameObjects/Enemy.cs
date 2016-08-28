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

    }
}
