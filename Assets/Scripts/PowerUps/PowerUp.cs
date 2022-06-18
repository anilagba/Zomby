using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpTypeEnum type;
    [SerializeField] float lifeTime;


    private void Start()
    {
        StartCoroutine(StartCountDown(lifeTime));
    }


    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 120f * Time.deltaTime);
    }




    IEnumerator StartCountDown(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        PowerUpManager.Instance.ActivatePowerUP(type, other.GetComponent<PlayerController>());
        Destroy(gameObject);
    }
}