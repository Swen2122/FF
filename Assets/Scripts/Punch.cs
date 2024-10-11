using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{   
    [Header("Sound")]
    [SerializeReference] private AudioSource audioSource;
    [SerializeReference] private AudioClip water_sound;
    [SerializeReference] private AudioClip fire_sound;
    [SerializeReference] private AudioClip earth_sound;
    [SerializeReference] private AudioClip wind_sound;
    [Header("Animation")]
    [SerializeReference] private Animator anim;
    [Header("Skills")]
    [SerializeReference] private Malee_atk weapon;  // ���������, ���� �������� ��������� �����
    [SerializeReference] private M2Skill m2;
    public Element_use elementUseScript;
    public Element_use elementM2;

    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet ��� ��������� ��������
    [Header("Atack")]
    public Collider2D myCollider;  // �������� ��� ���������� ���� �����
    public string targetTag = "Enemy";  // ��� ��� ���������� ������

    void Update()
    {   
        if (Input.GetMouseButtonDown(0))  // ˳�� ������ ���� ��� �����
        {
            FindEnemy();  // ������ ��� ������ � ���
            Element element = elementUseScript.currentElement;
            switch (element)
            {
                case Element.Water:
                    anim.SetTrigger("water_atk");
                    weapon.Water(new List<GameObject>(_enemy).ToArray(), -5);  // ��������� ����� ����� � �������� ������
                    break;
                case Element.Earth:
                    weapon.Earth(new List<GameObject>(_enemy).ToArray(), 15);
                    break;
                case Element.Fire:
                    weapon.Fire(new List<GameObject>(_enemy).ToArray(), 20);
                    break;
                case Element.Wind:
                    weapon.Wind(new List<GameObject>(_enemy).ToArray(), 10);
                    break;
            }
        }
        if (Input.GetMouseButtonDown(1))  // ˳�� ������ ���� ��� �����
        {          
            Element element = elementM2.currentElement;
            switch (element)
            {
                case Element.Water:
                    PlaySound(water_sound);
                    m2.WaterM2(10f, 10);
                    break;
                case Element.Earth:
                    PlaySound(earth_sound);
                    m2.EarthM2(30f, 30);
                    break;
                case Element.Fire:
                    
                    break;
                case Element.Wind:
                    
                    break;
            }
        }
    }

    void FindEnemy()
    {
        // ������� HashSet ����� ����� �������
        _enemy.Clear();
        // ��������� �� ��������� � ��� ��������� ��'����
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // ���� �������� �������� ��'���� � ����� "Enemy"
            if (hitCollider.CompareTag(targetTag))
            {
                // ������ ��'��� �� HashSet (���������� ������������� �����������)
                _enemy.Add(hitCollider.gameObject);
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
