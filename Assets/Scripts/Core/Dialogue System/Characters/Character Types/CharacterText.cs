using UnityEngine;
using System.Collections;

namespace CHARACTERS
{
    public class CharacterText : Character
    {
        public CharacterText(string name, CharacterConfigData config) : base(name, config, prefab: null)
        {
            Debug.Log($"Created text character {name}");
        }
    }
}