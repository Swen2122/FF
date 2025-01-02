using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewHealthStat", menuName = "Healt/HealthStat")]
public class HealthStat : ScriptableObject
{
    public float maxHealth;
    [System.Serializable]
    public class ResistStat
    {
        //(1 - звичайний, < більший резим, > більше шкоди)
        public Element elementType;
        public float resistance;
    }
    [System.Serializable]
    public class EnergyAbsorptionStat
    {
        public Element elementType;
        public float maxEnergy;
        public float energy;
    }
    [Header("Curse Elemental Energy")]
    public List<EnergyAbsorptionStat> energyStat = new List<EnergyAbsorptionStat>();
    [Header("Damage Resistances")]
    public List<ResistStat> resistStat = new List<ResistStat>();
}
