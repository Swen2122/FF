using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewComboDatabase", menuName = "Skills/ComboDatabase")]
public class ComboDatabase : ScriptableObject
{
    [System.Serializable]
    public class Combo
    {
        public ComboPunchData punchCombo;
        public float comboDelay;  // „ас м≥ж ударами
        public Element damageType;
    }
    public List<Combo> punchComboList = new List<Combo>();
}
