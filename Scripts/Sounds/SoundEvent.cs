using UnityEngine;

namespace LordAmbermaze.Sounds
{
    [System.Serializable]
    public class SoundEventEl
    {
        public string name;
        public SoundType soundType;
    }

    public class SoundEvent : MonoBehaviour
    {
        [SerializeField] private SoundEventEl[] _sounds;

        public void PlaySound(string name)
        {
            foreach (var sound in _sounds)
            {
                if (sound.name == name)
                {
                    SoundManager.PlaySound(sound.soundType);
                }
            }
        }
    }
}