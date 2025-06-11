using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TestShader2DSprite : MonoBehaviour
{
public ComputeShader computeShader;
public int textureSize = 256;
public float speed = 1.0f;
private RenderTexture rt;
private Texture2D tex2D;
private SpriteRenderer spriteRenderer;
private int kernelID;

void Awake()
{
    spriteRenderer = GetComponent<SpriteRenderer>();
}

void Start()
{
    // Create and configure RT
    rt = new RenderTexture(textureSize, textureSize, 0)
    {
        enableRandomWrite = true,
        graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R32G32B32A32_SFloat
    };
    rt.Create();

    // Prepare Compute Shader
    kernelID = computeShader.FindKernel("CSMain");
    computeShader.SetTexture(kernelID, "Result", rt);
    computeShader.SetInts("Resolution", textureSize, textureSize);

    // Create CPU Texture2D for sprite
    tex2D = new Texture2D(textureSize, textureSize, TextureFormat.RGBAFloat, false);

    // Generate initial sprite
    UpdateSpriteTexture();
}

void Update()
{
    // Dispatch compute shader each frame
    computeShader.SetFloat("Time", Time.time * speed);
    int tg = Mathf.CeilToInt(textureSize / 8f);
    computeShader.Dispatch(kernelID, tg, tg, 1);

    // Copy RT to Texture2D and update sprite
    UpdateSpriteTexture();
}

void UpdateSpriteTexture()
{
    RenderTexture prev = RenderTexture.active;
    RenderTexture.active = rt;
    tex2D.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0);
    tex2D.Apply();
    RenderTexture.active = prev;

    // Assign Texture2D to sprite
    Sprite spr = Sprite.Create(tex2D, new Rect(0, 0, textureSize, textureSize),
        new Vector2(0.5f, 0.5f), textureSize);
    spriteRenderer.sprite = spr;
    spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
}

void OnDestroy()
{
    if (rt != null) rt.Release();
}
}