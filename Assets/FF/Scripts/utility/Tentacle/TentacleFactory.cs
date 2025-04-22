using UnityEngine;
using System.Collections.Generic;

public class TentacleFactory : MonoBehaviour
{
    public static TentacleFactory Instance { get; private set; }
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
    }
    private List<TentacleGenerator> tentacles = new List<TentacleGenerator>();
    public TentacleGenerator CreateTentacle(TectacleSO tentacleSettings, Transform start, Transform end, float targetDistance, Camera maincamera)
    {
        GameObject tentacleObj = new ("Tentacle");
            tentacleObj.transform.SetParent(transform);
        LineRenderer lineRenderer = tentacleObj.AddComponent<LineRenderer>();
        LineRendererChange(tentacleObj.GetComponent<LineRenderer>(), tentacleSettings.lineRendererSettings);
        TentacleGenerator tentacle = tentacleObj.AddComponent<TentacleGenerator>();
        tentacle.Initialize(tentacleSettings.tentacleCount, lineRenderer, start, end, targetDistance, maincamera);
        tentacles.Add(tentacle);
        return tentacle;
    }
    private void LineRendererChange(LineRenderer lineRenderer, LineRendererSettings settings)
    {
        lineRenderer.material = settings.lineMaterial;
        lineRenderer.widthCurve = settings.widthCurve;
        lineRenderer.colorGradient = settings.colorGradient;
        lineRenderer.numCornerVertices = settings.cornerVertices;
        lineRenderer.numCapVertices = settings.endCapVertices;
        lineRenderer.startWidth = settings.startWidth;
        lineRenderer.endWidth = settings.endWidth;
        lineRenderer.useWorldSpace = settings.useWorldSpace;
        lineRenderer.alignment = settings.alignment;
        lineRenderer.sortingLayerName = "obj";
        lineRenderer.sortingOrder = 0;
    }
    private void Update()
    {
        if (tentacles.Count == 0) return;
        for (int i = tentacles.Count - 1; i >= 0; i--)
        {
            if (tentacles[i] == null || tentacles[i].lineRenderer == null || tentacles[i].end == null || tentacles[i].start == null)
            {
                Destroy(tentacles[i].gameObject);
                tentacles.RemoveAt(i);
            }
            else
            {
               tentacles[i].UpdateTentacle();
            }
        }
    }
    public void RewriteAllStart(Transform start)
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].start = start;
        }
    }
    public void RewriteStart(TentacleGenerator tentacle, Transform start)
    {
        tentacle.start = start;
    }
    public void RewriteEnd(TentacleGenerator tentacle, Transform end)
    {
        tentacle.end = end;
    }
}