using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime = 10f;  

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
