using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace PaddleBall {

    class CannonBall : GameObject {

        Vector2 velocity = Vector2.Zero;

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
        public void SetVelocity(Vector2 velocity) {
            this.velocity = velocity;
        }

        float distanceToDestroy = 2000;
        public override void Update(GameTime gameTime) {
            position += velocity;

            if (Vector2.Distance(position, screenCenter) > distanceToDestroy) {
                Destroy();
            }
        }


    }
}
