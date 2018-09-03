using UnityEngine;

public class Move : MonoBehaviour
{
    public float Speed;
    public float SpeedBoost = 2;
    public Transform ForwardPoint;
    public float ForwardDistance;

    private Quaternion _standardRotation;

    public void Start ()
	{
	    _standardRotation = transform.rotation;
	}
	
	public void Update ()
	{
        var speed = Speed;

	    if (Input.GetButton("Jump"))
	    {
	        speed *= SpeedBoost;
	    }

        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	    if (movement.sqrMagnitude > 0.01f)
	    {
	        transform.Translate(movement*Time.deltaTime*speed, Space.World);

	        var newRotation = _standardRotation*Quaternion.LookRotation(-new Vector3(movement.x, 0, movement.y), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.2f);
        }

        ForwardPoint.position = transform.position + movement*ForwardDistance;
    }
}
