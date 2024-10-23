using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Aoiti.Pathfinding;

public class PathfindingMovement : MonoBehaviour
{
    public Transform target; // ֳ������ ��'���
    public float speed = 5f;
    public Material lineMaterial; // ������� ��� ��

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
            // ��������� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                target.position = hit.point;
                FindPath();
            }
        }

        // ³��������� ����� (����������� ������� �����)
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
            Debug.Log("���� �� ��������!");
            path = null; // �������� ����, ���� ���� �� ��������
            lineRenderer.positionCount = 0; // �������� ����������
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

    // ������� ��� ���������� �����
    void VisualizePath()
    {
        if (path != null)
        {
            lineRenderer.positionCount = path.Count + 1; // +1 ��� ������� �������
            lineRenderer.SetPosition(0, transform.position);
            for (int i = 0; i < path.Count; i++)
            {
                lineRenderer.SetPosition(i + 1, path[i]);
            }
        }
    }



    float GetDistance(Vector3 A, Vector3 B)
    {
        return Vector3.Distance(A, B); // ������������� Vector3.Distance ������ sqrMagnitude
    }

    Dictionary<Vector3, float> GetNeighbourNodes(Vector3 pos)
    {
        Dictionary<Vector3, float> neighbours = new Dictionary<Vector3, float>();
        float gridSize = 1f; // ����� ������� ����

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