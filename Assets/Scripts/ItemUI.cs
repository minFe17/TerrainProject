using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Text _text;

    public void Init(Item item)
    {
        _image.sprite = item._sprite;
        _text.text = item._count.ToString();
    }
}
