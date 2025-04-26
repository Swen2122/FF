using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
public class BurstParticleEffectManager : MonoBehaviour
{
    public ParticleSystem ps;
    public VisualEffect vfx;
    public ObjectPool<VisualEffect> vfxPool;
    public static BurstParticleEffectManager Instance { get; private set; }
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
        vfxPool = new ObjectPool<VisualEffect>(vfx, transform);
        for (int i = 0; i < 10; i++)
        {
            var vfxInstance = Instantiate(vfx, transform);
            vfxInstance.gameObject.SetActive(false);
            vfxPool.Release(vfxInstance);
        }
    }
    public void PlayEffect(Vector2 position, Gradient color, float duration)
    {
        StartCoroutine(PlayEffectCoroutine(position, color, duration));
    }
    private IEnumerator PlayEffectCoroutine(Vector2 position, Gradient color, float duration)
    {
        var effect = vfxPool.Get();
        effect.SetVector3("position", (Vector3)position);
        effect.SetGradient("color", color);
        effect.Play();
        yield return new WaitForSeconds(duration);
        vfxPool.Release(effect);
    }
       
}
