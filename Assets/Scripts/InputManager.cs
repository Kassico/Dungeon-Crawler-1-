using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    private PlayerInput _PlayerInput;
    private InputAction _moveaction;
    private InputAction _attackaction;
    public static bool Attack;



    private void Awake()
    {
        _PlayerInput = GetComponent<PlayerInput>();

        _moveaction = _PlayerInput.actions["Move"];
        _attackaction = _PlayerInput.actions["attack"];
    }

    private void Update()
    {
        Movement = _moveaction.ReadValue<Vector2>();
        Attack = _attackaction.WasPerformedThisFrame();
        if (Attack)
            Debug.Log("ATTACK BUTTON PRESSED");

    }
}

