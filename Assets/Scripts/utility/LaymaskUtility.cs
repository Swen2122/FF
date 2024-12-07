using UnityEngine;

public static class LaymaskUtility
{
    /// <summary>
    /// �������� �� ��'��� �������� �� �������� ��������
    /// </summary>
    /// <param name="obj">��'��� ��� ��������</param>
    /// <param name="layermask">LayerMask � ���� ������������ ��'����</param>
    /// <returns> True - ��'��� �������� �� ��������� ����, ������, false</returns>
    public static bool InIsLMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1<<obj.layer)) != 0;
    }
}
