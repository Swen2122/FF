using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Aoiti.Pathfinding;

public class PathfindingMovement : MonoBehaviour
{
    public Transform target; // Цільовий об'єкт
    public float speed = 5f;
    public Material lineMaterial; // Матеріал для лінії

    private Pathfinder<Vector3> pathFinder;
    private List<Vector3> path;
    private LineRenderer lineRenderer;


    void Start()
    {
        pathFinder = new Pathfinder<Vector3>(GetDistance, GetNeighbourNodes);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Оновлюємо ціль
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                target.position = hit.point;
                FindPath();
            }
        }

        // Візуалізація шляху (викликається кожного кадру)
        VisualizePath();
    }


    void FindPath()
    {
        if (pathFinder.GenerateAstarPath(transform.position, target.position, out path))
        {
            MoveAlongPath();
        }
        else
        {
            Debug.Log("Шлях не знайдено!");
            path = null; // Очистити шлях, якщо його не знайдено
            lineRenderer.positionCount = 0; // Очистити візуалізацію
        }
    }


    void MoveAlongPath()
    {
        if (path.Count > 0)
        {
            transform.DOMove(path[0], speed * Vector3.Distance(transform.position, path[0])).SetEase(Ease.Linear).OnComplete(() =>
            {
                path.RemoveAt(0);
                if (path.Count > 0)
                {
                    MoveAlongPath();
                }
            });
        }
    }

    // Функція для візуалізації шляху
    void VisualizePath()
    {
        if (path != null)
        {
            lineRenderer.positionCount = path.Count + 1; // +1 для поточної позиції
            lineRenderer.SetPosition(0, transform.position);
            for (int i = 0; i < path.Count; i++)
            {
                lineRenderer.SetPosition(i + 1, path[i]);
            }
        }
    }



    float GetDistance(Vector3 A, Vector3 B)
    {
        return Vector3.Distance(A, B); // Використовуємо Vector3.Distance замість sqrMagnitude
    }

    Dictionary<Vector3, float> GetNeighbourNodes(Vector3 pos)
    {
        Dictionary<Vector3, float> neighbours = new Dictionary<Vector3, float>();
        float gridSize = 1f; // Розмір клітинки сітки

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;

                Vector3 neighbour = pos + new Vector3(x * gridSize, 0, z * gridSize);
                neighbours.Add(neighbour, Vector3.Distance(pos, neighbour));

            }
        }
        return neighbours;
    }
}