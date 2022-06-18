using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BulletController : MonoBehaviour
{
    BulletController thisBullet;
    int damage;
    float punchPower;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        thisBullet = GetComponent<BulletController>();
    }


    public void SetAttackPower(int damage, float punchPower)
    {
        this.damage = damage;
        this.punchPower = punchPower;
    }

    void GoBackToPool()
    {
        if (!gameObject.activeInHierarchy) return;
        BulletPooling.Instance.GetBulletBackToPool(thisBullet);
        transform.parent = BulletPooling.Instance.transform;
        rb.velocity = Vector3.zero;
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        gameObject.SetActive(false);
    }


    void ThrowBack(GameObject zomby)
    {
        NavMeshAgent agent = zomby.GetComponent<NavMeshAgent>();
        agent.Move(-zomby.transform.forward.normalized * punchPower);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ZombyController>())
        {
            other.GetComponent<ZombyController>().ReceiveDamage(damage);

            ThrowBack(other.gameObject);
        }
        GoBackToPool();
    }
}