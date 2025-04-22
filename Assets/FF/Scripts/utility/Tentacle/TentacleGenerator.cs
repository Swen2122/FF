using UnityEngine;
public class TentacleGenerator : MonoBehaviour
{
    public float smopthSpeed = 0.1f;
    public Transform start;
    public Transform end;
    public Vector3[] segmentPoses;
    public Vector3[] segmentV;
    public LineRenderer lineRenderer;
    private float targetDistance;
    private Camera mainCamera;
    public void Initialize(int length, LineRenderer lineRenderer, Transform start, Transform end, float targetDistance, Camera maincamera)
    {
        this.targetDistance = targetDistance;
        this.start = start;
        this.end = end;
        this.lineRenderer = lineRenderer;
        this.segmentPoses = new Vector3[length];
        this.mainCamera = maincamera;
        this.segmentV = new Vector3[length];
        lineRenderer.positionCount = length;
         for (int i = 0; i < length; i++)
        {
            float t = i / (float)(length - 1);
            segmentPoses[i] = Vector3.Lerp(start.position, end.position, t);
            segmentV[i] = Vector3.zero;
        }
        lineRenderer.SetPositions(segmentPoses);
    }

    // Метод для оновлення кожного об'єкта
    public void UpdateTentacle()
    {
        if (lineRenderer == null)
        {
            Debug.LogWarning("LineRenderer has been destroyed. Skipping UpdateTentacle.");
            return;
        }
        segmentPoses[0] = start.position;
        segmentPoses[^1] = end.position; // Останній елемент
        Vector3 direction = (segmentPoses[^1] - segmentPoses[0]) / (segmentPoses.Length - 1);
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPosition = segmentPoses[0] + direction * i;
            segmentPoses[i] = Vector3.SmoothDamp(
                segmentPoses[i],
                targetPosition,
                ref segmentV[i],
                smopthSpeed
            );
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}