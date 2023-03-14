using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    [SerializeField] Sprite[] _coins;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _itemPrefabs; 
    List<Item> lstItem = new List<Item>();

    public void OpenOrClose()
    {
        _ui.SetActive(!_ui.activeSelf);
    }

    public void AddItem(Item item)
    {
        foreach (Sprite sp in _coins)
        {
            if(item._eType.ToString().Equals(sp.name))
                item._sprite = sp;
            //foreach (EItemType type in Enum.GetValues(typeof(EItemType)))
            //{
            //    if (type.ToString().Equals(sp.name))
            //    {
            //        item._sprite = sp;
            //    }
            //}
        }
        lstItem.Add(item);
        GameObject temp = Instantiate(_itemPrefabs, _content);
        temp.GetComponent<ItemUI>().Init(item);
    }
}

public enum EItemType
{
    None,
    Blue,
    Gold,
    Green,
    Purple,
    Red,
    Max,
}

public class Item
{
    public Sprite _sprite;
    public EItemType _eType;
    public int _count;
}
