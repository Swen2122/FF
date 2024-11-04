using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Skill_System : MonoBehaviour
{   
    [Header("Sound")]
    [SerializeReference] private AudioSource audioSource;
    [SerializeReference] private AudioClip water_sound;
    [SerializeReference] private AudioClip fire_sound;
    [SerializeReference] private AudioClip earth_sound;
    [SerializeReference] private AudioClip wind_sound;
    [SerializeField] private AudioMixer audioMixer;
    private float currentPitch = 0.75f;
    [Header("Animation")]
    [SerializeReference] private Animator anim;
    [Header("Skills")]
    [SerializeReference] private M2Skill m2;
    public Element_use elementUseScript;
    public Element_use elementM2;
    [Header("Atack")]
    private HashSet<GameObject> _enemy = new HashSet<GameObject>();  // HashSet ��� ��������� ��������
    public Collider2D MeleeAttack;  // �������� ��� ���������� ���� �����
    [Header("��� �� ���� �������� �����")]
    public LayerMask targetLayer;
    [Header("Utility")]
    [SerializeReference] private Pause pause;
    void Update()
    {   
        if (Input.GetMouseButtonDown(0))  // ˳�� ������ ���� ��� �����
        {
            _enemy = FindUtility.FindEnemy(MeleeAttack, targetLayer);  // ������ ��� ������ � ���
            Element element = elementUseScript.currentElement;
            switch (element)
            {
                case Element.Water:
                    anim.SetTrigger("water_atk");
                    Damage.Water(new List<GameObject>(_enemy).ToArray(), -5);  // ��������� ����� ����� � �������� ������
                    break;
                case Element.Earth:
                    Damage.Earth(new List<GameObject>(_enemy).ToArray(), 15);
                    break;
                case Element.Fire:
                    Damage.Fire(new List<GameObject>(_enemy).ToArray(), 20);
                    break;
                case Element.Wind:
                    Damage.Wind(new List<GameObject>(_enemy).ToArray(), 10);
                    foreach (GameObject enemy in _enemy)
                    {
                        // �������� Rigidbody2D ������ ��� ������������ ������
                        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                        // ���� ����� �� Rigidbody2D
                        if (enemyRb != null)
                        {
                            // ������������� ����� Push ��� ������������� ������
                            PushUtility.Push(enemyRb, transform.position, 5f);
                        }
                    }
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
                    PitchChanger.ChangePitch(audioMixer , ref currentPitch, 0.05f);
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
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
