using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs instance;

    public PlayerInput playerInput;
    public Action<string> onControlsChanged;

    // Action Map: Player
    public Action<Vector2> onMoveInput;
    public Action<Vector2> onLookInput;
    public Action<Vector2> mousePosChanged;
    public Action<int> onNumSelectInput;
    public Action onJumpInput;
    public Action onInteractInput;
    public Action onTabInput;
    public Action onPrimaryInput;
    public Action onEscapeInput;

    public Vector2 mousePos;
    public bool isRaycastBlocked = false;

    // Action Map: UI

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        InitInputEvents();
        Debug.Log($"Current Action Map: {playerInput.currentActionMap.name}");
    }

    private void OnDestroy()
    {
        DisposeInputEvents();
    }

    private void InitInputEvents()
    {
        playerInput.onControlsChanged += OnControlsChanged;
        playerInput.onDeviceLost += OnDeviceLost;
        playerInput.onDeviceRegained += OnDeviceRegained;

        playerInput.onActionTriggered += OnAction;
    }

    private void DisposeInputEvents()
    {
        playerInput.onControlsChanged -= OnControlsChanged;
        playerInput.onDeviceLost -= OnDeviceLost;
        playerInput.onDeviceRegained -= OnDeviceRegained;

        playerInput.onActionTriggered -= OnAction;
    }

    public void ChangeActionMap(string actionMap)
    {

    }

    #region (General Events)
    private void OnControlsChanged(PlayerInput input)
    {
        onControlsChanged?.Invoke(input.currentControlScheme);
        Debug.LogWarning($"Controls Changed To: {input.currentControlScheme}");
    }

    private void OnDeviceLost(PlayerInput obj)
    {
    }

    private void OnDeviceRegained(PlayerInput obj)
    {
    }
    #endregion

    #region (Action Events)
    private void OnAction(InputAction.CallbackContext ctx)
    {
        switch(playerInput.currentActionMap.name)
        {
            case "Player":
                switch (ctx.action.name)
                {
                    case "Move":
                        onMoveInput?.Invoke(ctx.ReadValue<Vector2>());
                        break;
                    case "Look":
                        onLookInput?.Invoke(ctx.ReadValue<Vector2>());
                        break;
                    case "Selection":
                        if (ctx.action.phase == InputActionPhase.Performed)
                            onPrimaryInput?.Invoke();
                        break;
                    case "Cursor Position":
                        mousePos = ctx.ReadValue<Vector2>();
                        mousePosChanged?.Invoke(mousePos);
                        break;
                    case "Jump":
                        if (ctx.action.phase == InputActionPhase.Performed)
                            onJumpInput?.Invoke();
                        break;
                    case "Interact":
                        if (ctx.action.phase == InputActionPhase.Performed)
                            onInteractInput?.Invoke();
                        break;
                    case "Tab":
                        if (ctx.action.phase == InputActionPhase.Performed)
                            onTabInput?.Invoke();
                        break;
                    case "NumberSelect":
                        if(ctx.action.phase == InputActionPhase.Performed)
                        {
                            // Debug.Log($"Number Key Pressed: {(int.Parse(ctx.action.activeControl.displayName))}");
                            onNumSelectInput?.Invoke(int.Parse(ctx.action.activeControl.displayName));
                        }
                        break;
                    case "Escape":
                        if(ctx.action.phase == InputActionPhase.Canceled)
                            onEscapeInput?.Invoke();
                        break;
                    default:
                        break;
                }
                break;
            case "UI":
                Debug.Log($"UI Input detected");
                break;
        }
    }
    #endregion
}
