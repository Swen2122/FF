using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class getDMG : MonoBehaviour
{
    [SerializeReference] private AudioSource audioSource;
    [SerializeReference] private AudioClip death_audio;
    public float maxHP = 100;
    public float currentHP;
    public Image HP_bar;

    private void Start()
    {
        currentHP = maxHP;
        if (HP_bar != null)
        {
            HP_bar.fillAmount = currentHP / 100;
        }
    }
    public void TakeDMG(int DMG)
    {     
        currentHP -= DMG;
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
