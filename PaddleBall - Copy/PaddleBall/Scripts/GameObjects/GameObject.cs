using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace PaddleBall {

    public class GameObject {

        public static List<GameObject> allGameObjects = new List<GameObject>();

        public Vector2 position;
        public Vector2 scale = new Vector2(1, 1);
        public Vector2 originInPixels;
        public virtual Vector2 forward {
            get {
                return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            }
        }
        public Color color = Color.White;
        public Texture2D texture;
        protected ContentManager content;
        protected Vector2 screenCenter = new Vector2(ScreenManager.Instance.Dimensions.X, ScreenManager.Instance.Dimensions.Y) / 2f;
        public SpriteEffects flip = SpriteEffects.None;
        public float rotation =0f;
        public float layerDepth = 1f;
        public string texturePath;

        #region Game Loop Functions
        //1
        public virtual void LoadContent(ContentManager Content) {
            content = Content;
            texture = content.Load<Texture2D>(texturePath);           
        }
        //2
        public virtual void PostLoad() {
            SetOriginInPixels(texture.Width / 2, texture.Height / 2);
        }
        //3
        public virtual void Update(GameTime gameTime) {
        }
        //4
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, color, rotation, originInPixels, scale, flip, layerDepth);
        }
        //5
        public virtual void UnloadContent() {
            //content.Unload();
        }
        #endregion


        ////////////////////////
        #region GameObject Standard Functions
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

        public virtual void Destroy() {
            UnloadContent();
            allGameObjects.Remove(this);
        }
        #endregion
    }
}
