using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CHARACTERS
{
    [CreateAssetMenu(fileName = "Character Config", menuName = "Dialogue System/Character Config Asset")]
    public class CharacterConfigSO : ScriptableObject
    {
        //Use Visitor SO for this instead.
        public CharacterConfigData[] characters;

        public CharacterConfigData GetConfig(string characterName)
        {
            characterName = characterName.ToLower();

            foreach (CharacterConfigData character in characters)
            {
                CharacterConfigData data = character;

                if(string.Equals(characterName, data.name.ToLower()) || string.Equals(characterName, data.alias.ToLower()))
                {
                    return data.Copy();
                }
            }

            return CharacterConfigData.Default;
        }
    }
}