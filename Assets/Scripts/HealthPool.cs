using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthPool : MonoBehaviour
{
    [SerializeField] int maxHealth;
    float currentHealth;
    [SerializeField] AudioClip smallHitSound;
    [SerializeField] AudioClip bigHitSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Slider healthBarSlider;

    public int MaxHealth => maxHealth;

    private void Awake()
    {
        if(maxHealth <= 0)
            maxHealth = 1;
        currentHealth = maxHealth;
        if(GetComponent<Player>() != null )
            SetUpHealthBar();
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (damageAmount < 5 && damageAmount >= 1)
            audioSource.clip = smallHitSound;
        if (damageAmount >= 5)
            audioSource.clip = bigHitSound;
        audioSource.Play();
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            if (GetComponent<DropController>() != null)
                gameObject.GetComponent<DropController>().DropDefeatMaterial();
            if (GetComponentInChildren<ExplosionController>() != null)
            {
                GetComponentInChildren<ExplosionController>().StartExplosion();
            }
            else
                Destroy(gameObject);
        }
    }

    void SetUpHealthBar()
    {
        RectTransform rectTransform = healthBarSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * maxHealth);
    }

    void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth/maxHealth;
    }
}
