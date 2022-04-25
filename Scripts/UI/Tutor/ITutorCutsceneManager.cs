namespace LordAmbermaze.UI.Tutor
{
    public interface ITutorCutsceneManager
    {
        void StartTutorCutscene();
        void FinishTutorCutscene();
        void OnOpened();
    }
}