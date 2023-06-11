using UnityEngine;

public class Player : MonoBehaviour
{
    Inventory inventory;
    public Transform head;
    public PlayerItemInteractor itemInteractor;
    void Update()
    {
        var res = itemInteractor.CheckForItem(head);
        if (res.Success)
        {
            var itemObject = res.Value;
            itemObject.PickMeUp(inventory);
        }
    }
}
