using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public GameObject playerObject;

    private string text;
    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        text = "Items in inventory: 0";
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.SetText(text);
    }

    void Update()
    {
        UpdateInventoryText(playerObject.GetComponent<Inventory>().Size());
    }

    public void UpdateInventoryText(int numItems)
    {
        text = "Items in inventory: " + numItems;
        tmp.SetText(text);
    }
}
