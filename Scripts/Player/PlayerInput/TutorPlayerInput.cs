using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.UI.Tutor;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public class TutorPlayerInput : PlayerInput, ITutorInput
    {
        private bool _wasSet, onCyclePass;
		public override void HandleInput(){}

        void OnEnable()
        {
            TutorConnector.Instance.AddInput(this);
        }

        void OnDisable()
        {
            TutorConnector.Instance.RemoveInput(this);
        }

        private void ClearInput()
        {
            Right = false;
            Left = false;
            Up = false;
            Down = false;
            Skip = false;
            UsedSkill = false;
            Skill1 = false;
            ActivateSkill = false;
        }

        void Update()
        {
            if (onCyclePass)
            {
                onCyclePass = false;
                ClearInput();
            }
            if (_wasSet)
            {
                _wasSet = false;
                onCyclePass = true;
            }
        }

        public void SetAction(ETutorAction tutorAction)
        {
            ClearInput();
            _wasSet = true;
            switch (tutorAction)
            {
                case ETutorAction.MoveRight:
                {
                    Right = true;
                    break;
                }
                case ETutorAction.MoveLeft:
                {
                    Left = true;
                    break;
                }
                case ETutorAction.MoveUp:
                {
                    Up = true;
                    break;
                }
                case ETutorAction.MoveDown:
                {
                    Down = true;
                    break;
                }
                case ETutorAction.Skip:
                {
                    Skip = true;
                    break;
                }
                case ETutorAction.PrepareSkill1:
                {
                    UsedSkill = true;
                    Skill1 = true;
                    break;
                }
                case ETutorAction.ActivateSkill:
                {
                    ActivateSkill = true;
                    break;
                }
            }
        }
    }
}