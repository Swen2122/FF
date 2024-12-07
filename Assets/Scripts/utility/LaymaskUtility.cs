using UnityEngine;

public static class LaymaskUtility
{
    /// <summary>
    /// ѕерев≥р€Ї чи об'Їкт належить до заданого леймаску
    /// </summary>
    /// <param name="obj">ќб'Їкт дл€ перев≥рки</param>
    /// <param name="layermask">LayerMask з €ким перев≥р€ютьс€ об'Їкти</param>
    /// <returns> True - об'Їкт належить до потр≥бного шару, ≥накше, false</returns>
    public static bool InIsLMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1<<obj.layer)) != 0;
    }
}
