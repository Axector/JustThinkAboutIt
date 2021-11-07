using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ground_Shooter : DefaultClass
{
    [SerializeField]
    private float shotForce;
    [SerializeField]
    private float angerDistance;
    [SerializeField]
    private float delayBeforeShot;
    [SerializeField]
    private GameObject shotPosition;
    [SerializeField]
    private GameObject shotBullet;

    protected Player player;
    private bool canShoot;
    protected bool seePlayer;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        canShoot = true;
        seePlayer = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Check if player is in anger distance
        if (distanceToPlayer < angerDistance) {
            seePlayer = true;
        }
    }

    protected virtual void FixedUpdate()
    {
        // Shoot if player is in anger distance
        if (seePlayer && canShoot) {
            canShoot = false;

            DoShot();
        }
    }

    private void DoShot()
    {
        // Create bullet
        GameObject bullet = Instantiate(
            shotBullet,
            shotPosition.transform.position,
            Quaternion.identity
        );

        // Give impulse to bullet
        bullet.GetComponent<Rigidbody2D>().AddForce(shotPosition.transform.up * shotForce, ForceMode2D.Impulse);

        // Wait for next shot
        StartCoroutine(WaitForShot());
    }

    private IEnumerator WaitForShot()
    {
        yield return new WaitForSeconds(delayBeforeShot);

        canShoot = true;
    }
}
