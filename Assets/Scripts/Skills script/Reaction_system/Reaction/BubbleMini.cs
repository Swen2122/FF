using UnityEngine;
using System.Collections.Generic;
public class BubbleMini : MonoBehaviour
{
    public GameObject bubble;
    private Queue<GameObject> bubbles = new Queue<GameObject>();
    public int maxBubbles = 50;
    void Start()
    {
        for (int i = 0; i < maxBubbles; i++)
        {
            GameObject b = Instantiate(bubble);
            b.SetActive(false);
            bubbles.Enqueue(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CreateBubble(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
    public void CreateBubble(Vector3 position)
    {
        if (bubbles.Count > 0)
        {
            GameObject b = bubbles.Dequeue();
            position.z = 0;
            b.transform.position = position;
            b.SetActive(true);
        }
    }
    private void ReturnBubble(GameObject b)
    {
        b.SetActive(false);
        bubbles.Enqueue(b);
    }
}
