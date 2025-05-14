using UnityEngine;
using System.Collections.Generic;

public class AreaWeaponPrefab : MonoBehaviour
{
    public AreaWeapon weapon;
    private Vector3 targetSize;
    private float timer;
    public List<Enemy> enemiesInRange;
    private float counter;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapon = GameObject.Find("Area Weapon").GetComponent<AreaWeapon>();
        // Destroy(gameObject, weapon.duration);
        targetSize = Vector3.one * weapon.range;
        transform.localScale = Vector3.zero;
        timer = weapon.duration;
    }

    // Update is called once per frame
    void Update()
    {
        // grwo and shrink
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime*3);
        //shrink and destroy
        timer -= Time.deltaTime;
        if (timer <= 0) {
            targetSize = Vector3.zero;
            if (transform.localScale.x == 0f) {
                Destroy(gameObject);
            }
        }

        // periodic damage
        counter -= Time.deltaTime;
        if (counter <= 0) {
            counter = weapon.speed;
            for (int i = 0; i < enemiesInRange.Count; i++) {
                enemiesInRange[i].TakeDamage(weapon.damage);
            }
        }
        
    }
/*
    private void OnTriggerStay2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.damage);
        }
    }
*/

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            enemiesInRange.Add(collider.GetComponent<Enemy>());
        }

    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            enemiesInRange.Remove(collider.GetComponent<Enemy>());
        }
        
    }


}
