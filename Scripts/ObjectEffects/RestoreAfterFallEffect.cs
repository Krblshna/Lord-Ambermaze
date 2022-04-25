using System.Collections;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.ObjectEffects
{
    public class RestoreAfterFallEffect : MonoBehaviour, IObjectEffect
    {
        [SerializeField] private int _blinkTimes = 3;
        [SerializeField] private float _blinkDelay = 0.2f;
        public ObjectEffectType Type => ObjectEffectType.restore_after_fall;
        private Color _transparentColor = new Color(1, 1, 1, 0.5f);
        public void Execute(GameObject gameObj, Func callback)
        {
        }

        public void ExecutePos(GameObject gameObj, Vector2 pos, Func callback)
        {
            gameObj.transform.position = pos;
            var allSprites = gameObj.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in allSprites)
            {
                sprite.maskInteraction = SpriteMaskInteraction.None;
                iTween.ScaleTo(gameObj, iTween.Hash(
                    "scale", Vector3.one,
                    "time", 0
                ));
            }
            StartBlinking(allSprites);
        }

        private void StartBlinking(SpriteRenderer[] allSprites)
        {
            StartCoroutine(BlinkingProcess(allSprites));
        }

        IEnumerator BlinkingProcess(SpriteRenderer[] allSprites)
        {
            for (int i = 0; i < _blinkTimes; i++)
            {
                ChangeColor(allSprites, _transparentColor);
                yield return new WaitForSeconds(_blinkDelay);
                ChangeColor(allSprites, Color.white);
                yield return new WaitForSeconds(_blinkDelay);
            }
        }

        private void ChangeColor(SpriteRenderer[] allSprites, Color color)
        {
            foreach (var sprite in allSprites)
            {
                iTween.ColorTo(sprite.gameObject, iTween.Hash(
                    "color", color,
                    "time", 0
                ));
            }
        }
    }
}