using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.ObjectEffects
{
    public class FallEffect : MonoBehaviour, IObjectEffect
    {
        [SerializeField] private GameObject maskPrefab;
        [SerializeField] private Vector2 _scale;
        [SerializeField] private float _time = 0.25f;
        [SerializeField] private iTween.EaseType easeType = iTween.EaseType.easeInQuad;
        [SerializeField] private Color _fadeColor;
        public ObjectEffectType Type => ObjectEffectType.fall;
        private bool destroyed;

        public void OnDisable()
        {
            destroyed = true;
        }

        public void Execute(GameObject gameObj, Func callback)
        {
            var mask = CreateMask(gameObj.transform.position);
            var allSprites = ChangeSprites(gameObj);
            Move(gameObj, mask, allSprites, callback);
        }

        public void ExecutePos(GameObject gameObj, Vector2 pos, Func callback)
        {
            
        }

        private GameObject CreateMask(Vector3 pos)
        {
            return Instantiate(maskPrefab, pos, Quaternion.identity);
        }

        private SpriteRenderer[] ChangeSprites(GameObject gameObj)
        {
            var allSprites = gameObj.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in allSprites)
            {
                sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            }

            return allSprites;
        }

        private void Move(GameObject gameObj, GameObject mask, SpriteRenderer[] allSprites, Func callback)
        {
            var destination = gameObj.transform.position + Vector3.down * 1;
            iTween.MoveTo(gameObj, iTween.Hash(
                "position", (Vector3)destination,
                "time", _time,
                "easetype", easeType,
                "oncomplete", "onComplete",
                "oncompletetarget", gameObject
            ));

            iTween.ScaleTo(gameObj, iTween.Hash(
                "scale", (Vector3)_scale,
                "time", _time,
                "easetype", easeType
            ));

            foreach (var sprite in allSprites)
            {
                iTween.ColorTo(sprite.gameObject, iTween.Hash(
                    "color", _fadeColor,
                    "time", _time,
                    "easetype", easeType
                ));
            }

            Utils.SetTimeOut(() =>
            {
                if (destroyed) return;
                iTween.Stop(gameObj);
                mask.SetActive(false);
                callback?.Invoke();
            }, _time);
        }
    }
}