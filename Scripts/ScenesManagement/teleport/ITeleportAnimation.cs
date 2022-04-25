using AZ.Core;

namespace LordAmbermaze.ScenesManagement
{
    public interface ITeleportAnimation
    {
        void TeleportDepature(Func callback);
        void TeleportArrival(Func callback);
    }
}