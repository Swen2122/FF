using System.Collections.Generic;
using UnityEngine;

public class trap_script : MonoBehaviour
{
    private HashSet<GameObject> _enemy = new HashSet<GameObject>(); // HashSet ��� ��������� ������
    public Collider2D myCollider; // �������� ��� ���������� ���� �����
    [Header("��� �� ���� �������� �����")]
    public LayerMask targetLayer;

    [Header("Animation")]
    [SerializeReference] private Animator anim;
    private bool isActive = false; // ��������� ������

    [Header("Sound")]
    [SerializeReference] private AudioSource audioSource;
    [SerializeReference] private AudioClip beep_audio;
    [SerializeReference] private AudioClip explosion_audio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidTarget(other))
        {
            if (_enemy.Add(other.gameObject)) // ������ ��'��� �� ������ ������
            {
                StartTrap(); // ��������� ������
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsValidTarget(other) && !isActive)
        {
            StartTrap(); // �������������, ���� ������ ���� ����������
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsValidTarget(other))
        {
            _enemy.Remove(other.gameObject); // ��������� ��'��� �� HashSet
            if (_enemy.Count == 0)
            {
                StopTrap(); // ��������� ������, ���� ������ ����
            }
        }
    }

    private void StartTrap()
    {
        anim.SetBool("isActive", true); // �������� �������
        isActive = true; // ������ �������
        beep(); // ³��������� ���� ������������
    }

    private void StopTrap()
    {
        anim.SetBool("isActive", false); // ��������� �������
        isActive = false; // ������ ���������
    }

    public void boom()
    {
        FindEnemy(); // ��������� ������ ������
        Damage.ApplyDamage(new List<GameObject>(_enemy).ToArray(), 40, Element.None); // ������� ��� � ���
        explosion(); // ³��������� ���� ������
    }

    private void beep()
    {
        if (audioSource != null && beep_audio != null)
        {
            audioSource.PlayOneShot(beep_audio);
        }
    }

    private void explosion()
    {
        if (audioSource != null && explosion_audio != null)
        {
            audioSource.PlayOneShot(explosion_audio);
        }
    }

    private void FindEnemy()
    {
        _enemy.Clear(); // ������� HashSet
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (IsValidTarget(hitCollider))
            {
                _enemy.Add(hitCollider.gameObject);
            }
        }
    }

    // ��������, �� ��'��� �������� �� ��������� ���� �� �� � ��������
    private bool IsValidTarget(Collider2D collider)
    {
        return ((1 << collider.gameObject.layer) & targetLayer) != 0 && !collider.isTrigger;
    }
}
