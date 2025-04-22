using UnityEngine;
using System.Collections.Generic;
using Aoiti.Pathfinding;
public class EnemyMovement : MonoBehaviour
{
    [Header("Navigator Options")]
    [SerializeField] float gridSize = 0.5f;
    [SerializeField] public float speed = 2f;
    [SerializeField] LayerMask obstacles;
    [SerializeField] bool searchShortcut = false;
    [SerializeField] bool snapToGrid = false;
    [SerializeField] bool drawDebugLines;
    [SerializeField] float updateInterval = 0.02f;
    [SerializeField] float pathUpdateThreshold = 0.5f;

    private Pathfinder<Vector2> pathfinder;
    private List<Vector2> pathLeftToGo = new List<Vector2>();
    private bool isMoving = false;
    private float distanceThreshold = 0.1f;
    private Vector2 currentTargetPosition;
    private bool shouldUpdatePath = false;

    public delegate void PathCompletedHandler();
    public event PathCompletedHandler OnPathCompleted;

    private void Start()
    {
        pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000);
        InvokeRepeating(nameof(MoveAlongPath), 0f, updateInterval);
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;

        if (drawDebugLines)
        {
            DrawPathDebug();
        }

        if (shouldUpdatePath && isMoving)
        {
            UpdatePathToTarget(currentTargetPosition);
        }
    }

    public void SetTarget(Vector2 target)
    {
        currentTargetPosition = target;
        float distanceToCurrentTarget = Vector2.Distance(target, currentTargetPosition);

        if (distanceToCurrentTarget > pathUpdateThreshold)
        {
            shouldUpdatePath = true;
        }
    }

    private void UpdatePathToTarget(Vector2 target)
    {
        shouldUpdatePath = false;
        Vector2 startNode = GetClosestNode(transform.position);
        Vector2 targetNode = GetClosestNode(target);

        List<Vector2> newPath;
        if (pathfinder.GenerateAstarPath(startNode, targetNode, out newPath))
        {
            Vector2 currentPos = transform.position;
            pathLeftToGo = searchShortcut ? ShortenPath(newPath) : new List<Vector2>(newPath);

            if (!snapToGrid)
            {
                pathLeftToGo.Add(target);
            }

            OptimizePathFromCurrentPosition(currentPos);
        }
    }

    private void OptimizePathFromCurrentPosition(Vector2 currentPos)
    {
        if (pathLeftToGo.Count > 0)
        {
            int nearestPointIndex = 0;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < pathLeftToGo.Count; i++)
            {
                float dist = Vector2.Distance(currentPos, pathLeftToGo[i]);
                if (dist < nearestDistance && !Physics2D.Linecast(currentPos, pathLeftToGo[i], obstacles))
                {
                    nearestDistance = dist;
                    nearestPointIndex = i;
                }
            }

            if (nearestPointIndex > 0)
            {
                pathLeftToGo.RemoveRange(0, nearestPointIndex);
            }
        }
    }

    private void MoveAlongPath()
    {
        if (!isMoving || pathLeftToGo.Count == 0)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = pathLeftToGo[0];

        float step = speed * updateInterval;
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);

        if (Vector2.Distance(transform.position, pathLeftToGo[0]) < distanceThreshold)
        {
            pathLeftToGo.RemoveAt(0);

            if (pathLeftToGo.Count == 0)
            {
                isMoving = false;
                OnPathCompleted?.Invoke();
            }
        }
    }

    public void GetMoveCommand(Vector2 target)
    {
        currentTargetPosition = target;

        if (!isMoving)
        {
            Vector2 startNode = GetClosestNode(transform.position);
            Vector2 targetNode = GetClosestNode(target);

            if (startNode == targetNode)
            {
                pathLeftToGo = new List<Vector2> { target };
                isMoving = true;
                return;
            }

            List<Vector2> path;
            if (pathfinder.GenerateAstarPath(startNode, targetNode, out path))
            {
                pathLeftToGo = searchShortcut && path.Count > 0 ?
                    ShortenPath(path) : new List<Vector2>(path);

                if (!snapToGrid)
                {
                    pathLeftToGo.Add(target);
                }

                isMoving = true;
            }
        }
        else
        {
            shouldUpdatePath = true;
        }
    }

    private Vector2 GetClosestNode(Vector2 target)
    {
        return new Vector2(
            Mathf.Round(target.x / gridSize) * gridSize,
            Mathf.Round(target.y / gridSize) * gridSize
        );
    }

    private float GetDistance(Vector2 A, Vector2 B)
    {
        return Vector2.Distance(A, B);
    }

    private Dictionary<Vector2, float> GetNeighbourNodes(Vector2 pos)
    {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;

                Vector2 direction = new Vector2(i, j) * gridSize;
                Vector2 neighborPos = pos + direction;

                if (!Physics2D.Linecast(pos, neighborPos, obstacles))
                {
                    neighbours.Add(GetClosestNode(neighborPos), direction.magnitude);
                }
            }
        }

        return neighbours;
    }

    private List<Vector2> ShortenPath(List<Vector2> path)
    {
        List<Vector2> newPath = new List<Vector2>();

        for (int i = 0; i < path.Count; i++)
        {
            newPath.Add(path[i]);

            for (int j = path.Count - 1; j > i; j--)
            {
                if (!Physics2D.Linecast(path[i], path[j], obstacles))
                {
                    i = j - 1;
                    break;
                }
            }
        }

        return newPath;
    }

    private void DrawPathDebug()
    {
        for (int i = 0; i < pathLeftToGo.Count - 1; i++)
        {
            Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1], Color.green);
        }
    }

    public void StopMoving()
    {
        pathLeftToGo.Clear();
        isMoving = false;
        shouldUpdatePath = false;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Vector2 GetCurrentTarget()
    {
        return pathLeftToGo.Count > 0 ? pathLeftToGo[0] : (Vector2)transform.position;
    }

    public Vector2 GetCurrentTargetPosition()
    {
        return currentTargetPosition;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(MoveAlongPath));
    }
}