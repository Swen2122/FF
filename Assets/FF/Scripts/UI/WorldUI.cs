using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class WorldUI : MonoBehaviour
{
    public JawUI jawPrefab;
    public Transform jawParent;
    private ObjectPool<JawUI> jawPool;
    private List<Transform> enemys = new List<Transform>();
    public static WorldUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        jawPool = new ObjectPool<JawUI>(jawPrefab, jawParent);
        for (int i = 0; i < 10; i++)
        {
            var jaw = GameObject.Instantiate(jawPrefab, jawParent);
            jaw.gameObject.SetActive(false);
            jawPool.Release(jaw);
        }

    }
    public void TryShowJaw(Transform enemy, float maxDistance)
    {
        if ((Vector3.Distance(enemy.position, Controler.Instance.transform.position) > maxDistance) || enemys.Contains(enemy))
        return;
        var jaw = jawPool.Get();
        enemys.Add(enemy);
        jaw.transform.localScale = Vector3.one * 0.3f;
        jaw.SetTarget(enemy, jawPool);
    }

}
