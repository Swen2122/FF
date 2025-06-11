using System.Collections.Generic;
using UnityEngine;

public class HandsAnimationManager : MonoBehaviour
{
    public HandAnimation animatedObject;
    public int handsCount = 2;
    public List<HandAnimation> animatedObjects = new List<HandAnimation>();
    public Transform centerPosition;
    public Vector3 centerOffset;
    public float radius = 1.0f;
    private Vector3 center;
    private void Start()
    {
        center = centerPosition.position + centerOffset;
        for (int i = 0; i < handsCount; i++)
        {
            HandAnimation animated = Instantiate(animatedObject, transform);
            float angle = 2 * Mathf.PI * i / handsCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Vector3 handPosition = center + offset;
            animated.Inicialize(handPosition);
            animatedObjects.Add(animated);
        }
    }
    private void Update()
    {
        center = centerPosition.position + centerOffset;
        for (int i = 0; i < animatedObjects.Count; i++)
        {
            float angle = 2 * Mathf.PI * i / handsCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Vector3 handPosition = center + offset;
            animatedObjects[i].Inicialize(handPosition);
            animatedObjects[i].Animate(Time.deltaTime);
        }
    }
    private int nextHandIndex = 0;

    public void AnimeteTo(Vector3 targetPosition)
    {
        if (animatedObjects.Count == 0) return;
        var hand = animatedObjects[nextHandIndex];
        hand.OnAttackComplete = OnHandAttackComplete;
        hand.StartAnimation(targetPosition);
    }

    private void OnHandAttackComplete(HandAnimation hand)
    {
        nextHandIndex++;
        if (nextHandIndex >= animatedObjects.Count)
            nextHandIndex = 0;
    }

}
