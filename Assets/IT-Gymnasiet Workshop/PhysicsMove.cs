using UnityEngine;

public class PhysicsMove : MonoBehaviour
{
    public float Speed;
	public float JumpForce;
    public Transform FeetGround;

    private Quaternion _standardRotation;
	private Rigidbody _rigidbody;
	private float _jumpDelay;
	private float _jumpDelayIncrease = 0.5f;

	public void Start ()
	{
	    _standardRotation = transform.rotation;
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	public void Update ()
	{
        var speed = Speed;

		var isOnGround = Physics.Raycast(FeetGround.position + Vector3.up * 0.1f, Vector3.down, 0.3f, ~LayerMask.GetMask("TransformObject", "Player"));

		if (_jumpDelay <= 0.001f && isOnGround && Input.GetButton("Jump"))
	    {
			_rigidbody.AddForce(Vector3.up*JumpForce, ForceMode.Impulse);
		    _jumpDelay += _jumpDelayIncrease;
	    }

		_jumpDelay = Mathf.Max(_jumpDelay - Time.deltaTime, 0);

        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
	    if (movement.sqrMagnitude > 0.01f)
	    {
		    var isoRotation = Quaternion.Euler(0, -45, 0);
		    _rigidbody.MovePosition(_rigidbody.position + isoRotation*movement*Time.deltaTime*speed);

	        var newRotation = _standardRotation*isoRotation*Quaternion.LookRotation(-new Vector3(movement.x, 0, movement.z), Vector3.up);
            _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, newRotation, 0.2f));
        }
    }
}
