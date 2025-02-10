using UnityEngine;
using DG.Tweening;

public class GateManipulator : MonoBehaviour
{
    [SerializeField] private GameObject l_door;
    [SerializeField] private GameObject r_door;
    [SerializeField] private OpenType openType = OpenType.Horizontal;
    [SerializeField] private float openDistance = 2f; // Відстань відкриття воріт
    [SerializeField] private float openDuration = 1f; // Тривалість анімації відкриття
    [SerializeField] private Ease openEaseType = Ease.OutQuad; // Тип пом'якшення анімації

    private Vector3 l_doorClosedPosition;
    private Vector3 r_doorClosedPosition;
    private Vector3 l_doorOpenPosition;
    private Vector3 r_doorOpenPosition;
    enum OpenType
    {
        Horizontal,
        Vertical,
        Rotation
    }

    private void Start()
    {
        // Збереження початкових позицій воріт
        l_doorClosedPosition = l_door.transform.position;
        r_doorClosedPosition = r_door.transform.position;
    }

    public void OpenGate()
    {
        // Обчислення позицій відкритих воріт
        switch (openType)
        {
            case OpenType.Horizontal:
                l_doorOpenPosition = l_doorClosedPosition + new Vector3(-openDistance, 0, 0);
                r_doorOpenPosition = r_doorClosedPosition + new Vector3(openDistance, 0, 0);
                break;
            case OpenType.Vertical:
                l_doorOpenPosition = l_doorClosedPosition + new Vector3(0, -openDistance, 0);
                r_doorOpenPosition = r_doorClosedPosition + new Vector3(0, openDistance, 0);
                break;
            case OpenType.Rotation:
                l_doorOpenPosition = l_doorClosedPosition + new Vector3(-openDistance, 0, 0);
                r_doorOpenPosition = r_doorClosedPosition + new Vector3(openDistance, 0, 0);
                break;
        }


    // Анімація лівої та правої частин воріт
    l_door.transform.DOMove(l_doorOpenPosition, openDuration)
            .SetEase(openEaseType);

        r_door.transform.DOMove(r_doorOpenPosition, openDuration)
            .SetEase(openEaseType);
    }

    public void CloseGate()
    {
        // Повернення воріт до початкової позиції
        l_door.transform.DOMove(l_doorClosedPosition, openDuration)
            .SetEase(openEaseType);

        r_door.transform.DOMove(r_doorClosedPosition, openDuration)
            .SetEase(openEaseType);
    }
}