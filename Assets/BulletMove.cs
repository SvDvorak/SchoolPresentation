using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed;

    private float _timeAlive;

	public void Update ()
    {
        transform.Translate(Vector3.back*Time.deltaTime*Speed);

	    if (_timeAlive > 5)
	    {
	        Destroy(gameObject);
	    }

	    _timeAlive += Time.deltaTime;
    }
}
