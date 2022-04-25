using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
    public class TeleportConnector : Singleton<TeleportConnector>
    {
        private ITeleportAnimation _anim;
        public void RegisterAnimation(ITeleportAnimation anim)
        {
            _anim = anim;
        }

        public void PlayTeleportDepature(Func callback)
        {
            _anim.TeleportDepature(callback);
        }

        public void PlayTeleportArrival(Func callback)
        {
            _anim.TeleportArrival(callback);
        }
    }
}