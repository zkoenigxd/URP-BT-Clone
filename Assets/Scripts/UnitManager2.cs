using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UnitManager2 : MonoBehaviour
{
    UIManager uIManager;
    ArenaManager arena;
    Rigidbody2D unitRB;
    float speedModifier;
    bool canFire;
    float lastFrameVelocity;

    [SerializeField] GameObject engineAnimations;
    [SerializeField] AudioSource engineSound;

    public float SpeedModifier => speedModifier;
    public bool CanFire => canFire;

    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();
        arena = FindObjectOfType<ArenaManager>();
        if (!arena)
            Debug.LogWarning("ArenaManager not found");
        unitRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (engineAnimations != null && engineSound != null)
        {
            if (unitRB.velocity.magnitude <= lastFrameVelocity)
            {
                engineAnimations.SetActive(false);
                engineSound.Stop();
            }
            else
            {
                engineAnimations.SetActive(true);
                if (!engineSound.isPlaying)
                    engineSound.Play();
            }
            lastFrameVelocity = unitRB.velocity.magnitude;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        arena.RemoveUnit(this.GetComponent<EnemyBrain>());
        Debug.Log("Removed " + name + " from unit list");
        if (GetComponent<Player>() != null)
        {
            uIManager.DisplayRestartButton();
            return;
        }
        if (arena.CheckAllEnemiesDefeated())
        {
            Debug.Log("Level Cleared");
            //uIManager.DisplayWinButton();
        }
    }

    public void Destroy()
    {
        unitRB.simulated = false;
        GetComponentInChildren<ExplosionController>().StartExplosion();
    }
}
