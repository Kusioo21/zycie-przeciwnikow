using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 controllerInput;
    public float speed = 5f;
    //odniesienie do komponentu Rigidbody gracza
    Rigidbody rb;
    // Start is called before the first frame update
    public List<GameObject> enemies;
    public GameObject gun;
    public GameObject bulletSpawn;
    public GameObject swordHandle;
    public GameObject bulletPrefab; // poprawiona nazwa zmiennej 
    public GameObject shotgunBulletPrefab; // prefabrykat dla pocisków strzelby
    //referencja do komponentu LevelManager
    LevelManager lm;

    // Rodzaje broni które gracz może wybrać
    public enum WeaponType { Pistol, Shotgun } // ✅ Poprawiony zapis, dodano przecinek

    public WeaponType currentWeapon = WeaponType.Pistol;

    void Start()
    {
        //przypisujemy level manager ze sceny do zmiennej lm
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        //pobieramy odniesienie do komponentu Rigidbody
        rb = GetComponent<Rigidbody>();
        enemies = new List<GameObject>();
        InvokeRepeating("Shoot", 0, 2);
    }

    // Update jest wywoływany raz na klatkę
    void Update()
    {
        // Sprawdzamy czy gracz chce się ruszać i pobieramy dane z kontrolera
        controllerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Pobieramy wszystkich przeciwników w grze i sortujemy ich według odległości od gracza
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemies = enemies.OrderBy(enemy => Vector3.Distance(enemy.transform.position, transform.position)).ToList();

        // Jeśli przeciwnik jest wystarczająco blisko aktywujemy miecz i machamy nim
        if (enemies.Count > 0 && Vector3.Distance(transform.position, enemies[0].transform.position) < 2f)
        {
            swordHandle.SetActive(true);
            swordHandle.transform.Rotate(0, 2f, 0);
        }
        else
        {
            swordHandle.SetActive(false);
        }

        // Gracz może zmieniać broń używając klawiszy 1 i 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = WeaponType.Pistol;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeapon = WeaponType.Shotgun;
    }

    void FixedUpdate()
    {
        // Wyliczamy pozycję gracza po ruchu na podstawie jego wejścia
        Vector3 movementVector = new Vector3(controllerInput.x, 0, controllerInput.y);
        Vector3 targetPosition = transform.position + movementVector * Time.fixedDeltaTime * speed;
        rb.MovePosition(targetPosition);
    }

    void Shoot()
    {
        // Sprawdzamy czy jest przeciwnik do trafienia
        if (enemies.Count > 0)
        {
            // Celujemy broń w najbliższego przeciwnika
            gun.transform.LookAt(enemies[0].transform);

            // Wystrzeliwujemy odpowiedni pocisk zależnie od broni którą aktualnie trzyma gracz
            if (currentWeapon == WeaponType.Pistol)
            {
                FireBullet(bulletPrefab, 1, 0); // Jeden pocisk bez rozrzutu
            }
            else if (currentWeapon == WeaponType.Shotgun)
            {
                FireBullet(shotgunBulletPrefab, 5, 10f); // Strzelba wystrzeliwuje 5 pocisków z rozrzutem
            }
        }
    }

    void FireBullet(GameObject bulletPrefab, int bulletCount, float spreadAngle)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            // Tworzymy pocisk i ustawiamy jego kierunek
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, gun.transform.rotation);

            // Dodajemy losowy rozrzut dla strzałów ze strzelby
            bullet.transform.Rotate(UnityEngine.Random.Range(-spreadAngle, spreadAngle),
                                    UnityEngine.Random.Range(-spreadAngle, spreadAngle),
                                    0);

            // Nadajemy pociskowi siłę aby poleciał do przodu
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);

            // Po dwóch sekundach usuwamy pocisk żeby nie zapychać sceny
            Destroy(bullet, 2f);
        }

        Debug.Log("Strzał oddany!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Jeśli gracz zderzył się z przeciwnikiem dostaje obrażenia
        if (collision.gameObject.CompareTag("Enemy"))
        {
            lm.ReducePlayerHealth(5);
        }
    }
}
