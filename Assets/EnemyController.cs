using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public float speed = 4f;
    LevelManager lm;

    // HP przeciwnika (np. 10 na start)
    public int hp = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void FixedUpdate()
    {
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            // Pobranie wartości obrażeń od broni (domyślnie np. 1)
            int damage = 1;
            WeaponDamage weapon = other.gameObject.GetComponent<WeaponDamage>();
            if (weapon != null)
            {
                damage = weapon.damage;
            }

            // Odejmowanie HP przeciwnika
            hp -= damage;
            Debug.Log(gameObject.name + " otrzymał " + damage + " obrażeń. Pozostałe HP: " + hp);

            // Jeśli HP spadnie do 0 lub poniżej, przeciwnik znika z gry
            if (hp <= 0)
            {
                lm.AddPoints(1); // Możesz zmodyfikować przyznawanie punktów według swoich zasad
                Destroy(gameObject);
            }
        }
    }
}
