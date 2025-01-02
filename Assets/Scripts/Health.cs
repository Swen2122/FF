using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Health : MonoBehaviour, ICanHit
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip death_audio;
    [SerializeField] private AudioClip hit_audio;
    [SerializeField] private HealthStat healthStat;
    public float maxHP = 100;
    public float currentHP;
    public Image HP_bar;
    private void Start()
    {
        if (healthStat != null)
        {
            maxHP = healthStat.maxHealth;
        }
        currentHP = maxHP;
        if (HP_bar != null)
        {
            HP_bar.fillAmount = currentHP / 100;
        }
    }
    public void TakeHit(float DMG)
    {     
        currentHP -= DMG;
        if (hit_audio != null) audioSource.PlayOneShot(hit_audio);  
        currentHP = Mathf.Min(currentHP, maxHP);
        currentHP = Mathf.Max(currentHP, 0);
        if (HP_bar != null)
        {
            HP_bar.fillAmount = currentHP / 100;
        }
        Debug.Log(currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        audioSource.PlayOneShot(death_audio);
        Destroy(gameObject);
    }
}
