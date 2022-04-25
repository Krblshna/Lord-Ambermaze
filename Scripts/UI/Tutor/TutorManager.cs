using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.UI.Tutor
{
    public class TutorManager : UUID
    {
        private bool active;
        [SerializeField] private Animator _anim;
        [SerializeField] private bool _autoStart;
        private ITutorCutsceneManager _tutorCutsceneManager;

        IEnumerator Start()
        {
            _tutorCutsceneManager = GetComponent<ITutorCutsceneManager>();
            yield return new WaitForSeconds(0.5f);
            if (_autoStart && !used_id())
            {
                Activate();
            }
        }

        private void Update()
        {
            if (CommonBlocker.IsBlocked(CommonBlocks.Menu)) return;
            if (ButtonWrap.GetButtonDown(Buttons.Tutor))
            {
                if (active)
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
            }
            if (active)
            {
                HandleInput();
            }
        }
        public void Deactivate()
        {
            _anim.SetBool("show", false);
            CommonBlocker.Unblock(CommonBlocks.Tutor);
            _tutorCutsceneManager.FinishTutorCutscene();
            active = false;
        }

        public void OnOpened()
        {
            _tutorCutsceneManager.OnOpened();
        }

        public void Activate()
        {
            save_id();
            _anim.SetBool("show", true);
            active = true;
            _tutorCutsceneManager.StartTutorCutscene();
            CommonBlocker.Block(CommonBlocks.Tutor);
        }
        private void HandleInput()
        {
            if (ButtonWrap.GetButtonDown(CommonButtons.Exit))
            {
                Deactivate();
            }
        }
    }
}