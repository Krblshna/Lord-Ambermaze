using UnityEngine;

namespace LordAmbermaze.Factory
{
    public class SimpleFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        public void CreateObject()
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
        }
    }
}