using Naninovel.Commands;
using Naninovel;
using UnityEngine;

[CommandAlias("novel")]
public class SwitchToNovelMode : Command
{
    public StringParameter ScriptName;
    public StringParameter Label;

    public override async UniTask Execute(AsyncToken token = default)
    {
        // 1. Disable character control.

        // 2. Switch cameras.
        var advCamera = GameObject.Find("Camera - Main").GetComponent<Camera>();
        advCamera.enabled = false;
        var naniCamera = Engine.GetServiceOrErr<ICameraManager>().Camera;
        naniCamera.enabled = true;

        // 3. Load and play specified script (is required).
        if (Assigned(ScriptName))
        {
            var scriptPlayer = Engine.GetServiceOrErr<IScriptPlayer>();
            await scriptPlayer.LoadAndPlayAtLabel(ScriptName, Label);
        }

        // 4. Enable Naninovel input.
        var inputManager = Engine.GetServiceOrErr<IInputManager>();
        inputManager.ProcessInput = true;
    }
}
