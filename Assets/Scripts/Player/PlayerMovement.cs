using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerMovem : MonoBehaviour
{

    //[SerializeField] private float _moveSpeed = 2.5f;
    public static float _moveSpeed = 5f;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;


    public static bool allowedToMove = true;



    private const string _horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
            
    }

    private void Update()
    {



        if (allowedToMove)
        { 
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        _rb.linearVelocity = _movement * _moveSpeed;


            _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_Vertical, _movement.y);
        }



    }


}
