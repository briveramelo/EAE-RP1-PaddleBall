using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    public class Cannon {

        bool isClockWise = true;
        Texture2D image;
        ContentManager content;

        public void LoadContent() {
            string path = "Images/Cannon";
            image = content.Load<Texture2D>(path);
        }

        public void SwitchDirection() {
            isClockWise = !isClockWise;
        }

        public void Update() {
            //rotate clockwise by default
        }
    }
}
