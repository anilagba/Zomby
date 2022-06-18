using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : Simpleton<BulletPooling>
{
    [SerializeField] BulletController bulletPrefab;
    [SerializeField] int bulletCount;

    Queue<BulletController> bullets = new Queue<BulletController>();

    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            CreateNewBullet();
        }
    }

    private void Update()
    {
        bulletCount = bullets.Count;
    }


    public BulletController GetBulletFromPool()
    {
        BulletController bullet;
        if (bullets.Count > 0)
        {
            bullet = bullets.Dequeue();
            bullet.gameObject.SetActive(true);
        }
        else
        {
            CreateNewBullet();
            bullet = bullets.Dequeue();
            bullet.gameObject.SetActive(true);
        }
        return bullet;
    }


    void CreateNewBullet()
    {
        BulletController newBullet = Instantiate(bulletPrefab);
        newBullet.gameObject.SetActive(false);
        newBullet.transform.parent = transform;
        bullets.Enqueue(newBullet);
    }


    public void GetBulletBackToPool(BulletController bullet)
    {
        bullets.Enqueue(bullet);
    }
}