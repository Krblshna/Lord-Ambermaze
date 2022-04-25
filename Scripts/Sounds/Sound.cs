using UnityEngine;

namespace LordAmbermaze.Sounds
{
    public class Sound : MonoBehaviour, ISound
    {
        [SerializeField] private SoundType soundType;
        public void Play()
        {
            SoundManager.PlaySound(soundType);
        }
    }
}