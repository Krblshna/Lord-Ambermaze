using UnityEngine;

namespace LordAmbermaze.TurnableObjects.Spikes
{
    public class SpikesAnimGraphic : MonoBehaviour, ISpikesGraphics
    {
        private Animator _anim;

        public void Init()
        {
            _anim = GetComponent<Animator>();
        }

        private void SetTrigger(string animName)
        {
            _anim.SetTrigger(animName);
        }

        public void Prepare()
        {
            SetTrigger("prepare");
        }

        public void Attack()
        {
            SetTrigger("attack");
        }

        public void Wait()
        {
            SetTrigger("wait");
        }
    }
}