using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CuteEngine.Utilities;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystemManager : Singleton<InputSystemManager>
{
    const string PLAYER_ACTIONMAP = "Player";
    const string UI_ACTIONMAP = "UI";

    [SerializeField] InputActionAsset playerInputAction;

    #region UnityAction
    public UnityAction onRotateItem;

    #endregion

    InputActionMap playerControlMap;
    InputActionMap uiControlMap;

    bool globalInputEnable = false;
    bool playerControlEnable = true;
    bool uiControlMapEnable = false;

    protected override void InitAfterAwake()
    {
        playerControlMap = playerInputAction.FindActionMap(PLAYER_ACTIONMAP);
        uiControlMap = playerInputAction.FindActionMap(UI_ACTIONMAP);
    }

    void Start()
    {
        ToggleGlobalInput(true);
    }

    #region ToggleInput

    public void ToggleGlobalInput(bool toggle)
    {
        globalInputEnable = toggle;
        UpdateInputState();
    }

    public void TogglePlayerControl(bool toggle)
    {
        playerControlEnable = toggle;
        UpdateInputState();
    }

    public void ToggleUIControl(bool toggle)
    {
        uiControlMapEnable = toggle;
        UpdateInputState();
    }

    void UpdateInputState()
    {
        if (globalInputEnable && playerControlEnable) playerControlMap.Enable();
        else playerControlMap.Disable();

        if (globalInputEnable && uiControlMapEnable) uiControlMap.Enable();
        else uiControlMap.Disable();
    }

    #endregion

    #region ControlFunction

    void OnRotateItem(InputValue value)
    {
        var _value = value.Get<Vector2>();
        if (_value.y != 0)
            onRotateItem?.Invoke();
    }

    #endregion
}
