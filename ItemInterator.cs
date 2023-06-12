using UnityEngine;

public class ItemInterator : MonoBehaviour
{
    public LayerMask ItemLayerMask;
    public float ItemScanDistance = 5f;

    public Result<ItemObject> CheckForItem(Transform head)
    {
        Ray r = new Ray(head.position, head.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, ItemScanDistance, ItemLayerMask))
        {
            if (hit.collider != null)
            {
                ItemObject itemObject;
                if (hit.collider.gameObject.TryGetComponent(out itemObject))
                {
                    return new Result<ItemObject>(true, itemObject);
                }
            }
        }
        return new Result<ItemObject>(false, null);
    }
}
