using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject defaultSprite;
    public GameObject tieSprite;
    public GameObject socksSprite;
    public GameObject tieAndSocksSprite;

    public Animator defaultAnimator;
    public Animator tieAnimator;
    public Animator socksAnimator;
    public Animator tieAndSocksAnimator;

    [HideInInspector]
    public Animator currentAnimator;

    private PlayerState _currentState;
    private PlayerInput _playerInput;

    // Properties to track collected items
    public bool HasTie { get; private set; }
    public bool HasSocks { get; private set; }
    public bool HasResume { get; private set; }

    public void CollectTie()
    {
        HasTie = true;
        InitStateChange();
    }

    public void CollectSocks()
    {
        HasSocks = true;
        InitStateChange();
    }

    public void CollectResume()
    {
        HasResume = true;
    }

    public void SetAppearance(GameObject sprite, Animator animator)
    {
        // Activate the desired sprite
        sprite.SetActive(true);

        //Set the current, active Animator
        currentAnimator = animator;
    }

    public void SwitchToPlayerActionMap()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    public void SwitchToUIActionMap()
    {
        _playerInput.SwitchCurrentActionMap("UI");
    }

    public void EnableSneezeAction()
    {
        if (_playerInput.currentActionMap.name == "Player")
        {
            _playerInput.actions["Sneeze"].Enable();
        }
    }

    public void DisableSneezeAction()
    {
        if (_playerInput.currentActionMap.name == "Player")
        {
            _playerInput.actions["Sneeze"].Disable();
        }
    }

    public void EnableDashAction()
    {
        if (_playerInput.currentActionMap.name == "Player")
        {
            _playerInput.actions["Dash"].Enable();
        }
    }

    public void DisableDashAction()
    {
        if (_playerInput.currentActionMap.name == "Player")
        {
            _playerInput.actions["Dash"].Disable();
        }
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        // Set initial PlayerState to default
        ChangeState(new PlayerDefaultState(this));
    }

    private void Update()
    {
        _currentState.OnStateUpdate();
    }

    private void InitStateChange()
    {
        if (HasTie && !HasSocks)
        {
            ChangeState(new PlayerTieState(this));
        }
        else if (HasSocks && !HasTie)
        {
            ChangeState(new PlayerSocksState(this));
        }
        else if (HasTie && HasSocks)
        {
            ChangeState(new PlayerTieAndSocksState(this));
        }
        else
        {
            ChangeState(new PlayerDefaultState(this));
        }
    }

    private void ChangeState(PlayerState newState)
    {
        _currentState?.OnStateExit();
        _currentState = newState;
        _currentState.OnStateEnter();
    }
}
