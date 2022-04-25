using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Level;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
    public class Monster_UUID : UUID
    {
        [SerializeField] private bool _tutorMonster;
        private Vector2Int initPos;
        private void OnEnable()
        {
            initPos = transform.localPosition.ToVector2Int();
        }
        public override string ID
        {
            get
            {
                var levelManager = LevelManager.Instance;
                if (levelManager == null)
                {
                    return m_UUID + initPos;
                }
                return m_UUID;
            }
            set { m_UUID = value; }
        }

        public void SaveKill()
        {
            if (_tutorMonster) return;
            save_id();
        }

        public bool WasKilled()
        {
            return used_id();
        }
    }
}