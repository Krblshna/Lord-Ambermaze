using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using AZ.Core.Depot;
using AZ.Menu;
using UnityEngine;

namespace LordAmbermaze.UI.MainMenu
{
    public class MenuController : MonoBehaviour, IMenuController
    {
        [SerializeField] GameObject continueObj;
        private List<IMenuItem> _items;
        private IMenuItem _curItem;
        private int _selectedId;

        public void Init()
        {
            if (!Storage.HaveAutosave())
            {
                continueObj.SetActive(false);
            }
            _items = GetComponentsInChildren<IMenuItem>().ToList();
            SelectId();
        }

        public void CustomUpdate()
        {
            HandleInput();
            HandleInputItem();
        }

        private void HandleInput()
        {
            if (ButtonWrap.IsAxisDown(Direction.Down))
            {
                MoveDown();
            }
            else if (ButtonWrap.IsAxisDown(Direction.Up))
            {
                MoveUp();
            }

            if (ButtonWrap.GetButtonDown(CommonButtons.Use) || ButtonWrap.GetButtonDown(CommonButtons.Jump))
            {
                SoundManager.PlaySound(SoundType.button_press);
                InvokeFunc();
            }
        }

        private void HandleInputItem()
        {
            if (_curItem == null) return;
            _curItem.HandleInput();
        }

        private void InvokeFunc()
        {
            _items[_selectedId].Activate();
        }

        private void SelectId()
        {
            for (var idx = 0; idx < _items.Count; idx++)
            {
                var item = _items[idx];
                if (idx == _selectedId)
                {
                    item.OnSelected();
                    _curItem = item;
                }
                else
                {
                    item.OnDeselected();
                }
            }
        }

        private void SelectItem(int delta)
        {
            SoundManager.PlaySound(SoundType.button_click);
            var newId = _selectedId + delta;
            newId = Mathf.Clamp(newId, 0, _items.Count - 1);
            if (newId != _selectedId)
            {
                _selectedId = newId;
                SelectId();
            }
        }

        private void MoveDown()
        {
            SelectItem(1);
        }

        private void MoveUp()
        {
            SelectItem(-1);
        }
    }
}