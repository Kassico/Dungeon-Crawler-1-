using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    private PlayerInput _PlayerInput;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _dashAction;
    public static bool Attack;
    public static bool Dash;




    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();

        _moveAction = _PlayerInput.actions["Move"];
        _attackAction = _PlayerInput.actions["attack"];
        _dashAction = _PlayerInput.actions["dash"];
    }

    private void Update()
    {
        PlayerAudioManeger playerAudioManeger = GetComponent<PlayerAudioManeger>();

        Movement = _moveAction.ReadValue<Vector2>();
        Attack = _attackAction.WasPerformedThisFrame();
        Dash = _dashAction.WasPerformedThisFrame();


    }
}

