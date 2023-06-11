using UnityEngine;

public class Player : MonoBehaviour
{
    Inventory inventory;
    public Transform head;
    public PlayerItemInteractor itemInteractor;
    public PlayerMovement movement;
    public MovementConfig movementConfig;
    private void Start()
    {
        if (!TryGetComponent(out movement))
        {
            Debug.LogWarning("No movement Script attached!");
        }
        else
        {
            movement.Initiate(movementConfig);
        }
    }

    void Update()
    {
        var res = itemInteractor.CheckForItem(head);
        if (res.Success)
        {
            var itemObject = res.Value;
            itemObject.PickMeUp(inventory);
        }
        
        if(movement != null)
        {
            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            movement.Move(axis,mouse);
        }

    }
}
