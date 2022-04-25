using System;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.CellHighlight
{
    public class CellHighlight : MonoBehaviour, ICellHighlight
    {
        [SerializeField]
        private SpriteRenderer[] _renderers;
        private CellHighlightManager _manager;
        private Color _initColor;

        public void SetPos(Vector2 pos)
        {
            transform.position = pos;
        }

        public void SetActivate(bool active)
        {
            gameObject.SetActive(active);
        }

        public void ColorTo(Color initColor, Color color, float time)
        {
            _initColor = initColor;
            foreach (var renderer in _renderers)
            {
                renderer.material.SetColor("_Color", initColor);
                iTween.ColorTo(renderer.gameObject, iTween.Hash(
                    "color", color,
                    "time", time,
                    "easetype", iTween.EaseType.linear
                ));
                iTween.ColorTo(renderer.gameObject, iTween.Hash(
                    "color", initColor,
                    "time", time,
                    "delay", time,
                    "easetype", iTween.EaseType.linear
                ));
            }
            Utils.SetTimeOut(OnFinish, time * 2 + 0.1f);
        }

        public void BlinkColorTo(Color initColor, Color color, float time)
        {
            _initColor = initColor;
            foreach (var renderer in _renderers)
            {
                renderer.material.SetColor("_Color", initColor);
                iTween.ColorTo(renderer.gameObject, iTween.Hash(
                    "color", color,
                    "time", time,
                    "easetype", iTween.EaseType.linear,
                    "looptype", iTween.LoopType.pingPong
                ));
            }
        }

        public void SetColor(Color color)
        {
            _initColor = color;
            foreach (var renderer in _renderers)
            {
                renderer.material.SetColor("_Color", color);
            }
        }

        public void RemoveColor()
        {
            Color color = new Color(_initColor.r, _initColor.g, _initColor.b, 0);
            foreach (var renderer in _renderers)
            {
                renderer.material.SetColor("_Color", color);
            }

            OnFinish();
        }

        public void PermanentColorTo(Color initColor, Color color, float time)
        {
            _initColor = initColor;
            foreach (var renderer in _renderers)
            {
                renderer.material.SetColor("_Color", initColor);
                iTween.ColorTo(renderer.gameObject, iTween.Hash(
                    "color", color,
                    "time", time,
                    "easetype", iTween.EaseType.linear
                ));
            }
        }

        public void RemoveColor(float time)
        {
            Color color = new Color(_initColor.r, _initColor.g, _initColor.b, 0);
            foreach (var renderer in _renderers)
            {
                iTween.Stop(renderer.gameObject);
                iTween.ColorTo(renderer.gameObject, iTween.Hash(
                    "color", color,
                    "time", time,
                    "easetype", iTween.EaseType.linear
                ));
            }
            Utils.SetTimeOut(OnFinish, time);
        }

        private void OnFinish()
        {
            _manager.CellFree(this);
        }

        public void Init(CellHighlightManager manager)
        {
            _manager = manager;
        }
    }
}