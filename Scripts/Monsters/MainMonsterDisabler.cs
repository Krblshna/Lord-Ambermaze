using System.Collections;
using System.Collections.Generic;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
    public class MainMonsterDisabler : MonoBehaviour
    {
        [SerializeField] private Monster_UUID _monsterUuid;
        public bool used_id()
        {
            var condition = new Condition(StorageType.Counters, _monsterUuid.ID, 1);
            return condition.Test();
        }
    }
}
