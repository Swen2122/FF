using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewHealthStat", menuName = "Healt/HealthStat")]
public class HealthStat : ScriptableObject
{
    [Header("Base")]
    public float maxHealth;
    public AudioClip deathAudio;
    public AudioClip hit_audio;
    [System.Serializable]
    public class ResistStat
    {
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
