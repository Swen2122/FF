using UnityEngine;

public abstract class AbstractReactionEffect : MonoBehaviour
{
    [SerializeField] protected ElementalEnergyData energyData;
    protected ElementalReaction.ReactionEffect settings;
    [SerializeField] protected float currentEnergy;
    [SerializeField] protected float maxEnergy;
    protected float nextTickTime;
    protected float damage;
    protected float radius;
    public virtual void Initialize(ElementalReaction.ReactionEffect effect)
    {
        if (effect == null)
        {
            Debug.LogError("Initialize in AbstractReactionEffect called with null effect.");
            return;
        }
        settings = effect;
        maxEnergy = energyData.maxEnergy; 
        currentEnergy = settings.energy;
        radius = settings.radius;
    }
    protected virtual void Update()
    {
        if (PauseManager.IsPaused) return;
        if (settings == null)
        {
            Debug.LogError("Update called before Initialize in AbstractReactionEffect.");
            return;
        }

        if (currentEnergy <= 0)
        {
            OnReactionEnd();
            return;
        }

        if (Time.time >= nextTickTime)
        {
            ProcessEnergyTick();
            nextTickTime = Time.time + settings.tickInterval;
        }
    }

    // ������� ����������� ��������� ����㳿
    protected virtual void ProcessEnergyTick()
    {
        if (settings == null)
        {
            Debug.LogError("ProcessEnergyTick called without valid settings.");
            return;
        }
        currentEnergy -= settings.energyPerTick;
        OnEnergyTick();
    }

    // ������� � ������ ����������
    public virtual void InteractWithElement(Element element, float power)
    {
        if (settings == null || energyData == null)
        {
            Debug.LogError("InteractWithElement called with missing settings or energyData.");
            return;
        }

        float modifier = energyData.GetInteractionModifier(element);
        float energyChange = power * modifier;

        var interaction = energyData.elementInteractions.Find(x => x.interactingElement == element);
        if (interaction != null && interaction.canDisrupt && modifier < 0)
        {
            if (currentEnergy + energyChange <= 0)
            {
                OnReactionDisrupted();
                return;
            }
        }

        currentEnergy = Mathf.Clamp(currentEnergy + energyChange, 0, maxEnergy);
    }
    public void CheckElementalInteraction(Collider2D obj)
    {
        if (obj.TryGetComponent<IElementalObject>(out var elementalObj))
        {
            InteractWithElement(elementalObj.CurrentElement, 10f);
        }
    }
    protected abstract void OnEnergyTick();
    protected abstract void OnReactionEnd();
    protected abstract void OnReactionDisrupted();
}