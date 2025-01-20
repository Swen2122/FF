using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCombo", menuName = "Skills/ComboPunchData")]
public class ComboPunchData : ScriptableObject
{
    [System.Serializable]
    public class ComboSegment
    {
        public float damage;
        public float knockback;
        public AnimationClip animation;
        public AudioClip hitSound;  // Звук удару
    }
    public List<ComboSegment> ComboSegments = new List<ComboSegment>();
}


