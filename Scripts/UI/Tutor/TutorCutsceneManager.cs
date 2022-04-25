using System.Collections;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LordAmbermaze.UI.Tutor
{
    public enum ETutorAction { MoveLeft, MoveRight, MoveUp, MoveDown, Skip, PrepareSkill1, ActivateSkill}
    [System.Serializable]
    public class TutorStep
    {
        public ETutorAction TutorAction = ETutorAction.MoveLeft;
        public float WaitForNextAction = -1;
    }

    [System.Serializable]
    public class TutorPart
    {
        public GameObject labelObj;
        public int Idx;
        public TutorStep[] Steps;
        public float TimeBeforeReset = 0;
    }

    public class TutorCutsceneManager : MonoBehaviour, ITutorCutsceneManager
    {
        [SerializeField] private GameObject _tutorCamera;
        [SerializeField] private TutorPart[] _tutorParts;
        [SerializeField] private GameObject _nextText, _closeText;
        private TutorManager _tutorManager;
        private float _minStepTime = 0.75f,
            _curStepTime;
        private bool _active, _needChangeStep, _needRefresh, _controlable;
        private int _selectedTutorIdx;

        private int _curIndex { get; set; }

        private TutorStep[] CurSteps => _tutorParts[_selectedTutorIdx].Steps;
        private string TutorName(int tutorIdx)
        {
            var tutorNum = _tutorParts[tutorIdx].Idx;
            return $"TutorScene {tutorNum}";
        }

        private void RefreshLabel()
        {
            for (var i = 0; i < _tutorParts.Length; i++)
            {
                var tutorPart = _tutorParts[i];
                tutorPart.labelObj.SetActive(i == _selectedTutorIdx);
            }
        }

        public void Update()
        {
            if (!_active) return;
            CheckTime();
            HandleInput();
        }

        private void HandleInput()
        {
            if (!_controlable) return;
            if (ButtonWrap.IsAxisDown(Direction.Left))
            {
                MoveLeft();
            }
            else if (ButtonWrap.IsAxisDown(Direction.Right))
            {
                MoveRight();
            }

            if (ButtonWrap.GetButtonDown(CommonButtons.Use))
            {
                if (_selectedTutorIdx < _tutorParts.Length - 1)
                {
                    MoveRight();
                }
                else
                {
                    Deactivate();
                }
            }
        }

        private void Deactivate()
        {
            _controlable = false;
            _tutorManager.Deactivate();
        }

        private void MoveRight()
        {
            SelectItem(1);
        }

        private void MoveLeft()
        {
            SelectItem(-1);
        }

        private void SelectItem(int deltaX = 0)
        {
            var selectedTutor = _selectedTutorIdx + deltaX;
            selectedTutor = Mathf.Clamp(selectedTutor, 0, _tutorParts.Length - 1);
            if (_selectedTutorIdx != selectedTutor)
            {
                _active = false;
                StartCoroutine(UnloadTutor(() =>
                {
                    _selectedTutorIdx = selectedTutor;
                    RefreshVariables();
                    StartCoroutine(LoadTutor(() =>
                    {
                        
                        RefreshLabel();
                        Utils.SetTimeOut(() =>
                        {
                            _active = true;
                            RefreshVariables();
                            NextStep();
                        }, 0.5f);
                    }));
                }));
            }
        }

        private void CheckTime()
        {
            _curStepTime += Time.deltaTime;
            if (_curStepTime >= _minStepTime)
            {
                if (_needRefresh)
                {
                    _needRefresh = false;
                    Refresh();
                }
                if (_needChangeStep)
                {
                    _needChangeStep = false;
                    NextStep();
                }
            }
        }

        public void Start()
        {
            _tutorManager = GetComponent<TutorManager>();
            EventManager.StartListening(EventList.TurnFinished, OnTurnFinish);
            TutorConnector.Instance.InitCutsceneManager(this);
        }

        private void OnTurnFinish()
        {
            if (!_active) return;
            _curIndex++;
            if (_curIndex >= CurSteps.Length)
            {
                if (_curStepTime >= _minStepTime)
                {
                    Refresh();
                }
                else
                {
                    _needRefresh = true;
                }
            }
            else
            {
                if (_curStepTime >= _minStepTime)
                {
                    NextStep();
                }
                else
                {
                    _needChangeStep = true;
                }
            }
        }

        private void Refresh()
        {
            _curIndex = 0;
            StartCoroutine(RestartLevelProcess());
        }

        private IEnumerator RestartLevelProcess()
        {
            var waitTime = _tutorParts[_selectedTutorIdx].TimeBeforeReset;
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            _active = false;
            _tutorCamera.SetActive(false);
            AsyncOperation unloadingTutor = SceneManager.UnloadSceneAsync(TutorName(_selectedTutorIdx));
            while (!unloadingTutor.isDone)
            {
                yield return null;
            }
            AsyncOperation loadingTutor = SceneManager.LoadSceneAsync(TutorName(_selectedTutorIdx), LoadSceneMode.Additive);
            while (!loadingTutor.isDone)
            {
                yield return null;
            }
            _tutorCamera.SetActive(true);
            _active = true;
            NextStep();
        }

        private IEnumerator LoadTutor(Func onFinish = null)
        {
            AsyncOperation loadingTutor = SceneManager.LoadSceneAsync(TutorName(_selectedTutorIdx), LoadSceneMode.Additive);
            while (!loadingTutor.isDone)
            {
                yield return null;
            }
            onFinish?.Invoke();
        }

        private IEnumerator UnloadTutor(Func onFinish = null)
        {
            AsyncOperation unloadingTutor = SceneManager.UnloadSceneAsync(TutorName(_selectedTutorIdx));
            if (unloadingTutor != null)
            {
                while (!unloadingTutor.isDone)
                {
                    yield return null;
                }
            }

            onFinish?.Invoke();
        }

        private void RefreshVariables()
        {
            SetControls();
            _needRefresh = false;
            _needChangeStep = false;
            _curIndex = 0;
            _curStepTime = 0;
        }

        private void SetControls()
        {
            var nextActive = _selectedTutorIdx < _tutorParts.Length - 1;
            _nextText.SetActive(nextActive);
            _closeText.SetActive(!nextActive);
        }

        public void StartTutorCutscene()
        {
            _selectedTutorIdx = 0;
            RefreshVariables();
            StartCoroutine(LoadTutor(() =>
            {
                RefreshLabel();
                Utils.SetTimeOut(() =>
                {
                    _active = true;
                    RefreshLabel();
                    NextStep();
                }, 0.5f);
            }));
        }

        public void FinishTutorCutscene()
        {
            _active = false;
            _needRefresh = false;
            _curStepTime = 0;
            StartCoroutine(UnloadTutor());
        }

        public void OnOpened()
        {
            _controlable = true;
        }

        private void NextStep()
        {
            _curStepTime = 0;
            var tutorStep = CurSteps[_curIndex];
            TutorConnector.Instance.SetTutorAction(tutorStep.TutorAction);
            if (tutorStep.WaitForNextAction > 0)
            {
                Utils.SetTimeOut(OnTurnFinish, tutorStep.WaitForNextAction);
            }
        }
    }
}