using System.Collections.Generic;
using UnityEngine;

public class TransformToObject : MonoBehaviour
{
	public GameObject CurrentVisual;
	private readonly List<Collider> _closeObjects = new List<Collider>();

	public void Update()
	{
	    if (Input.GetButtonUp("Fire1"))
	    {
		    var closestDistance = float.MaxValue;
		    Collider closestObject = null;
		    foreach (var closeObject in _closeObjects)
		    {
			    var distance = Vector3.Distance(closeObject.transform.position, transform.position);

				if (distance < closestDistance)
			    {
				    closestDistance = distance;
				    closestObject = closeObject;
			    }
		    }

		    if (closestObject != null)
		    {
				Destroy(CurrentVisual);
			    var parentRigidbody = transform.parent.GetComponent<Rigidbody>();
			    parentRigidbody.MovePosition(parentRigidbody.position + new Vector3(0, closestObject.bounds.center.y * closestObject.transform.localScale.y * 0.5f, 0));
			    CurrentVisual = Instantiate(
					closestObject.gameObject,
					transform.position,
					transform.rotation,
					transform.parent);
			    CurrentVisual.transform.localScale = closestObject.transform.localScale;
			    CurrentVisual.GetComponent<MeshCollider>().convex = true;
			    var existingRigidbody = CurrentVisual.GetComponent<Rigidbody>();

				if(existingRigidbody != null)
					Destroy(existingRigidbody);
		    }
	    }
	}

	public void OnTriggerEnter(Collider other)
	{
		_closeObjects.Add(other);
	}

	public void OnTriggerExit(Collider other)
	{
		_closeObjects.Remove(other);
	}
}