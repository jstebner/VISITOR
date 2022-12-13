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
    public Transform bulletDirection;
    public AudioSource gunShotSoundEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        playerShoot.shootInput += Shoot;
        playerShoot.reload += StartReload;
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
        if (currentAmmo > 0 && CanShoot()) {
            if (Physics.Raycast(bulletDirection.position, bulletDirection.forward, out RaycastHit hitInfo, maxDistance)) {
                Debug.Log(hitInfo.transform.name);
                IDamageable target = hitInfo.transform.GetComponent<IDamageable>();
                target?.Damage(damage);
            }
            currentAmmo--;
            timeSinceLastShot = 0;
            gunShotSoundEffect.Play();
        }
    }
}
