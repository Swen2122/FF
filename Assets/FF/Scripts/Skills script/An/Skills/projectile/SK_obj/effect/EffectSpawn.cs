using UnityEngine;

[CreateAssetMenu(fileName = "NewEffectSpawn", menuName = "Effect/EffectSpawn")]
public class EffectSpawn : ProjectileEffectBase
{
    public GameObject effectPrefab;
    public override void OnHit(Collider2D other)
    {
    }
    public override void SpawnEffect(Vector3 position)
    {
        if (effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);
            Destroy(effectInstance, 10f);
        }
        else
        {
            Debug.LogWarning("Effect prefab is not assigned in the EffectSpawn scriptable object.");
        }
    }

}
