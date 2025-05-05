using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private Collider2D interactZone;
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Collider2D[] collidersInTrigger = Physics2D.OverlapBoxAll(
                interactZone.bounds.center,
                interactZone.bounds.size,
                0);

            foreach (var collider in collidersInTrigger)
            {
                IInteractible interactible = collider.GetComponent<IInteractible>();
                if (interactible != null)
                {
                    interactible.Use();
                    Debug.Log($"Interacted with: {collider.gameObject.name}");
                }
            }
        }
    }
}
