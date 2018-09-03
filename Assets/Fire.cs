using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject Bullet;
    private Transform _bulletRoot;

    public void Start()
    {
        _bulletRoot = new GameObject("Bullets").transform;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var instance = Instantiate(Bullet);
            instance.transform.SetParent(_bulletRoot);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
        }
    }
}
