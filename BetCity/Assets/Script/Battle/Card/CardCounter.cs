using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int counter = 0;

    void Start()
    {
        if (counterText == null)
        {
            counterText = GetComponentInChildren<TextMeshProUGUI>();
        }
        if (counterText == null)
        {
            Debug.LogError("Failed to get TextMeshProUGUI component on " + gameObject.name);
        }
    }

    public bool SetCounter(int _value)
    {
        counter += _value;
        CounterChange();
        if (counter == 0)
        {
            Destroy(gameObject);
            return false;
        }
        return true;
    }

    private void CounterChange()
    {
        if (counterText != null)
        {
            counterText.text = counter.ToString();
        }
    }
}