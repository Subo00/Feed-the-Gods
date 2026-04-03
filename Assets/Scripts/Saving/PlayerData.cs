using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public Vector3 rotation;

    // Inventory: item id -> quantity
    public SerializableDictionary<uint, uint> itemsStack;

    public PlayerData()
    {
        position = Vector3.zero;
        rotation = Vector3.zero;
        itemsStack = new SerializableDictionary<uint, uint>();
    }
}
