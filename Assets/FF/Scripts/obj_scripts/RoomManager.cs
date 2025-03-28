using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GateManager[] gates;
    [SerializeField] private BaseEnemyAI[] enemys;
    [SerializeField] private Collider2D triger;
    public RoomState roomState; //{ get; set; }
    private GameObject doors;
    private GameObject enemyDir;
    [SerializeField] private int enemyAmount;
    [SerializeField] private bool isOpen;
    private bool isEnable;

    void Start()
    {
        doors = transform.Find("Doors")?.gameObject;
        enemyDir = transform.Find("Enemys")?.gameObject;
        gates = doors.GetComponentsInChildren<GateManager>();
        enemys = enemyDir.GetComponentsInChildren<BaseEnemyAI>();
        enemyAmount = enemys.Length;
        isEnable = false; // Add this line
        Debug.Log($"Room initialized with {enemyAmount} enemies");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyAmount > 0) roomState = RoomState.Fight;
    }

    private void FixedUpdate()
    {
        switch (roomState)
        {
            case RoomState.Open:
                OpenRoom();
                EnableEnemy();
                break;
            case RoomState.Fight:
                CloseRoom();
                EnableEnemy();
                CalculateEnemy();
                break;
            case RoomState.Close:
                CloseRoom();
                DisableEnemy();
                break;
        }
    }

    private void CloseRoom()
    {
        if (isOpen) isOpen = false; else return;
        foreach (var door in gates)
        {
            door.Close();
        }
    }

    private void OpenRoom()
    {
        if (!isOpen) isOpen = true; else return;
        foreach (var door in gates)
        {
            door.Open();
        }
    }

    private void DisableEnemy()
    {
        if (isEnable)
        {
            isEnable = false;
            foreach (var enemy in enemys)
            {
                if (enemy != null)
                {
                    enemy.DisableAI();
                    Debug.Log($"Disabling enemy {enemy.name}");
                }
            }
        }
    }

    private void EnableEnemy()
    {
        if (!isEnable)
        {
            isEnable = true;
            foreach (var enemy in enemys)
            {
                if (enemy != null)
                {
                    enemy.EnableAI();
                    Debug.Log($"Enabling enemy {enemy.name}");
                }
            }
        }
    }

    private void CalculateEnemy()
    {
        enemys = enemyDir.GetComponentsInChildren<BaseEnemyAI>();
        enemyAmount = enemys.Length;
        Debug.Log($"Calculated enemy amount: {enemyAmount}");
        if (enemyAmount <= 0) roomState = RoomState.Open;
    }
}

public enum RoomState
{
    Open,
    Close,
    Fight
}