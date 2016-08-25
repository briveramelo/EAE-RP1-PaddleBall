using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaddleBall {

    public class GameObject {

        public Vector2 position, scale, originInPixels;
        public Color color;
        public Texture2D texture;
        public SpriteEffects flip;
        public float rotation;
        public float layerDepth = 1f;

        public GameObject() : this(null) { }

        public GameObject(Texture2D texture) {
            this.texture = texture;
            color = Color.White;
            position = new Vector2();
            scale = new Vector2(1f, 1f);
            rotation = 0f;
            flip = SpriteEffects.None;
            originInPixels = new Vector2();
        }

        public virtual void SetPosition(float x, float y) {
            position.X = x;
            position.Y = y;
        }

        public virtual void SetScale(float scalar) {
            scale.X = scalar;
            scale.Y = scalar;
        }

        public virtual void SetScale(float x, float y) {
            scale.X = x;
            scale.Y = y;
        }

        /// <summary>
        /// Takes rotation in radians
        /// </summary>
        /// <param name="rotation"></param>
        public virtual void SetRotation(float rotation) {
            this.rotation = rotation;
        }

        public virtual void SetFlip(SpriteEffects flip) {
            this.flip = flip;
        }

        protected virtual void SetLayerDepth(float layerDepth) {
            this.layerDepth = layerDepth;
        }

        public virtual void SetOriginInPixels(float x, float y) {
            originInPixels.X = x;
            originInPixels.Y = y;
        }

        public virtual void Update(GameTime gameTime) {
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, color, rotation, originInPixels, scale, flip, layerDepth);
        }
    }
}
