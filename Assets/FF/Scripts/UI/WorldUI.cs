using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class WorldUI : MonoBehaviour
{
    public JawUI jawPrefab;
    public Image healthBarImage;
    public Transform parent;
    private ObjectPool<JawUI> jawPool;
    ObjectPool<Image> HealtBarPool;
    private List<Transform> enemysJaw = new List<Transform>();
    private List<Transform> enemysBar = new List<Transform>();
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
        jawPool = new ObjectPool<JawUI>(jawPrefab, parent);
        for (int i = 0; i < 10; i++)
        {
            var jaw = GameObject.Instantiate(jawPrefab, parent);
            jaw.gameObject.SetActive(false);
            jawPool.Release(jaw);
        }
        HealtBarPool = new ObjectPool<Image>(healthBarImage, parent);
        for (int i = 0; i < 10; i++)
        {
            var healthBar = GameObject.Instantiate(healthBarImage, parent);
            healthBar.gameObject.SetActive(false);
            HealtBarPool.Release(healthBar);
        }

    }
    public void TryShowJaw(Transform enemy, float maxDistance)
    {
        if ((Vector3.Distance(enemy.position, Controler.Instance.transform.position) > maxDistance) || enemysJaw.Contains(enemy))
        return;
        var jaw = jawPool.Get();
        enemysJaw.Add(enemy);
        jaw.transform.localScale = Vector3.one * 0.5f;
        jaw.SetTarget(enemy, jawPool);
    }
    public void TryShowHealthBar(Transform enemy, float maxDistance)
    {
        if ((Vector3.Distance(enemy.position, Controler.Instance.transform.position) > maxDistance) || enemysBar.Contains(enemy))
            return;
        var healthBar = HealtBarPool.Get();
        enemysBar.Add(enemy);
        healthBar.transform.localScale = Vector3.one * 1f;
        enemy.GetComponent<Health>().HP_bar = healthBar;
        healthBar.transform.position = enemy.position + new Vector3(0, 1f, 0);
    }

}
