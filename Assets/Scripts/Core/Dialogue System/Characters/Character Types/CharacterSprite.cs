using UnityEngine;
using System.Collections;

namespace CHARACTERS
{
    public class CharacterSprite : Character
    {
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();

        public CharacterSprite(string name, CharacterConfigData config, GameObject obj) : base(name, config, obj)
        {
            rootCG.alpha = 0;
            Debug.Log($"Created sprite character {name}");
        }

        public override IEnumerator ShowOrHide(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
                yield return null;
            }

            revealingProcess = null;
            hidingProcess = null;
        }

        public override IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            float rot = faceLeft ? 0 : 180;
            root.localRotation = Quaternion.Euler(0, rot, 0);
            yield return null;

            flippingProcess = null;
        }
    }
}