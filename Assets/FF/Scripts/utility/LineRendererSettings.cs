using UnityEngine;

[CreateAssetMenu(fileName = "LineRendererSettings", menuName = "FF/Rendering/Line Renderer Settings")]
public class LineRendererSettings : ScriptableObject
{
    public Material lineMaterial;
    public AnimationCurve widthCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public Gradient colorGradient;
    public int cornerVertices = 5;
    public int endCapVertices = 5;
    public float startWidth = 0.1f;
    public float endWidth = 0.1f;
    public bool useWorldSpace = true;
    public LineAlignment alignment = LineAlignment.View;
}
