using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Collections;

namespace PaddleBall {
    public static class Globalvars
    {
        public static Texture2D test;
        public static Rectangle testrec = new Rectangle(0, 0, 100, 100);
        public static Vector2 testloc = new Vector2(235, 235);
    }
    public class GameObject {
        public int Width
        {
            get { return texture.Width; }
        }
        public int Height
        {
            get { return texture.Height; }
        }
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, Width, Height);

            }

        }
        
        
        private readonly List<IEnumerator> coroutines = new List<IEnumerator>();
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
        //Calling order
        //1
        public virtual void LoadContent(ContentManager Content) {
            content = Content;
            texture = content.Load<Texture2D>(texturePath);
            Globalvars.test = content.Load<Texture2D>("Images/testimg");
            
        }
        //2
        public virtual void PostLoad() {
            SetOriginInPixels(texture.Width / 2, texture.Height / 2);
        }
        //3
        public virtual void Update(GameTime gameTime) {
            for (int i = coroutines.Count - 1; i >= 0; i--) {
                if (!coroutines[i].MoveNext()) {
                    coroutines.RemoveAt(i);
                }
            }
        }
        //4
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, color, rotation, originInPixels, scale, flip, layerDepth);
            spriteBatch.Draw(Globalvars.test, Globalvars.testloc, Globalvars.testrec, Color.White);

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

        protected void StartCoroutine(IEnumerator coroutine) {
            coroutines.Add(coroutine);
        }
        #endregion
    }
}
