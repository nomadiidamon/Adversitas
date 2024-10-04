using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour, ITextDisplay
{
    [SerializeField] private TMP_Text textComponent;  // Reference to a TextMeshPro component
    [SerializeField] private string displayText;
    [SerializeField] private Transform displayPosition;

    public string DisplayText
    {
        get { return displayText; }
        set
        {
            displayText = value;
            ShowText();  // Automatically show the new text when it's set
        }
    }

    public void ShowText()
    {
        if (textComponent != null)
        {
            textComponent.text = displayText;  // Update the UI text
        }
        else
        {
            Debug.LogWarning("Text component is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowText();
    }
}
