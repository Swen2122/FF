using UnityEngine;

public class Rotate
{
    public void RotateToTarget(GameObject gameObject, Vector3 target)
    {
        Vector3 direction = target - gameObject.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotation, Time.deltaTime * 5f);
    }
}
