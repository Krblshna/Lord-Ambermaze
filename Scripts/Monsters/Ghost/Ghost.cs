using System;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
    public class Ghost : MonoBehaviour
    {
        private SpriteRenderer _render;
        private void Awake()
        {
            _render = GetComponent<SpriteRenderer>();
        }

        public void ChangeMaterial(Material material)
        {
            _render.material = material;
        }
    }
}