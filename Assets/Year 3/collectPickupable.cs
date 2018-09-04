using System.Collections.Generic;
using UnityEngine;

public class collectPickupable : MonoBehaviour
{
    public int ExpectedCount;
    public int Count;
    public moveStairs Stairs;

    private HashSet<Rigidbody> _collectedItems;
    private int _pickupableLayer;

    public void Start()
    {
        _collectedItems = new HashSet<Rigidbody>();
        _pickupableLayer = LayerMask.NameToLayer("Pickupable");
    }

    public void Update()
    {
        Count = _collectedItems.Count;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (IsPickupable(other))
        {
            _collectedItems.Add(other.attachedRigidbody);

            if (_collectedItems.Count == ExpectedCount)
            {
                Stairs.ActivateMoveStairs();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (IsPickupable(other))
        {
            _collectedItems.Remove(other.attachedRigidbody);
        }
    }

    private bool IsPickupable(Collider other)
    {
        return other.gameObject.layer == _pickupableLayer;
    }
}
