using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall {
    public class TextInputManager {

        Coroutiner myCoroutiner;
        ContentManager content;
        SpriteFont spriteFont;
        Vector2 namePosition, newHighScorePosition;
        Vector2 scale;
        private static TextInputManager instance;
        public static TextInputManager Instance {
            get {
                if (instance == null) {
                    instance = new TextInputManager();
                }
                return instance;
            }
        }
        Keys[] lastPressedKeys;
        string name;
        int maxCharacters;
        int currentNameLength;
        Keys[] validLetters;
        Vector2[] underScorePositions;
        Vector2 timerPosition;

        public bool IsTyping() {
            return isTyping;
        }
        bool isTyping;
        bool underScoreIsShowing;
        private TextInputManager() {
            myCoroutiner = new Coroutiner();
            isTyping = false;
            underScoreIsShowing = false;

            newHighScorePosition = new Vector2(500, 250);
            namePosition = new Vector2(850, 400);
            float spacing = 70;
            underScorePositions = new Vector2[] {
                namePosition + new Vector2(5, 0),
                namePosition + new Vector2(5 + spacing*1, 0),
                namePosition + new Vector2(5 + spacing*2, 0),
            };
            timerPosition = new Vector2(900,800);
            maxCharacters = 3;
            scale = Vector2.One * 1f;
            //scale = Vector2.One * (6f / 10f);
            validLetters = new Keys[] {
                Keys.A,
                Keys.B,
                Keys.C,
                Keys.D,
                Keys.E,
                Keys.F,
                Keys.G,
                Keys.H,
                Keys.I,
                Keys.J,
                Keys.K,
                Keys.L,
                Keys.M,
                Keys.N,
                Keys.O,
                Keys.P,
                Keys.Q,
                Keys.R,
                Keys.S,
                Keys.T,
                Keys.U,
                Keys.V,
                Keys.W,
                Keys.X,
                Keys.Y,
                Keys.Z,
            };
        }

        public void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("DS-DIGIT");
        }

        public void PostLoad() {
            isTyping = true;
            underScoreIsShowing = true;
            timeToEnterName = 20;
            timeLeftToEnterName = timeToEnterName;
            name = "";
            currentNameLength = 0;
            lastPressedKeys = new Keys[] { };
            myCoroutiner.StopAllCoroutines();
            myCoroutiner.StartCoroutine(Flicker());
        }
        IEnumerator Flicker() {
            int framesOn = 60;
            int framesOff = 50;
            underScoreIsShowing = true;
            for (int i = 0; i < framesOn; i++) {
                yield return null;
            }
            underScoreIsShowing = false;
            for (int i = 0; i < framesOff; i++) {
                yield return null;
            }
            myCoroutiner.StartCoroutine(Flicker());
            myCoroutiner.StartCoroutine(CountDown());
        }

        int timeLeftToEnterName;
        int timeToEnterName;
        IEnumerator CountDown() {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float timeToPause = timeToEnterName;
            timeLeftToEnterName = timeToEnterName;
            while (stopwatch.ElapsedMilliseconds < timeToPause * 1000) {
                timeLeftToEnterName = (int)Math.Floor((timeToEnterName - ((float)stopwatch.ElapsedMilliseconds / 1000f)));
                yield return null;
            }
            isTyping = false;
        }

        public void Update() {
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys) {
                if (!lastPressedKeys.Contains(key)) {
                    OnKeyDown(key);
                }
            }

            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
            myCoroutiner.Update();
        }

        private void OnKeyDown(Keys key) {
            if (validLetters.Contains(key)) {
                if (name.Length < maxCharacters) {
                    name += key.ToString();
                    currentNameLength++;
                    if (currentNameLength == maxCharacters) {
                        currentNameLength = maxCharacters-1;
                    }
                }
            }
            else if (key == Keys.Back) {
                if (name.Length > 0) {
                    name = name.Remove(name.Length - 1);
                    currentNameLength--;
                    if (currentNameLength <0) {
                        currentNameLength = 0;
                    }
                }
            }
            else if (key == Keys.Enter) {
                if (name.Length == maxCharacters) {
                    SubmitName();
                }
            }
        }

        void SubmitName() {
            ScoreDisplay newScoreDisplay = new ScoreDisplay(new Score(11, ScoreBoard.Instance.GetScore(), name, true));
            if (HighScoreDisplay.Instance.IsNewHighScore(newScoreDisplay.myScore.score)) {
                HighScoreDisplay.Instance.AddHighScore(newScoreDisplay);
            }
            isTyping = false;
            myCoroutiner.StopAllCoroutines();
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(spriteFont, "NEW HIGH SCORE !", newHighScorePosition, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, name, namePosition, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            if (underScoreIsShowing) {
                spriteBatch.DrawString(spriteFont, "_", underScorePositions[currentNameLength], Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.DrawString(spriteFont, timeLeftToEnterName.ToString(), timerPosition, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

    }

}
