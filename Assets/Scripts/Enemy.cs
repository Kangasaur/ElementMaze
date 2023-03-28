using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform player;
    int health = 3;
    public string weakType;
    Rigidbody rb;
    [SerializeField] PrefabDatabase prefabDB;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 15f)
        {
            transform.LookAt(player);
            Vector3 move = transform.TransformDirection(Vector3.forward) * 2;
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
