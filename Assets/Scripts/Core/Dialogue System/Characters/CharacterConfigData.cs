using UnityEngine;
using System.Collections;
using TMPro;
using DIALOGUE;

namespace CHARACTERS
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;

        public CharacterConfigData Copy()
        {
            CharacterConfigData copy = new CharacterConfigData();
            copy.name = name;
            copy.alias = alias;
            copy.characterType = characterType;

            copy.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            copy.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            copy.nameFont = nameFont;
            copy.dialogueFont = dialogueFont;
            return copy;
        }

        private static Color DefaultColor => DialogueManager.Instance.Config.defaultTextColor;
        private static TMP_FontAsset DefaultFont => DialogueManager.Instance.Config.defaultFont;

        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData copy = new CharacterConfigData();
                copy.name = "";
                copy.alias = "";
                copy.characterType = Character.CharacterType.TEXT;

                copy.nameColor = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, DefaultColor.a);
                copy.dialogueColor = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, DefaultColor.a);

                copy.nameFont = DefaultFont;
                copy.dialogueFont = DefaultFont;
                return copy;

            }
        }
    }
}