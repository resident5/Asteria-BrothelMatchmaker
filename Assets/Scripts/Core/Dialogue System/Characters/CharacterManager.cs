using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DIALOGUE;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO Config => DialogueManager.Instance.Config.characterConfigurationAsset;

        private const string CHARACTER_CASTING_ID = " as ";

        private const string CHARACTER_NAME_ID = "<charname>";
        private string CharacterRootPath => $"Characters/{CHARACTER_NAME_ID}";
        private string CharacterPrefabPath => $"{CharacterRootPath}/Character {CHARACTER_NAME_ID}";

        [SerializeField]
        private RectTransform _characterPanel = null;
        public RectTransform CharacterPanel => _characterPanel;
        private void Awake()
        {
            Instance = this;
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return Config.GetConfig(characterName);
        }

        public Character CreateCharacter(string characterName, bool showAfterCreation = false)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogError($"Character already exists {characterName}");
                return null;
            }

            Character_Info info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);

            characters.Add(characterName.ToLower(), character);

            if (showAfterCreation)
                character.Show();

            return character;
        }

        private Character CreateCharacterFromInfo(Character_Info info)
        {
            CharacterConfigData config = info.data;

            switch (config.characterType)
            {
                case Character.CharacterType.TEXT:
                    return new CharacterText(info.name, config);

                case Character.CharacterType.SPRITE:
                case Character.CharacterType.SPRITESHEET:
                    return new CharacterSprite(info.name, config, info.prefab);
            }

            return null;
        }

        public Character GetCharacter(string characterName, bool createCharacter = false)
        {
            if (characters.ContainsKey(characterName.ToLower()))
                return characters[characterName.ToLower()];
            else if (createCharacter)
                return CreateCharacter(characterName);

            return null;
        }

        private Character_Info GetCharacterInfo(string characterName)
        {
            Character_Info result = new Character_Info();

            string[] nameData = characterName.Split(new string[] { CHARACTER_CASTING_ID }, System.StringSplitOptions.RemoveEmptyEntries);

            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;
            result.data = Config.GetConfig(result.castingName);
            result.prefab = GetPrefab(result.castingName);
            return result;
        }

        private GameObject GetPrefab(string characterName)
        {
            string prefabPath = FormatCharacterPath(CharacterPrefabPath, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        private string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private class Character_Info
        {
            public string name = "";
            public string castingName = "";
            public CharacterConfigData data = null;
            public GameObject prefab;
        }

    }
}