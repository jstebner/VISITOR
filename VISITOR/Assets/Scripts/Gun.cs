using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;
    [SerializeField] private float currentAmmo;
    [SerializeField] private float magSize;
    [SerializeField] private float reloadTime;
    [SerializeField] private float fireRate;
    private bool reloading;
    private float timeSinceLastShot;
    private Transform bulletDirection;
    public AudioSource gunShotSoundEffect;

    void OnEnable() {
        playerShoot.shootInput += Shoot;
        playerShoot.reload += StartReload;
    }

    void OnDisable() {
        playerShoot.shootInput -= Shoot;
        playerShoot.reload -= StartReload;
    }

    public void StartReload() {
        if (!reloading) {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        Debug.Log("Reloading...");
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magSize;
        reloading = false;
        Debug.Log("Reloaded");
    }

    void Update() {
        timeSinceLastShot += Time.deltaTime;
    }

    private bool CanShoot() {
        return !reloading && timeSinceLastShot > 1f / (fireRate / 60f);
    }

    // Update is called once per frame
    private void Shoot()
    {
        Debug.Log("Gun Script Shoot Function");
        if (currentAmmo > 0 && CanShoot()) {
            Debug.Log("Gun Script Shot");
            bulletDirection = GameObject.Find("barrelDirection").transform;
            if (Physics.Raycast(bulletDirection.position, bulletDirection.forward, out RaycastHit hitInfo, maxDistance)) {
                Debug.Log($"Gun Script Hit {hitInfo.transform.name}");
                IDamageable target = hitInfo.transform.GetComponent<IDamageable>();
                target?.Damage(damage);
            }
            currentAmmo--;
            timeSinceLastShot = 0;
            gunShotSoundEffect.Play();
        }
    }
}
