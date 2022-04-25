using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.UI.Tutor
{
    public class TutorConnector : Singleton<TutorConnector>
    {
        private List<ITutorInput> _tutorInputList = new List<ITutorInput>();
        private ITutorCutsceneManager _tutorCutsceneManager;

        public void AddInput(ITutorInput tutorInput)
        {
            _tutorInputList.Add(tutorInput);
        }

        public void RemoveInput(ITutorInput tutorInput)
        {
            _tutorInputList.Remove(tutorInput);
        }

        public void InitCutsceneManager(ITutorCutsceneManager tutorCutsceneManager)
        {
            _tutorCutsceneManager = tutorCutsceneManager;
        }

        public void SetTutorAction(ETutorAction tutorAction)
        {
            foreach (var tutorInput in _tutorInputList)
            {
                tutorInput.SetAction(tutorAction);
            }
        }
    }
}