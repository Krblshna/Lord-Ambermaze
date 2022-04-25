using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.ObjectEffects
{
    public class TeleportEffect : MonoBehaviour, IObjectEffect
    {
        [SerializeField] private GameObject maskPrefab;
        [SerializeField] private Vector2 _scale;
        private float _time = 0.7f;
        private iTween.EaseType easeType = iTween.EaseType.linear;
        [SerializeField] private Color _fadeColor;
        public ObjectEffectType Type => ObjectEffectType.teleport;

        public void Execute(GameObject gameObj, Func callback)
        {
            var mask = CreateMask(gameObj.transform);
            var allSprites = ChangeSprites(gameObj);
            Move(gameObj, mask, allSprites, callback);
        }

        public void ExecutePos(GameObject gameObj, Vector2 pos, Func callback)
        {
            
        }

        private GameObject CreateMask(Transform parent)
        {
            return Instantiate(maskPrefab, parent.position, Quaternion.identity, parent);
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
            var destination = mask.transform.position + Vector3.up * 2;
            var anim = mask.GetComponent<Animator>();
            anim?.SetTrigger("show");
            //iTween.MoveTo(mask, iTween.Hash(
            //    "position", (Vector3)destination,
            //    "time", _time,
            //    "easetype", easeType,
            //    "oncomplete", "onComplete",
            //    "oncompletetarget", gameObject
            //));

            //foreach (var sprite in allSprites)
            //{
            //    iTween.ColorTo(sprite.gameObject, iTween.Hash(
            //        "color", _fadeColor,
            //        "time", _time,
            //        "easetype", easeType
            //    ));
            //}

            Utils.SetTimeOut(() =>
            {
                iTween.Stop(mask);
                //mask.SetActive(false);
                callback?.Invoke();
            }, _time);
        }
    }
}