using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {

    public enum Layer {
        Cannon=0,
        CannonBall=1,
        Enemy=2,
        Shield=3,
        Obstacle=4
    }

    public class CircleCollider {
        public static Dictionary<Layer, List<CircleCollider>> allColliders = new Dictionary<Layer, List<CircleCollider>>() {
            {Layer.Cannon, new List<CircleCollider>()},
            {Layer.CannonBall, new List<CircleCollider>()},
            {Layer.Enemy, new List<CircleCollider>()},
            {Layer.Shield, new List<CircleCollider>()},
            {Layer.Obstacle, new List<CircleCollider>()}
        };
        public static bool[][] layersWillCollide = new bool[][] {
            new bool[] { false, false, true, false, false},  //Cannon
            new bool[] { false, false, true, false, true},  //CannonBall
            new bool[] { true, true, false, true, false},  //Enemy
            new bool[] { false, false, true, false, false},  //Shield
            new bool[] { false, true, true, false, false}  //Obstacle  
        }; 
        public Layer layer;
        public GameObject gameObject;
        public Vector2 offset = Vector2.Zero;
        public float radius=0f;
        /// <summary>
        /// Points from this collider's position, to the other colliders position
        /// </summary>
        public Vector2 normal = Vector2.Zero;
        public Vector2 position {
            get {
                return gameObject.position + offset;
            }
        }
        public CircleCollider(Layer layer, GameObject gameObject, float radius) {
            this.layer = layer;
            this.gameObject = gameObject;
            this.radius = Math.Abs(radius);
            allColliders[layer].Add(this);
        }
        public CircleCollider(Layer layer, GameObject gameObject, float radius, Vector2 offset) {
            this.layer = layer;
            this.gameObject = gameObject;
            this.radius = Math.Abs(radius);
            this.offset = offset;
            allColliders[layer].Add(this);
        }
        public void Destroy() {
            allColliders[layer].Remove(this);
        }

        public virtual CircleCollider GetOverlappingCollider() {
            foreach (KeyValuePair <Layer, List<CircleCollider>> dictItem in allColliders) {
                if (layersWillCollide[(int)dictItem.Key][(int)layer]) {
                    foreach (CircleCollider col in dictItem.Value) {
                        if (Vector2.Distance(col.position, position)<(col.radius + radius)) {
                            normal = Vector2.Normalize(position - col.position);
                            return col;
                        }
                    }
                }
            }
            return null;
        }

    }
}
