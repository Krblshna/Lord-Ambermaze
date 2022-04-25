using AZ.Core;
using LordAmbermaze.UI.Tutor;

namespace LordAmbermaze.Player
{
    public interface ITutorInput
    {
        void SetAction(ETutorAction tutorAction);
    }
}