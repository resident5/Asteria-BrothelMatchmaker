using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEditor;
using UnityEditor.SceneManagement;

[Overlay(typeof(SceneView), "Quick Menu")]
public class SceneSwapperOverlay : ToolbarOverlay
{

    SceneSwapperOverlay() : base(ShowScenes.scenesId) { }


    [EditorToolbarElement(scenesId, typeof(SceneView))]
    class ShowScenes : EditorToolbarDropdown, IAccessContainerWindow
    {
        public const string scenesId = "Quick Menu/Scenes";

        public ShowScenes()
        {
            this.text = "Scenes";
            this.tooltip = "Show Scenes to swap to";
            this.clicked += OnClick;
        }

        public EditorWindow containerWindow { get; set; }

        void OnClick()
        {
            var menu = new GenericMenu();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                menu.AddItem(new GUIContent(TruncateScenePath(scene.path)), EditorSceneManager.GetActiveScene().path == scene.path, () => SwapScene(scene));
            }
            menu.ShowAsContext();
        }

        void SwapScene(EditorBuildSettingsScene scene)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scene.path);
            }
        }

        string TruncateScenePath(string path)
        {
            string finalName = path.Substring(path.LastIndexOf('/') + 1);
            return finalName;
        }
    }

    //[EditorToolbarElement(itemId, typeof(SceneView))]
    //class SpawnItems : EditorToolbarDropdown, IAccessContainerWindow
    //{
    //    public const string itemId = "Quick Menu/Items";

    //    public EditorWindow containerWindow { get; set; }

    //    public SpawnItems()
    //    {
    //        this.text = "Items";
    //        this.tooltip = "Spawn chosen items";
    //        this.clicked += OnClick;
    //    }

    //    void OnClick()
    //    {
    //        var menu = new GenericMenu();
    //        foreach (var scene in EditorBuildSettings.scenes)
    //        {
    //            menu.AddItem(new GUIContent("Spawn Dark Cube"), false, () => SpawnPrefab("Assets/Prefabs/Environments/Dark Floor Cube.prefab"));
    //        }
    //        menu.ShowAsContext();
    //    }

    //    void SpawnPrefab(string path)
    //    {
    //        GameObject assetObj = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
    //        PrefabUtility.InstantiatePrefab(assetObj);
    //    }
    //}

    //[EditorToolbarElement(selectorId, typeof(SceneView))]
    //class Selector : EditorToolbarDropdown, IAccessContainerWindow
    //{
    //    public const string selectorId = "Quick Menu/Selection";

    //    public EditorWindow containerWindow { get; set; }

    //    public Selector()
    //    {
    //        this.text = "Quick Selections";
    //        this.tooltip = "Quickly select specific items in the hierarchy";
    //        this.clicked += OnClick;
    //    }

    //    void OnClick()
    //    {
    //        var menu = new GenericMenu();
    //        foreach (var scene in EditorBuildSettings.scenes)
    //        {
    //            menu.AddItem(new GUIContent("Select Materials"), false, () => SelectFolder("Assets/MGFX/Materials"));
    //        }
    //        menu.ShowAsContext();
    //    }

    //    void SelectFolder(string path)
    //    {
    //        Object assetObj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
    //        Selection.activeObject = assetObj;
    //        EditorGUIUtility.PingObject(assetObj);
    //    }
    //}
}
