using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element //список елементів
{
    None,
    Water,
    Earth,
    Fire,
    Wind,
    Electro,
    Ice
}

public class Element_select : MonoBehaviour
{
    public Element element;

    public Element GetElement()
    {
        return element;
    }
}