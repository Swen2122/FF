using UnityEngine;
using System.Collections.Generic;

public class ShieldSegmentManager : MonoBehaviour
{
    public ReactionItem segmentPrefab;
    public Transform target; // Ціль, до якої будуть прив'язані сегменти
    public int numberOfSegments = 10;
    public float segmentminRadius = 1.0f;
    public float segmentSpacing = 0.5f;
    public float segmentRotationSpeed = 1.0f;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 2f;  
    public ObjectPool<ReactionItem> segmentPool; // Пул об'єктів для сегментів
    public List<Transform> segments = new List<Transform>(); // Змінено на список для динамічного управління сегментами
    private float radius; 
    private float currentAngle = 0f;
    private float timer = 0f;

    private void Awake()
    {
        segmentPool = new ObjectPool<ReactionItem>(segmentPrefab, target);
        for (int i = 0; i < numberOfSegments; i++)
        {
            var segment = GameObject.Instantiate(segmentPrefab, target);
            var altProjectile = segment.GetComponent<AltProjectile>();
            if (altProjectile != null) altProjectile.Initialize(this);
            segment.gameObject.SetActive(false);
            segmentPool.Release(segment);
            segments.Add(segmentPool.Get().transform); // Додаємо сегмент до списку
        }
    }

    private void Update()
    {
        if (target == null) return;
        radius = Mathf.Max(segmentminRadius, (float)numberOfSegments * segmentSpacing / (2 * Mathf.PI));
        currentAngle += segmentRotationSpeed * Time.deltaTime;
        float angleStep = Mathf.PI * 2f / numberOfSegments;
        float waveTime = Time.time * waveFrequency;
        segments.RemoveAll(s => s == null);
        for (int i = 0; i < segments.Count; i++)
        {
            float angle = currentAngle + i * angleStep;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            offset.y = Mathf.Sin(angle + waveTime) * waveAmplitude;
            segments[i].transform.position = target.position + offset;
        }
    }
    public void ReturnSphere(ReactionItem item)
    {
        StartCoroutine(Return(item)); 
    }
    private System.Collections.IEnumerator Return(ReactionItem item)
    {
        Debug.Log("Release: " + item.name);
        segmentPool.Release(item);
        yield return new WaitForSeconds(0.3f);
        var newitem = segmentPool.Get();
        Debug.Log("Get from pool: " + newitem.name);
        var altProjectile = newitem.GetComponent<AltProjectile>();
        if (altProjectile != null) altProjectile.Initialize(this);
        newitem.transform.parent = target;
        segments.Add(newitem.transform);
        Debug.Log("Segments count: " + segments.Count);
    }
}