using UnityEngine;

namespace LordAmbermaze.Core
{
    public class RandSmallShift : MonoBehaviour
    {
        void Start()
        {
            var pos = transform.position;
            var posY = pos.y + Random.Range(0, 1000) / Mathf.Pow(10f, 6f);
            transform.position = new Vector3(pos.x, posY);
        }
    }
}