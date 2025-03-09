using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventoryButton : MonoBehaviour
{
    public Button Button;
    public Sprite Sprite;
    public TMP_Text Count;

    public InventoryButton(Button button, Sprite image, TMP_Text count)
    {
        Button = button;
        Sprite = image;
        Count = count;
    }
}
