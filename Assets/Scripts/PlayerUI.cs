using UnityEngine;
using TMPro;

// drag in inspector

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    
    void Start()
    {
        
    }

   // dont forget to tag Interactable on all object that needs to interact.
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
