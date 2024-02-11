using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private List<GameObject> bulletPool;

    // Dependency Injection kullanarak BulletSettings nesnesini enjekte etmiyoruz çünkü artık singleton olarak erişilecek
    void Start()
    {
        // Bullet poolunu başlat
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    void Update()
{
    // Fire bullets
    if (Time.time >= BulletSettings.Instance.nextFireTime && GameManager.isGameStarted)
    {
        FireBullet();
        BulletSettings.Instance.nextFireTime = Time.time + 1f / BulletSettings.Instance.fireRate;
    }
    
    // Eğer fire rate pool size'tan büyükse, pool size'a eşitle
    if (BulletSettings.Instance.fireRate > poolSize)
    {
        AdjustPoolSize();
    }
}

// Pool size'ı fire rate'e eşitleme metodu
private void AdjustPoolSize()
{
    int difference = Mathf.CeilToInt(BulletSettings.Instance.fireRate) - poolSize;
    for (int i = 0; i < difference; i++)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.SetActive(false);
        bulletPool.Add(bullet);
    }
    poolSize += difference;
}

    void FireBullet()
    {
        GameObject gun = GameObject.Find("Gun");
        // Find an inactive bullet from the pool
        GameObject bullet = GetInactiveBullet();
        if (bullet != null)
        {
            bullet.transform.position = new Vector3(gun.transform.position.x, gun.transform.position.y, gun.transform.position.z + 2);
            bullet.SetActive(true);
        }
    }

    GameObject GetInactiveBullet()
    {
        // Find an inactive bullet from the pool
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    void FixedUpdate()
    {
        // Move active bullets
        foreach (GameObject bullet in bulletPool)
        {
            if (bullet.activeInHierarchy)
            {
                bullet.transform.Translate(Vector3.forward * BulletSettings.Instance.bulletSpeed * Time.fixedDeltaTime);

                // Check if bullet reaches maximum range
                if (bullet.transform.position.z >= GameObject.Find("Gun").transform.position.z + BulletSettings.Instance.bulletRange)
                {
                    bullet.SetActive(false);
                }
            }
        }
    }
}
