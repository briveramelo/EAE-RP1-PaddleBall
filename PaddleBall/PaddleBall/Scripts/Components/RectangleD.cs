using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall{
    /// <summary>
    /// Creates a Rectangle with the 2 opposing diagonal points for definition
    /// v1 must be top left and v2 must be bottom right
    /// this is NOT enforced, so improper entry can cause errors depending on implementation
    /// </summary>
    public struct RectangleD {
        public Vector2 topLeft, bottomRight;

        public RectangleD(Vector2 topLeft, Vector2 bottomRight) {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }

        public bool IsPointWithin(Vector2 point) {
            if (point.X > topLeft.X && point.X < bottomRight.X) {
                if (point.Y > topLeft.Y && point.Y < bottomRight.Y) {
                    return true;
                }
            }
            return false;
        }
    }
}
