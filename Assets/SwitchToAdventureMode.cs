using Naninovel.Commands;
using Naninovel;
using UnityEngine;

[CommandAlias("adventure")]
public class SwitchToAdventureMode : Command
{
    [ParameterAlias("reset")]
    public BooleanParameter ResetState = true;

    public override async UniTask Execute(AsyncToken token = default)
    {
        // 1. Disable Naninovel input.
        var inputManager = Engine.GetServiceOrErr<IInputManager>();
        inputManager.ProcessInput = true;

        // 2. Stop script player.
        var scriptPlayer = Engine.GetServiceOrErr<IScriptPlayer>();
        scriptPlayer.Stop();

        // 3. Hide text printer.
        var hidePrinter = new HidePrinter();
        hidePrinter.Execute(token).Forget();

        // 4. Reset state (if required).
        if (ResetState)
        {
            var stateManager = Engine.GetServiceOrErr<IStateManager>();
            await stateManager.ResetState();
        }

        // 5. Switch cameras.
        var mainCamera = GameObject.Find("Camera - Main").GetComponent<Camera>();
        var naniCamera = Engine.GetServiceOrErr<ICameraManager>().Camera;
        var uiCamera = Engine.GetServiceOrErr<ICameraManager>().UICamera;

        mainCamera.enabled = true;
        naniCamera.enabled = false;
    }
}
