using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;
using Pathfinding.RVO;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    Controller[] attachedControllers;
    WeaponController[] attachedWeaponControllers;
    UnitManager2 unitManager;
    PlayerStats stats;
    JoyStickInput playerInputActions;
    Rigidbody2D playerRB;
    Vector2 moveInput;
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
    [SerializeField] float boostScalar;

    public Transform TargetTransform => targetTransform;
    public Rigidbody2D PlayerRB => playerRB;
    public Vector2 FireInput => fireInput;
    public Vector2 MoveInput => moveInput;
    public bool PlayerIsDead => playerIsDead;
    public Controller[] AttachedControllers => attachedControllers;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        gameManager = GameManager.Instance;
        playerIsDead = false;
        Instantiate(shipPrefab, this.transform);
        attachedControllers = shipPrefab.GetComponentsInChildren<Controller>();
        playerInputActions = new JoyStickInput();
        playerRB = GetComponent<Rigidbody2D>();
        playerRB.mass = shipPrefab.GetComponent<ShipTraits>().ShipMass;
        Debug.Log(playerRB.ToString());
        unitManager = GetComponent<UnitManager2>();
        playerRB.centerOfMass = Vector3.zero;
        approxUnitSpeed = ((speed / playerRB.drag) - Time.fixedDeltaTime * speed) / playerRB.mass;
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.started += Shoot_started;
        playerInputActions.Player.Shoot.canceled += Shoot_canceled;
        playerInputActions.Player.Move.performed += Move_performed;
        playerInputActions.Player.Shoot.performed += Shoot_performed;
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

    private void Move_performed(InputAction.CallbackContext context) { }

    void FixedUpdate()
    {
        TurnShip();
        Move();
        minimapCamera.orthographicSize = Mathf.Clamp((maxZoom - minZoom) * (playerRB.velocity.magnitude / approxUnitSpeed) + minZoom, minZoom, maxZoom);
        rvo.velocity = playerRB.velocity;
    }

    private void Update()
    {
        moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        if (playerInputActions.Player.Shoot.ReadValue<Vector2>() != Vector2.zero)
        {
            fireDirection.transform.rotation = Quaternion.FromToRotation(Vector2.down, playerInputActions.Player.Shoot.ReadValue<Vector2>());
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
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(-moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), .1f);
        }
    }

    private void Move()
    {
        playerRB.AddForce(speed * moveInput.normalized);
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