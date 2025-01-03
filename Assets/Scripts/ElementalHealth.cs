using System.Collections.Generic;
using UnityEngine;

public class ElementalHealth : ICanHit
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private HealthStat healthStat;
    [SerializeField] private Element elementType;
    [SerializeField] private float currentEnergy;
    [SerializeField] private float maxEnergy;
    
    void Start()
    {
        var energyInfo = healthStat.energyStat.Find(e => e.elementType == elementType);
        if(energyInfo != null)
        {
            maxEnergy = energyInfo.maxEnergy;
            currentEnergy = energyInfo.energy;
        }
        else
        {
            Debug.LogError("energyInfo not found");
        }
        
    }
    public override void TakeHit(float damage, Element elementType)
    {
        // Обчислення шкоди з урахуванням резистів
        AbsorbEnergy(elementType, damage);
        CheckForFreeze();
        if (healthStat.hit_audio != null)
            audioSource.PlayOneShot(healthStat.hit_audio);

        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        Debug.Log($"Current HP: {currentEnergy}");

        if (currentEnergy <= 0)
        {
            DestroyObject();
        }
    }
    private Dictionary<Element, float> externalEnergies = new Dictionary<Element, float>();

    public void AbsorbEnergy(Element energyType, float amount)
    {
        if (energyType == elementType)
        {
            // Якщо елемент співпадає, додаємо до основної енергії
            currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        }
        else
        {
            // Накопичуємо енергію від інших елементів
            if (!externalEnergies.ContainsKey(energyType))
            {
                externalEnergies[energyType] = 0;
            }
            externalEnergies[energyType] += amount;
        }

        Debug.Log($"Energy from {energyType}: {externalEnergies[energyType]}");
    }
    private void CheckForFreeze()
    {
        if (externalEnergies.TryGetValue(Element.Ice, out float coldEnergy) && coldEnergy >= currentEnergy)
        {
            FreezeElemental();
        }
    }
    private void FreezeElemental()
    {
        if (gameObject.TryGetComponent<IEnemyAI>(out var ai))
        {
            ai.DisableAI();
        }
    }
    public override bool IsDestroyed()
    {
        return currentEnergy <= 0;
    }
    protected override void DestroyObject()
    {
        if (healthStat.deathAudio != null)
        {
            audioSource.PlayOneShot(healthStat.deathAudio);
        }

        Destroy(gameObject);
    }
}
