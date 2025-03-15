using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventoryButton
{
    public Button Button;
    public Image Image;
    public GameObject AmountImage;
    public TMP_Text AmountText;

    public InventoryButton(Button button, Image image, GameObject amountImage, TMP_Text amountText)
    {
        Button = button;
        Image = image;
        AmountImage = amountImage;
        AmountText = amountText;
    }
}
