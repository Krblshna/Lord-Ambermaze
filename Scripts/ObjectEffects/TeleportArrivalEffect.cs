using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.ObjectEffects
{
    public class TeleportArrivalEffect : MonoBehaviour, IObjectEffect
    {
        [SerializeField] private GameObject maskPrefab;
        [SerializeField] private Vector2 _scale;
        private float _time = 1f;
        private iTween.EaseType easeType = iTween.EaseType.linear;
        [SerializeField] private Color _fadeColor;
        public ObjectEffectType Type => ObjectEffectType.teleportArrival;

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

        private void ChangeSpritesBack(GameObject gameObj)
        {
            var allSprites = gameObj.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in allSprites)
            {
                sprite.maskInteraction = SpriteMaskInteraction.None;
            }
        }

        private void Move(GameObject gameObj, GameObject mask, SpriteRenderer[] allSprites, Func callback)
        {
            var anim = mask.GetComponent<Animator>();
            anim?.SetTrigger("show");
            
            Utils.SetTimeOut(() =>
            {
                iTween.Stop(mask);
                mask.SetActive(false);
                ChangeSpritesBack(gameObj);
                callback?.Invoke();
            }, _time);
        }
    }
}