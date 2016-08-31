using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace PaddleBall {
    public class Shield : GameObject {

        CircleCollider myCollider;
        int health = 3;

        private static Shield instance;
        public static Shield Instance {
            get {
                if (instance == null) {
                    instance = new Shield();
                }
                return instance;
            }
        }

        public override void LoadContent(ContentManager Content) {
            texturePath = "Images/Barrier";
            SetLayerDepth(0f);
            myCollider = new CircleCollider(Layer.Shield, this, 350);
            position = screenCenter;
            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime) {
            CircleCollider overlappingCollider = myCollider.GetOverlappingCollider();
            if (overlappingCollider != null) {
                if (overlappingCollider.layer == Layer.Enemy) {
                    overlappingCollider.gameObject.Destroy();
                    overlappingCollider.Destroy();
                    TakeDamage();
                }
            }
            base.Update(gameTime);
        }

        void TakeDamage() {
            AudioManager.Instance.PlaySound(SoundFX.Hit);
            health--;
            if (health == 0) {
                Destroy();
            }
        }

    }
}
