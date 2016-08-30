using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall{

    
    public enum SoundFX {
        Launch=0,
        ReAttach=1,
        Hit=2,
        Explode=3
    }
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

        public void PlaySound(SoundFX soundToPlay) {
            Debug.WriteLine(soundToPlay + " at index: " + soundFXCount[soundToPlay]);
            int soundFXIndex = soundFXCount[soundToPlay];
            soundFX[soundToPlay].ElementAt<SoundEffect>(soundFXIndex).Play();
            soundFXCount[soundToPlay]++;
            if (soundFXCount[soundToPlay] >= soundFX[soundToPlay].Count) {
                soundFXCount[soundToPlay] = 0;
            }
        }
        Dictionary<SoundFX, int> soundFXCount = new Dictionary<SoundFX, int>() {
            { SoundFX.Launch, 0},
            { SoundFX.ReAttach, 0},
            { SoundFX.Explode, 0}
        };


    }
}
