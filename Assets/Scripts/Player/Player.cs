using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;
using Pathfinding.RVO;
using System;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    Controller[] attachedControllers;
    WeaponController[] attachedWeaponControllers;
    PlayerStats stats;
    PlayerInputActions playerInputActions;
    Rigidbody2D playerRB;
    float turnInput;
    Vector2 thrustInput;
    Vector2 fireInput;
    bool playerIsDead;
    float approxUnitSpeed;
    RVOController rvo;

    [SerializeField] GameObject fireDirection;
    [SerializeField] Transform targetTransform;
    [SerializeField] GameObject shieldHitAnim;
    [SerializeField] GameObject shieldDesAnim;
    [SerializeField] GameObject damageTakenAnim;
    [SerializeField] GameObject shipPrefab;
    [SerializeField] Camera minimapCamera;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] float spawnDelay = .5f;
    [SerializeField] float speed = 150;
    [SerializeField] float turnSpeed = 5;
    [SerializeField] float boostScalar;

    public Transform TargetTransform => targetTransform;
    public Rigidbody2D PlayerRB => playerRB;
    public bool PlayerIsDead => playerIsDead;
    public Controller[] AttachedControllers => attachedControllers;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        gameManager = GameManager.Instance;
        playerIsDead = false;
        Instantiate(shipPrefab, this.transform);
        attachedControllers = shipPrefab.GetComponentsInChildren<Controller>();
        playerInputActions = new PlayerInputActions();
        playerRB = GetComponent<Rigidbody2D>();
        Debug.Log(playerRB.ToString());
        playerRB.centerOfMass = Vector3.zero;
        approxUnitSpeed = ((speed / playerRB.drag) - Time.fixedDeltaTime * speed) / playerRB.mass;
        playerInputActions.PlayerMovement.Enable();
        playerInputActions.PlayerMovement.Shoot.started += Shoot_started;
        playerInputActions.PlayerMovement.Shoot.canceled += Shoot_canceled;
        playerInputActions.PlayerMovement.Shoot.performed += Shoot_performed;
        rvo = GetComponentInChildren<RVOController>();
        if (rvo != null)
            Debug.Log("Successfully got RVO");
        Physics2D.IgnoreLayerCollision(6, 10);
        ResetWeaponInput();
        if (gameManager.EnteringNewArena)
        {
            StartCoroutine(HoldThenBoost());
        }
        stats.InitializeUI();
    }

    public void ResetWeaponInput()
    {
        attachedWeaponControllers = GetComponentsInChildren<WeaponController>();
    }

    void Boost()
    {
        //Debug.Log("Boost Activated with force: " + boostInput);
        playerRB.AddForce(gameManager.EntryVector * boostScalar, ForceMode2D.Impulse);
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
            for (int i = 0; i < attachedWeaponControllers.Count(); ++i)
                attachedWeaponControllers[i].StartRepeatFire();
    }

    private void Shoot_performed(InputAction.CallbackContext context) { }

    private void Shoot_canceled(InputAction.CallbackContext obj)
    {
        for (int i = 0; i < attachedWeaponControllers.Count(); ++i)
            attachedWeaponControllers[i].StopRepeatFire();
    }

    void FixedUpdate()
    {
        TurnShip();
        ApplyThrust();
        minimapCamera.orthographicSize = Mathf.Clamp((maxZoom - minZoom) * (playerRB.velocity.magnitude / approxUnitSpeed) + minZoom, minZoom, maxZoom);
        rvo.velocity = playerRB.velocity;
    }

    private void Update()
    {
        thrustInput = playerInputActions.PlayerMovement.ApplyThrust.ReadValue<Vector2>();
        turnInput = playerInputActions.PlayerMovement.Turn.ReadValue<float>();
        if (playerInputActions.PlayerMovement.Shoot.ReadValue<Vector2>() != Vector2.zero)
        {
            fireDirection.transform.rotation = Quaternion.FromToRotation(Vector2.right, playerInputActions.PlayerMovement.Shoot.ReadValue<Vector2>());
        }
    }

    private void OnDestroy()
    {
        playerIsDead = true;
    }

    IEnumerator HoldThenBoost()
    {
        yield return new WaitForSeconds(spawnDelay);
        Boost();
    }

    private void TurnShip()
    {
            Debug.Log("Turn: " + turnInput);
            PlayerRB.AddTorque(turnInput * turnSpeed);
    }

    private void ApplyThrust()
    {
        if (thrustInput != Vector2.zero)
        {
            Debug.Log("Tranform: " + transform.up);
            thrustInput = (Vector2.Dot(thrustInput, transform.up)) * transform.up;
            Debug.Log("Thrust: " + thrustInput.ToString());
            playerRB.AddRelativeForce(speed * thrustInput);
        }
    }

        public void PlayShieldHitAnim()
    {
        shieldHitAnim.SetActive(true);
        StartCoroutine(DeactivateAfterTime(.1f, shieldHitAnim));
    }

    public void PlayShieldDestroyedAnim()
    {
        shieldDesAnim.SetActive(true);
        StartCoroutine(DeactivateAfterTime(.2f, shieldDesAnim));
    }

    public void PlayDamageTakenAnim()
    {
        damageTakenAnim.SetActive(true);
        StartCoroutine(DeactivateAfterTime(.05f, damageTakenAnim));
    }

    IEnumerator DeactivateAfterTime(float holdTime, GameObject activatedObject)
    {
        yield return new WaitForSeconds(holdTime);
        activatedObject.SetActive(false);
    }

    public void SetShipPrefab(GameObject ship)
    {
        shipPrefab = ship;
        attachedControllers = GetComponentsInChildren<WeaponController>();
    }
}