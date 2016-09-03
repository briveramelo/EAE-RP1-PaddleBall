using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {
    public class Shield : GameObject {

        CircleCollider myCollider;
        List<Texture2D> textures = new List<Texture2D>();
        int health = 3;

        private static Shield instance;
        public static Shield Instance {
            get {
                if (instance == null) {
                    instance = new Shield();
                }
                return instance;
            }
            set { instance = value; }
        }

        float scaleSize = 0.5f;
        public override void LoadContent(ContentManager Content) {
            
            SetLayerDepth(0f);
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.Shield, this, 350 * scaleSize);
            position = screenCenter;
            content = Content;
            textures.Add(content.Load<Texture2D>("Images/RED_BARRIER"));
            textures.Add(content.Load<Texture2D>("Images/ORANGE_BARRIER"));
            textures.Add(content.Load<Texture2D>("Images/Barrier"));
        }

        public override void PostLoad() {
            SetOriginInPixels(textures[health-1].Width / 2, textures[health - 1].Height / 2);
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
            AudioManager.Instance.PlaySound(SoundFX.ShieldHit);
            health--;
            if (health == 0) {
                Destroy();
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(textures[health-1], position, null, color, rotation, originInPixels, scale, flip, layerDepth);
        }

    }
}
