using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Transform bomb, spawnPos;
    public static Transform spPos;
    private CharacterController controller;
    public float gravity = -9.81f;
    public float speed = 7f;
    public bool move = false;
    public static float timeToExplosion = 3f;
    Vector3 pos = new Vector3();

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        spPos = spawnPos;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            double x = spawnPos.position.x;
            double z = spawnPos.position.z;
            pos = spawnPos.transform.position;

            if (x > 0) { while (x > 2.1) { x -= 4.2; } pos.x -= (float)x; }
            else { while (x < 2.1) { x += 4.2; } pos.x -= (float)x - 4.2f; }

            if (z > 0) { while (z > 2.1) { z -= 4.2; } pos.z -= (float)z; }
            else { while (z < 2.1) { z += 4.2; } pos.z -= (float)z - 4.2f; }

            pos.y = 2.6f;
            Instantiate(bomb,pos,bomb.rotation);
        }

        if (Input.GetKeyDown("1"))
        {
            timeToExplosion = 1f;
        }

        if (Input.GetKeyDown("2"))
        {
            timeToExplosion = 2f;
        }

        if (Input.GetKeyDown("3"))
        {
            timeToExplosion = 3f;
        }

        if (Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("d") || Input.GetKey("s"))
        {
            move = true;
        }
        else { move = false; }

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            controller.Move(direction * speed * Time.deltaTime);
        }

        pos = spawnPos.position;
        if (pos.x > 21) { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
