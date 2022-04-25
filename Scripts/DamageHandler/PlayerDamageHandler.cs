using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.DamageHandler
{
	public class PlayerDamageHandler : AnimColorDamageHandler
    {
        [SerializeField] private ParticleSystem _particle;
        public override void GetHit()
        {
            base.GetHit();
            if (_particle != null)
            {
                _particle.Play();
            }
        }
	}
}