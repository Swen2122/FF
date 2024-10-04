using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Element_use : MonoBehaviour
{
        public Image icon;
        public Sprite waterSprite;
        public Sprite earthSprite;
        public Sprite fireSprite;
        public Sprite windSprite;
    public void OnElementSelected(Element element)
        {
        Debug.Log("OnElementSelected метод викликано з елементом: " + element);
        switch (element)
        {
            case Element.Water:
                icon.sprite = waterSprite;
                break;
            case Element.Earth:
                icon.sprite = earthSprite;
                break;
            case Element.Fire:
                icon.sprite = fireSprite;
                break;
            case Element.Wind:
                icon.sprite = windSprite;
                break;
        }
        Debug.Log("Іконка змінена на:"+element);
        }

}
