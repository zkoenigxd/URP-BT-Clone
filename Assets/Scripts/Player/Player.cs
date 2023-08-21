using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] GameObject shieldHitAnim;
    [SerializeField] GameObject shieldDesAnim;
    [SerializeField] GameObject damageTakenAnim;
    [SerializeField] float spawnDelay = .5f;
    [SerializeField] float speed = 150;
    [SerializeField] float boostScalar;
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
        approxUnitSpeed = ((speed / playerRB.drag) - Time.fixedDeltaTime * speed) / playerRB.mass;
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.started += Shoot_started;
        playerInputActions.Player.Shoot.canceled += Shoot_canceled;
        playerInputActions.Player.Move.performed += Move_performed;
        playerInputActions.Player.Shoot.performed += Shoot_performed;
        Physics2D.IgnoreLayerCollision(6, 10);

        if (gameManager.EnteringNewArena)
        {
            StartCoroutine(HoldThenBoost());
        }
    }

    public void ResetWeaponInput()
    {
        attachedControllers = GetComponentsInChildren<WeaponController>();
    }

    void Boost()
    {
        //Debug.Log("Boost Activated with force: " + boostInput);
        playerRB.AddForce(gameManager.EntryVector * boostScalar, ForceMode2D.Impulse);
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
        TurnShip();
        Move();
        minimapCamera.orthographicSize = Mathf.Clamp((maxZoom - minZoom) * (playerRB.velocity.magnitude / approxUnitSpeed) + minZoom, minZoom, maxZoom);
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
}