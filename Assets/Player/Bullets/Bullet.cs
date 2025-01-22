using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float damage;
    public float speed;
    public float lifeTime;
    private float currentLife;
    private Rigidbody rb;

    public void Shoot()
    {
        transform.parent = null;
        rb.AddForce(transform.up * speed, ForceMode.Impulse);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(isActiveAndEnabled) 
        { 
            currentLife += Time.deltaTime;
            if(currentLife >= lifeTime) 
            { 
                Destroy(rb);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Destroy()
    {
        rb.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
        transform.parent = BulletPool.Instance.transform;
        transform.localRotation = Quaternion.identity;
        transform.position = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Bullet triggered by: {other.gameObject.name}");
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Debug.Log($"Damage applied to: {other.gameObject.name}");
        }
        else
        {
            Debug.Log($"No EnemyHealth component found on: {other.gameObject.name}");
        }
        Destroy();
    }
}
