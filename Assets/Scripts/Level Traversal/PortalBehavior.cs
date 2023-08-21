using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalBehavior : MonoBehaviour
{
    [SerializeField] string levelName = null;
    [SerializeField] Collider2D portalCollider;
    [SerializeField] float portalTravelDelay = 3;
    [SerializeField] int startPosition;

    string currentScene;
    float collisionTimer;

    private void Awake()
    {
        int countLoaded = SceneManager.sceneCount;
        if (countLoaded > 2) { Debug.LogError("To many open scenes"); }
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
            if (loadedScenes[i].name != "LevelUtilities")
                currentScene = loadedScenes[i].name;
        }
        Debug.Log(currentScene);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (portalCollider != null)
        {
            if (collision.gameObject.GetComponent<Player>() != null)
            {
                StartPortalAvtivation();
            }
        }
    }

    private void OnTrigegrExit2D(Collision collision)
    {
        if (portalCollider != null)
        {
            if (collision.gameObject.GetComponent<Player>() != null)
            {
                collisionTimer = 0;
            }
        }
    }

    void ActivatePortal()
    {
        if (levelName != null)
        {
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
            //SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
    }

    void StartPortalAvtivation()
    {
        collisionTimer += Time.deltaTime;
        if (collisionTimer > portalTravelDelay)
            ActivatePortal();
    }
}
