using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootInterval = 2f;      
    [SerializeField] float activationDelay = 4f;     
    [SerializeField] Collider2D sawCollider;
    [SerializeField] SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        StartCoroutine(DelayedActivation());
    }

    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(activationDelay + Random.Range(0f, 2f));

        StartCoroutine(ShootBullets());
    }

    IEnumerator ShootBullets()
    {
        while (true)
        {
            if (spriteRenderer.color.a == 1f)
            {
                ShootIn4D();
            }
            
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void ShootIn4D()
    {
        Vector2[] directions = {
            new Vector2(1, 1), new Vector2(-1, 1),
            new Vector2(1, -1), new Vector2(-1, -1)
        };

        foreach (Vector2 dir in directions)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletMovement bulletMovement = bulletInstance.GetComponent<BulletMovement>();
            bulletMovement._Direction = dir;
            Destroy(bulletInstance, 4f);  
        }
    }
}
