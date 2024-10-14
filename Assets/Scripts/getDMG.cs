using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getDMG : MonoBehaviour
{
    [SerializeReference] private AudioSource audioSource;
    [SerializeReference] private AudioClip death_audio;
    public int maxHP = 100;
    public int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }
    public void TakeDMG(int DMG)
    {
        currentHP -= DMG;
        Debug.Log(currentHP);
        if(currentHP <= 0)
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
