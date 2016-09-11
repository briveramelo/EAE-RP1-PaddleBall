using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PaddleBall{

    
    public enum SoundFX {
        Launch=0,
        Hit=1,
        DestroyEnemy=2,
        PaddleDeath=3,
        PlayerLoss=4,
        ShieldHit=5,
        MegaLaser=6
    }
    /// <summary>
    /// Handles all sound effects and background music
    /// </summary>
    public class AudioManager {

        private static AudioManager instance;
        public static AudioManager Instance {
            get {
                if (instance == null) {
                    instance = new AudioManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// soundFXCount keeps track of the current index of the soundFX list that the PlaySound cycles through
        /// </summary>
        static Dictionary<SoundFX, int> soundFXCount = new Dictionary<SoundFX, int>() {
            { SoundFX.Launch, 0},
            { SoundFX.Hit, 0},
            { SoundFX.DestroyEnemy, 0},
            { SoundFX.PaddleDeath, 0},
            { SoundFX.PlayerLoss, 0},
            { SoundFX.ShieldHit, 0},
            { SoundFX.MegaLaser, 0}
        };

        /// <summary>
        /// the soundFX dictionary provides access to lists of SoundEffects by type of SoundFX for simple shuffling
        /// </summary>
        Dictionary<SoundFX, List<SoundEffect>> soundFX;
        Song title, game;
        ContentManager content;
        public void LoadContent(ContentManager Content) {
            content = Content;
            soundFX = new Dictionary<SoundFX, List<SoundEffect>>() {
                {SoundFX.Launch, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Launches/Launch1"),
                    content.Load<SoundEffect>("SoundFX/Launches/Launch2"),
                    content.Load<SoundEffect>("SoundFX/Launches/Launch3"),
                    content.Load<SoundEffect>("SoundFX/Launches/Launch4")
                } },
                {SoundFX.Hit, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Bounces/Bounce1"),
                    content.Load<SoundEffect>("SoundFX/Bounces/Bounce2"),
                    content.Load<SoundEffect>("SoundFX/Bounces/Bounce3"),
                    content.Load<SoundEffect>("SoundFX/Bounces/Bounce4")
                } },
                {SoundFX.DestroyEnemy, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/DestroyEnemy/Destroyenemy1"),
                    content.Load<SoundEffect>("SoundFX/DestroyEnemy/Destroyenemy3"),
                    content.Load<SoundEffect>("SoundFX/DestroyEnemy/Destroyenemy4"),
                    content.Load<SoundEffect>("SoundFX/DestroyEnemy/Destroyenemy5"),
                } },
                {SoundFX.PaddleDeath, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion1"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion2"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion3"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion4"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion5"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion6"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion7"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion8"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion9")
                } },
                {SoundFX.PlayerLoss, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Playerloss")
                } },
                {SoundFX.MegaLaser, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/MegaLasers/MegaLaser1"),
                    content.Load<SoundEffect>("SoundFX/MegaLasers/MegaLaser2"),
                    content.Load<SoundEffect>("SoundFX/MegaLasers/MegaLaser3"),
                    content.Load<SoundEffect>("SoundFX/MegaLasers/MegaLaser4"),
                    content.Load<SoundEffect>("SoundFX/MegaLasers/MegaLaser5"),
                    content.Load<SoundEffect>("SoundFX/MegaLasers/MegaLaser6")
                } },
                {SoundFX.ShieldHit, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Shieldhit")
                } }
            };
            title = content.Load<Song>("SoundFX/Tunes/Title");
            game = content.Load<Song>("SoundFX/Tunes/Game");
            PlayBackgroundMusic(ScreenManager.Instance.GetCurrentScreen());
        }


        public void StopMusic() {
            MediaPlayer.Stop();
        }

        public void PlayBackgroundMusic(Screen screen) {
            switch (screen) {
                case Screen.Title:
                    MediaPlayer.Play(title);
                    break;
                case Screen.Scores:
                    MediaPlayer.Play(title);
                    break;
                case Screen.Game:
                    MediaPlayer.Play(game);
                    break;
            }
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// Plays the next in line of the soundFX of your choice
        /// increments the soundFX in the given list for predictable variety
        /// </summary>
        /// <param name="soundToPlay"></param>
        public void PlaySound(SoundFX soundToPlay) {
            int soundFXIndex = soundFXCount[soundToPlay];
            soundFX[soundToPlay].ElementAt<SoundEffect>(soundFXIndex).Play();
            soundFXCount[soundToPlay]++;
            if (soundFXCount[soundToPlay] >= soundFX[soundToPlay].Count) {
                soundFXCount[soundToPlay] = 0;
            }
        }
        


    }
}
