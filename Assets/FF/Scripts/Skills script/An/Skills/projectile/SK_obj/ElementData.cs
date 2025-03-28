using UnityEngine;
using System.Collections.Generic;
// Дані елементів
[CreateAssetMenu(fileName = "New Element data", menuName = "Element/Element data")]
public class ElementData : ScriptableObject
{
    [Header("Element Info")]
    public Dictionary<Element, Sprite> elementSprites = new Dictionary<Element, Sprite>();
    // Масив для зберігання активних елементів
    public Element[] activeElements = new Element[4];
    // Посилання на спрайти через інспектор
    public Sprite waterSprite;
    public Sprite earthSprite;
    public Sprite fireSprite;
    public Sprite windSprite;
    public Sprite iceSprite;
    public Sprite electricSprite;
}