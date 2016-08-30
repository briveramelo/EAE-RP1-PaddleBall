using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall{

    
    public enum SoundFX {
        Launch=0,
        ReAttach=1
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

        Dictionary<SoundFX, SoundEffect> soundFX;
        ContentManager content;
        public void LoadContent(ContentManager Content) {
            content = Content;
            soundFX = new Dictionary<SoundFX, SoundEffect>() {
                {SoundFX.Launch, content.Load<SoundEffect>("SoundFX/Launch") },
                {SoundFX.ReAttach, content.Load<SoundEffect>("SoundFX/ReAttach") }
            };
            Song backgroundMusic = content.Load<Song>("SoundFX/SpectreLead_JForceEdit_01");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }

        public void PlaySound(SoundFX soundToPlay) {
            soundFX[soundToPlay].Play();
        }


    }
}
