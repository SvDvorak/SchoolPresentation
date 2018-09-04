using UnityEngine;

public class moveStairs : MonoBehaviour
{
    public Animator Doors;

    private bool _move;
    private Vector3 _startPos;
    private float _moveTime = 4f;
    private float _delay = 2f;
    private float _endTime;
    private Vector3 _targetPos;

    public void Update()
    {
        if (_move)
        {
            var ratioLeft = Mathf.Clamp((_endTime - Time.time) / _moveTime, 0, 1);
            var ratioDone = 1 - ratioLeft;
            var shake = Random.onUnitSphere * Mathf.Min(ratioLeft, 0.2f) * 10 * Time.deltaTime;
            var movePos = Vector3.Lerp(_startPos, _targetPos, ratioDone);
            transform.position = shake + movePos;

            if (Mathf.Approximately(ratioDone, 1))
            {
                Doors.SetTrigger("Open");
            }
        }
    }

    public void ActivateMoveStairs()
    {
        _move = true;
        _startPos = transform.position;
        _targetPos = new Vector3(transform.position.x, 0, transform.position.z);
        _endTime = Time.time + _moveTime + _delay;
    }
}
