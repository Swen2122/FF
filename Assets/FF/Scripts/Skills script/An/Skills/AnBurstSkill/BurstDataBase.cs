using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDataBase", menuName = "Skills/Burst/DataBase")]
public class BurstDataBase : ScriptableObject
{
    public List<AnBurstSO> allBursts;
    private Dictionary<Element, AnBurstSO> burstDictionary;

    private void OnEnable()
    {
        burstDictionary = new Dictionary<Element, AnBurstSO>();
        foreach (var burst in allBursts)
        {
            burstDictionary[burst.element] = burst;
        }
    }
    public AnBurstSO GetBurst(Element element)
    {
        return burstDictionary[element];
    }
}
