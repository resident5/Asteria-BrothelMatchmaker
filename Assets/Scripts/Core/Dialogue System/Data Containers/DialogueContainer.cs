﻿using System.Collections;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;

        public void SetDialogueColor(Color color) => dialogueText.color = color;

        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;

        public void ActivateDialogueBox(bool active) => root.SetActive(active);
    }
}
