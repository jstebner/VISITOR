using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visitor : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 200f;

    public void Damage(float damage)
    {
        Debug.Log("Damaged");
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
