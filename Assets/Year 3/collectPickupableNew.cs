using System.Collections.Generic;
using UnityEngine;

public class collectPickupableNew : MonoBehaviour
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
        // TODO FIX
    }

    public void OnTriggerExit(Collider other)
    {
        // TODO FIX
    }

    private bool IsPickupable(Collider other)
    {
        return other.gameObject.layer == _pickupableLayer;
    }
}
