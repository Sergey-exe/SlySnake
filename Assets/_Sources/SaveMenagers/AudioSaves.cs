using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        [Header("Audio Settings")] 
        public float MasterVolume = 1f;
        public bool MasterMute = false;

        public float MusicVolume = 0.5f;
        public bool MusicMute = false;

        public float SoundsVolume = 0.5f;
        public bool SoundsMute = false;

        public float UIVolume = 0.5f;
        public bool UIVomute = false;
    }
}