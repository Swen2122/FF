using UnityEngine;

public interface ICanHit
{
    /// <summary>
    /// ����� ��� ��������� ����������.
    /// </summary>
    /// <param name="DMG">ʳ������ ����������, �� ������ ��'���.</param>
    void TakeHit(float damage);
}