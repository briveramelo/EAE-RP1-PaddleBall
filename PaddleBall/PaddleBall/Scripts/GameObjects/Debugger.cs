﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PaddleBall {

    /// <summary>
    /// Included for demo purposes to increase excitement in the short time available.
    /// Fun easter egg
    /// </summary>
    public class Debugger {
        private static Debugger instance;
        public static Debugger Instance {
            get {
                if (instance == null) {
                    instance = new Debugger();
                    hasEnteredIntenseRound = false;
                    myCoroutiner = new Coroutiner();
                }
                return instance;
            }
            set { instance = value; }
        }
        static Coroutiner myCoroutiner;
        static bool hasEnteredIntenseRound;
        KeyboardState lastKeyboardState;
        public void Update() {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A) &&
                keyboardState.IsKeyDown(Keys.S) &&
                keyboardState.IsKeyDown(Keys.D) &&
                keyboardState.IsKeyDown(Keys.F) &&
                keyboardState.IsKeyDown(Keys.Enter) &&
                lastKeyboardState != keyboardState) {

                if (!hasEnteredIntenseRound) {
                    hasEnteredIntenseRound = true;
                    myCoroutiner.StartCoroutine(DisplayEnterDebugMode());
                }
            }

            myCoroutiner.Update();
        }

        bool displayDebugMode;
        IEnumerator DisplayEnterDebugMode() {
            displayDebugMode = true;
            Ship.Instance.ActivateMegaLaser();
            AudioManager.Instance.PlaySound(SoundFX.MegaLaser);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            float timeToDisplay = 4f;

            while (stopwatch.ElapsedMilliseconds < 0.6f * 1000) {
                yield return null;
            }
            displayDebugMode =false;
            while (stopwatch.ElapsedMilliseconds < 1.0f * 1000) {
                yield return null;
            }
            EnemySpawner.Instance.DEBUG_ENTER_INTENSE_ROUND(7);
            displayDebugMode = true;
            while (stopwatch.ElapsedMilliseconds < 1.6f * 1000) {
                yield return null;
            }
            displayDebugMode = false;
            while (stopwatch.ElapsedMilliseconds < 2f * 1000) {
                yield return null;
            }
            displayDebugMode = true;
            while (stopwatch.ElapsedMilliseconds < timeToDisplay * 1000) {
                yield return null;
            }
            stopwatch.Stop();
            displayDebugMode = false;
        }

        public void LoadContent(ContentManager Content) {
            content = Content;
            spriteFont = content.Load<SpriteFont>("DS-DIGIT");
            position = new Vector2(225,800);
            scale = Vector2.One * 1f;
        }
        SpriteFont spriteFont;
        ContentManager content;
        Vector2 position;
        Vector2 scale;
        public void Draw(SpriteBatch spriteBatch) {
            if (displayDebugMode) {
                spriteBatch.DrawString(spriteFont, "INTENSITY ACTIVATED !", position, Color.Red, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
