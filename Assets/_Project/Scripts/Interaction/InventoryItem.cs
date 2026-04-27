using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public TextAsset inkJSON;

    public InventoryItem(string name, Sprite icon, TextAsset ink)
    {
        this.itemName = name;
        this.icon = icon;
        this.inkJSON = ink;
    }
}