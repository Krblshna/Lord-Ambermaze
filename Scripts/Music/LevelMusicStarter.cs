using System.Linq;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Music
{
    [System.Serializable]
    public class LevelMusicData
    {
        public int BiomId;
        public int LevelId;
        public AudioClip SceneMusic;
        public float Volume = 0.2f;
    }

    public class LevelMusicStarter : Singleton<LevelMusicStarter>
    {
        public AudioClip defaultMusic;
        [SerializeField] private LevelMusicData[] data;

        public void TryStartMusic(int biomId, int levelId)
        {
            var mData = data.First(musicData => musicData.BiomId == biomId && musicData.LevelId == levelId);
            if (mData == null)
            {
                LevelMusicManager.Instance.start_music(defaultMusic, 0.2f, 0.5f);
                return;
            }
            
            LevelMusicManager.Instance.start_music(mData.SceneMusic, mData.Volume, 0.5f);
        }
    }
}