using System;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
    public class MTP_T_Bee : MoveToPlayerOnTrigger
    {
        public override Type GetAgroType()
        {
            return typeof(AgroBeeBehaviour);
        }
    }
}