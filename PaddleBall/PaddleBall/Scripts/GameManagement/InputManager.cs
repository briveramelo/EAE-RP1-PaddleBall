using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace PaddleBall {
    public class InputManager {

        private static InputManager instance;
        public static InputManager Instance {
            get {
                if (instance == null) {
                    instance = new InputManager();
                }
                return instance;
            }
        }
        public InputManager() {}

        void Initialize() {

        }

        public void Update(GameTime gameTime) {
            // Poll for current keyboard state
            KeyboardState state = Keyboard.GetState();

            // Print to debug console currently pressed keys
            //System.Text.StringBuilder sb = new StringBuilder();
            //foreach (var key in state.GetPressedKeys()) {
            //    sb.Append("Key: ").Append(key).Append(" pressed ");
            //}

            //if (sb.Length > 0) {
            //    System.Diagnostics.Debug.WriteLine(sb.ToString());
            //}
            //else {
            //    System.Diagnostics.Debug.WriteLine("No Keys pressed");
            //}

            //// Move our sprite based on arrow keys being pressed:
            //if (state.IsKeyDown(Keys.Right))
            //    position.X += 10;
            //if (state.IsKeyDown(Keys.Left))
            //    position.X -= 10;
            //if (state.IsKeyDown(Keys.Up))
            //    position.Y -= 10;
            //if (state.IsKeyDown(Keys.Down))
            //    position.Y += 10;

        }

    }
}
