using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
    public class SpikeBallBehaviour : MoveAttackBehaviour
    {
        protected override void Move()
        {
            var direction = MoveVector.Direction();
            var animType = AnimTypeUtils.GetMoveAnimType(direction);
            _animManager.Play(animType);
            _characterEngine.Move();
        }

        protected override void Attack(Func onAttack = null)
        {
            Move();
        }
    }
}