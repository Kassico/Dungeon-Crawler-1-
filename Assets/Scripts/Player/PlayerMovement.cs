using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float _moveSpeed = 5f;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;

    public static bool allowedToMove = true;

    private float footstepTimer;
    private float footstepInterval = 0.4f; // hur ofta fotstegsljudet spelas när spelaren rör sig

    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private void Awake() // hämtar komponenter och sätter moveSpeed till det som är satt i playerData, så att den kan sparas mellan sceneer
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        if (playerData.instance != null && playerData.isInitialized)
        {
            _moveSpeed = playerData.instance.moveSpeed;
        }

    }

    private void Start() // flyttar spelaren till spawn pointen när scenen startar
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
        }
    }

    private void Update() // hanterar rörelse och animationer, samt spelar fotstegsljud när spelaren rör sig
    {
        PlayerAudioManeger playerAudioManeger = GetComponent<PlayerAudioManeger>();



        if (allowedToMove)
        { 
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        _rb.linearVelocity = _movement * _moveSpeed;



            if (_rb.linearVelocity.sqrMagnitude > 0.1f)
            {
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0)
                {
                    playerAudioManeger.PlayFootstep();
                    footstepTimer = footstepInterval;
                }
            }



            _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_Vertical, _movement.y);
        }



    }


}
