using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    WeaponController[] attachedControllers;
    UnitManager2 unitManager;
    JoyStickInput playerInputActions;
    Rigidbody2D playerRB;
    Vector2 moveInput;
    Vector2 fireInput;
    bool playerIsDead;
    float approxUnitSpeed;

    [SerializeField] Camera minimapCamera;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] GameObject fireDirection;
    [SerializeField] float spawnDelay = .5f;
    public Rigidbody2D PlayerRB => playerRB;
    public Vector2 FireInput => fireInput;
    public Vector2 MoveInput => moveInput;
    public bool PlayerIsDead => playerIsDead;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerIsDead = false;
        attachedControllers = GetComponentsInChildren<WeaponController>();
        playerInputActions = new JoyStickInput();
        playerRB = GetComponent<Rigidbody2D>();
        unitManager = GetComponent<UnitManager2>();
        playerRB.centerOfMass = Vector3.zero;
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.started += Shoot_started;
        playerInputActions.Player.Shoot.canceled += Shoot_canceled;
        playerInputActions.Player.Move.performed += Move_performed;
        playerInputActions.Player.Shoot.performed += Shoot_performed;
        approxUnitSpeed = unitManager.UnitSpeed * 5;
        Physics2D.IgnoreLayerCollision(6, 10);

        if (gameManager.EnteringNewArena)
        {
            StartCoroutine(HoldOnStart());
            unitManager.SetBoostInpust((gameManager.EntryVector * approxUnitSpeed));
        }
    }

    public void ResetWeaponInput()
    {
        attachedControllers = GetComponentsInChildren<WeaponController>();
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
            for (int i = 0; i < attachedControllers.Count(); ++i)
                attachedControllers[i].StartRepeatFire();
    }

    private void Shoot_performed(InputAction.CallbackContext context) { }

    private void Shoot_canceled(InputAction.CallbackContext obj)
    {
        for (int i = 0; i < attachedControllers.Count(); ++i)
            attachedControllers[i].StopRepeatFire();
    }

    private void Move_performed(InputAction.CallbackContext context) { }

    void FixedUpdate()
    {
        minimapCamera.orthographicSize = Mathf.Clamp(((maxZoom - minZoom) / (approxUnitSpeed)) * playerRB.velocity.magnitude + minZoom, minZoom, maxZoom);
    }

    private void Update()
    {
        unitManager.SetMoveInput(playerInputActions.Player.Move.ReadValue<Vector2>());
        if (playerInputActions.Player.Shoot.ReadValue<Vector2>() != Vector2.zero)
        {
            fireDirection.transform.rotation = Quaternion.FromToRotation(Vector2.down, playerInputActions.Player.Shoot.ReadValue<Vector2>());
        }
    }

    private void OnDestroy()
    {
        playerIsDead = true;
    }

    IEnumerator HoldOnStart()
    {
        yield return new WaitForSeconds(spawnDelay);
        unitManager.Boost();
    }
}