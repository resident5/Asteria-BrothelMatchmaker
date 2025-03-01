using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DIALOGUE;

namespace CHARACTERS
{
    public abstract class Character
    {
        public const bool DEFAULT_ORIENTATION_FACE_LEFT = true;

        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public Animator animator;
        public DialogueManager dialogueManager => DialogueManager.Instance;

        public CharacterConfigData config;

        protected bool facingLeft = DEFAULT_ORIENTATION_FACE_LEFT;

        protected CharacterManager characterManager => CharacterManager.Instance;

        //Coroutines
        protected Coroutine revealingProcess, hidingProcess;
        protected Coroutine flippingProcess;
        
        public bool isRevealing => revealingProcess != null;
        public bool isHiding => hidingProcess != null;
        public virtual bool isVisible => false;
        public bool isFacingLeft => facingLeft;
        public bool isFacingRight => !facingLeft;
        public bool isFlipping => flippingProcess != null;

        public enum CharacterType
        {
            TEXT,
            SPRITE,
            SPRITESHEET
        }

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, characterManager.CharacterPanel);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });

        public Coroutine Say(List<string> lines)
        {
            dialogueManager.ShowSpeakerName(displayName);
            UpdateTextOnScreen();
            return dialogueManager.Say(lines);
        }

        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;

        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;

        public void UpdateTextOnScreen() => dialogueManager.ApplySpeakerData(config);
        public void ResetConfigureationData() => characterManager.GetCharacterConfig(name);

        public virtual Coroutine Show()
        {
            if (isRevealing)
                return revealingProcess;

            if (isHiding)
                characterManager.StopCoroutine(hidingProcess);

            revealingProcess = characterManager.StartCoroutine(ShowOrHide(true));

            return revealingProcess;
        }

        public virtual Coroutine Hide()
        {
            if (isHiding)
                return hidingProcess;

            if (isRevealing)
                characterManager.StopCoroutine(revealingProcess);

            hidingProcess = characterManager.StartCoroutine(ShowOrHide(false));

            return hidingProcess;

        }

        public virtual IEnumerator ShowOrHide(bool show)
        {
            Debug.Log("Show and Hide cant be called from here");
            yield return null;
        }

        public Coroutine Flip()
        {
            if (isFacingLeft)
                return FaceRight();
            else
                return FaceLeft();
        }

        public Coroutine FaceLeft(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                characterManager.StopCoroutine(flippingProcess);

            facingLeft = true;
            flippingProcess = characterManager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));

            return flippingProcess;
        }

        public Coroutine FaceRight(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                characterManager.StopCoroutine(flippingProcess);

            facingLeft = false;
            flippingProcess = characterManager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));

            return flippingProcess;
        }

        public virtual IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            Debug.Log("Cannot flip");
            yield return null;
        }

        public virtual void OnReceiveCastingExpression(int layer, string expression)
        {
            return;
        }
    }
}