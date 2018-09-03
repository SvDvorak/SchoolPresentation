using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public Transform Target;
    public float DampTime = 0.15f;

    private Vector3 _velocity = Vector3.zero;

    public void Update()
    {
        if (Target)
        {
            var point = Camera.main.WorldToViewportPoint(Target.position);
            var delta = Target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            var destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, DampTime);
        }
    }
}