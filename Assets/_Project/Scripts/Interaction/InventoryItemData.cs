using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public Sprite icon;
    public TextAsset inkJSON;
    public bool isReadable;

    public InventoryItemData(string name, Sprite sprite, TextAsset ink, bool readable)
    {
        itemName = name;
        icon = sprite;
        inkJSON = ink;
        isReadable = readable;
    }
}