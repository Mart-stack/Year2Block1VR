using UnityEngine;
using TMPro;

public class VRInventory : MonoBehaviour
{
    public static VRInventory Instance;

    [Header("Counts")]
    public int foodCount;
    public int waterCount;
    public int electricityCount;

    [Header("UI (optional)")]
    public TMP_Text foodText;
    public TMP_Text waterText;
    public TMP_Text electricityText;

    void Awake() => Instance = this;

    public void AddItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Food:        foodCount++; break;
            case ItemType.Water:       waterCount++; break;
            case ItemType.Electricity: electricityCount++; break;
        }
        Debug.Log($"Добавлен предмет: {type}");
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (foodText)        foodText.text        = $"Food: {foodCount}";
        if (waterText)       waterText.text       = $"Water: {waterCount}";
        if (electricityText) electricityText.text = $"Energy: {electricityCount}";
    }


}
