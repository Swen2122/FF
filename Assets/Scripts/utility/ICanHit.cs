using UnityEngine;

public abstract class ICanHit : MonoBehaviour
{
    /// <summary>
    /// ����� ��� ��������� ����������. �� ���� ����������� � ������� ������.
    /// </summary>
    /// <param name="damage">ʳ������ ����������.</param>
    /// <param name="element">��� ��������.</param>
    public abstract void TakeHit(float damage, Element element);

    /// <summary>
    /// ��������, �� ��'��� ��� �������.
    /// </summary>
    /// <returns>������� true, ���� ��'��� �������.</returns>
    public abstract bool IsDestroyed();

    /// <summary>
    /// ����� ��� ������� �������� ��'����.
    /// </summary>
    protected abstract void DestroyObject();
}