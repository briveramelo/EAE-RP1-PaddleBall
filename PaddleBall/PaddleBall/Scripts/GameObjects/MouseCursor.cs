using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PaddleBall {

    /// <summary>
    /// Just to ensure there is a display of a mouse in the title and scoreboard screens
    /// Looks a little funky though
    /// </summary>
    public class MouseCursor : GameObject {

        public override void LoadContent(ContentManager Content) {
            texturePath = "Images/arrow_cursor";
            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime) {
            MouseState mouseState = Mouse.GetState();
            position = mouseState.Position.ToVector2();
            base.Update(gameTime);
        }
    }
}
