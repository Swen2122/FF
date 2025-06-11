using UnityEngine;

public enum Element
{
    None,
    Water,
    Earth,
    Fire,
    Wind,
    Electro,
    Ice
}
public class ElementSelect : MonoBehaviour
{
    public Element element;

    public Element GetElement()
    {
        return element;
    }
}