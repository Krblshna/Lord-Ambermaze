using UnityEngine;

namespace LordAmbermaze.Consumables
{
    public class ConsumableAppear : MonoBehaviour, IConsumableAppear
    {
        public void Appear(Vector2 direction)
        {
            var animator = GetComponent<Animator>();
            animator.SetInteger("x", (int)direction.x);
            animator.SetInteger("y", (int)direction.y);
            animator.SetTrigger("show");
        }
    }
}