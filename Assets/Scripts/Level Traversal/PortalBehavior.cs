using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalBehavior : MonoBehaviour
{
    public float timeTillJump;

    [SerializeField] Scene nextLevel;
    [SerializeField] Collider2D portalCollider;
    [SerializeField] float portalTravelDelay = 3;
    [SerializeField] int startPosition;

    IEnumerator portalTimer;
    float collisionTimer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (portalCollider != null)
        {
            if (collision.gameObject.GetComponent<Player>() != null)
            {
                StartCoroutine( portalTimer = StartPortalActivation(portalTravelDelay));
            }
        }
    }

    private void OnTrigegrExit2D(Collision collision)
    {
        if (portalCollider != null)
        {
            if (collision.gameObject.GetComponent<Player>() != null)
            {
                StopCoroutine(portalTimer);
            }
        }
    }

    void ActivatePortal()
    {
        if (nextLevel != null)
        {
            SceneManager.LoadScene(nextLevel.name);
        }
    }

    IEnumerator StartPortalActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivatePortal();

        collisionTimer += Time.deltaTime;
        timeTillJump = collisionTimer - portalTravelDelay;
        if (collisionTimer > portalTravelDelay)
            ActivatePortal();
    }
}
