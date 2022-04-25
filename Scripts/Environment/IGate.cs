using LordAmbermaze.Interactions;

namespace LordAmbermaze.Environment
{
    public interface IGate
    {
        void GateOpenChange(bool active);
        void Init();
        void SetLock(DoorLock doorLock);
        void OnLockOpen();
    }
}