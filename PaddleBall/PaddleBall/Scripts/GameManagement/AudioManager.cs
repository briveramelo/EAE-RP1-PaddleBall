using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace PaddleBall{

    
    public enum SoundFX {
        Launch=0,
        ReAttach=1,
        Hit=2,
        Explode=3
    }
    //Handles all sound effects and background music
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
        /// the soundFX dictionary provides access to lists of SoundEffects by type of SoundFX for simple shuffling
        /// </summary>
        Dictionary<SoundFX, List<SoundEffect>> soundFX;
        ContentManager content;
        public void LoadContent(ContentManager Content) {
            content = Content;
            soundFX = new Dictionary<SoundFX, List<SoundEffect>>() {
                {SoundFX.Launch, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Shoots/Shoot_00"),
                    content.Load<SoundEffect>("SoundFX/Shoots/Shoot_01"),
                    content.Load<SoundEffect>("SoundFX/Shoots/Shoot_02"),
                    content.Load<SoundEffect>("SoundFX/Shoots/Shoot_03")
                } },
                {SoundFX.ReAttach, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_00"),
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_01"),
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_02"),
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_03")
                } },
                {SoundFX.Hit, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_00"),
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_01"),
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_02"),
                    content.Load<SoundEffect>("SoundFX/Hits/Hit_03")
                } },
                {SoundFX.Explode, new List<SoundEffect>() {
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion_00"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion_01"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion_02"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion_03"),
                    content.Load<SoundEffect>("SoundFX/Explosions/Explosion_04")
                } }
            };
            List<Song> backgroundMusics = new List<Song>() {
                content.Load<Song>("SoundFX/Tunes/LevelMusic_1"),
                content.Load<Song>("SoundFX/Tunes/LevelMusic_2"),
                content.Load<Song>("SoundFX/Tunes/LevelMusic_3"),
            };

            MediaPlayer.Play(backgroundMusics[0]);
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
        /// <summary>
        /// soundFXCount keeps track of the current index of the soundFX list that the PlaySound cycles through
        /// </summary>
        Dictionary<SoundFX, int> soundFXCount = new Dictionary<SoundFX, int>() {
            { SoundFX.Launch, 0},
            { SoundFX.ReAttach, 0},
            { SoundFX.Hit, 0},
            { SoundFX.Explode, 0}
        };


    }
}
