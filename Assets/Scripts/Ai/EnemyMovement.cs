using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    [Header("Navigator options")]
    [SerializeField] float gridSize = 0.5f;
    [SerializeField] float speed = 0.05f;

    Pathfinder<Vector2> pathfinder;
    [SerializeField] LayerMask obstacles;
    [SerializeField] bool searchShortcut = false;
    [SerializeField] bool snapToGrid = false;
    Vector2 targetNode;
    List<Vector2> path;
    List<Vector2> pathLeftToGo = new List<Vector2>();
    [SerializeField] bool drawDebugLines;

    private bool isMoving = false;

    void Start()
    {
        pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000);
    }

    void Update()
    {
        if (isMoving && pathLeftToGo.Count > 0)
        {
            Vector3 dir = (Vector3)pathLeftToGo[0] - transform.position;
            transform.position += dir.normalized * speed;

            if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < speed * speed)
            {
                transform.position = pathLeftToGo[0];
                pathLeftToGo.RemoveAt(0);
            }

            if (pathLeftToGo.Count == 0)
            {
                isMoving = false;
            }
        }

        if (drawDebugLines)
        {
            for (int i = 0; i < pathLeftToGo.Count - 1; i++)
            {
                Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1]);
            }
        }
    }

    public void GetMoveCommand(Vector2 target)
    {
        Vector2 closestNode = GetClosestNode(transform.position);
        if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path))
        {
            pathLeftToGo = searchShortcut && path.Count > 0 ? ShortenPath(path) : new List<Vector2>(path);
            if (!snapToGrid) pathLeftToGo.Add(target);
            isMoving = true;
        }
    }

    Vector2 GetClosestNode(Vector2 target)
    {
        return new Vector2(Mathf.Round(target.x / gridSize) * gridSize, Mathf.Round(target.y / gridSize) * gridSize);
    }

    float GetDistance(Vector2 A, Vector2 B)
    {
        return (A - B).sqrMagnitude;
    }

    Dictionary<Vector2, float> GetNeighbourNodes(Vector2 pos)
    {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;

                Vector2 dir = new Vector2(i, j) * gridSize;
                if (!Physics2D.Linecast(pos, pos + dir, obstacles))
                {
                    neighbours.Add(GetClosestNode(pos + dir), dir.magnitude);
                }
            }
        }
        return neighbours;
    }

    List<Vector2> ShortenPath(List<Vector2> path)
    {
        List<Vector2> newPath = new List<Vector2>();

        for (int i = 0; i < path.Count; i++)
        {
            newPath.Add(path[i]);
            for (int j = path.Count - 1; j > i; j--)
            {
                if (!Physics2D.Linecast(path[i], path[j], obstacles))
                {
                    i = j;
                    break;
                }
            }
            newPath.Add(path[i]);
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }

    public void StopMoving()
    {
        pathLeftToGo.Clear();
        isMoving = false;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
