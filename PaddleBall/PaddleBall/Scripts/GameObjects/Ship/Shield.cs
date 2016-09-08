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
        List<Animation> shieldAnimations;
        SpriteSheetSpecs spriteSheetSpecs;

        int health = 3;

        public Shield() : base() { }

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
            content = Content;
            spriteSheetSpecs = new SpriteSheetSpecs(750, 750, 17, 5, 3, 0, 0);
            shieldAnimations = new List<Animation>();
            shieldAnimations.Add(new Animation(this, spriteSheetSpecs, "Images/Spritesheets/RedShieldPulseOne"));
            shieldAnimations.Add(new Animation(this, spriteSheetSpecs, "Images/Spritesheets/OrangeShieldPulseOne"));
            shieldAnimations.Add(new Animation(this, spriteSheetSpecs, "Images/Spritesheets/YellowShieldPulseOne"));
            shieldAnimations.ForEach(anim => anim.LoadContent(Content));
            shieldAnimations.ForEach(anim => anim.PostLoad());
        }

        public override void PostLoad() {
            SetLayerDepth(0f);
            scale = Vector2.One * scaleSize;
            myCollider = new CircleCollider(Layer.Shield, this, 350 * scaleSize);
            position = screenCenter;
            SetOriginInPixels(spriteSheetSpecs.width/ 2, spriteSheetSpecs.height / 2);
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
            PulseManager.Instance.Animate();
            if (health == 0) {
                Destroy();
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            shieldAnimations[health - 1].Draw(spriteBatch);
            //spriteBatch.Draw(textures[health-1], position, null, color, rotation, originInPixels, scale, flip, layerDepth);
        }

        public override void Destroy() {
            Instance = null;
            base.Destroy();
        }

    }
}
