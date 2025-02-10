using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    // Info/Text to appear in the name/dialogue panel
    [Header("NPC Info")]
    public string npcName;
    [TextArea(3, 5)] public string npcDialogue;

    // Target for info/text
    [Header("UI Elements")]
    public GameObject dialogueUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the NPC was approached by the player and not a different object
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player has gone out of range
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Hide the interaction panel
            CloseDialogue();
        }
    }

    private void Update()
    {
        // Check if the player is within range and has pressed the interaction button
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Show the interaction panel
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        // Show the interaction panel
        dialogueUI.SetActive(true);
        nameText.text = npcName;
        dialogueText.text = npcDialogue;
    }

    private void CloseDialogue()
    {
        // Hide the interaction panel
        dialogueUI.SetActive(false);
    }
}
