using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public void Interact(GameObject player)
    {
        PlayerPickup pickup = player.GetComponent<PlayerPickup>();

        if (pickup != null && pickup.hasKey)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("Door is locked. Need a key.");
        }
    }

    void OpenDoor()
    {
        Debug.Log("Door opened!");
    }
}