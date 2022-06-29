using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    float countdown;
    bool hasExploded = false;
    LayerMask mask = LayerMask.GetMask("LVL");

    void Start()
    {

        countdown = Controller.timeToExplosion;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0 && !hasExploded)
        {
            //Explode();
            //hasExploded = true;

            Ray ray = new Ray(transform.position + Vector3.up, transform.right);
            Ray ray1 = new Ray(transform.position + Vector3.up, transform.up);
            Ray ray2 = new Ray(transform.position + Vector3.up, -transform.right);
            Ray ray3 = new Ray(transform.position + Vector3.up, -transform.up);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 12f, Color.cyan);
            Debug.DrawRay(ray1.origin, ray1.direction * 12f, Color.cyan);
            Debug.DrawRay(ray2.origin, ray2.direction * 12f, Color.cyan);
            Debug.DrawRay(ray3.origin, ray3.direction * 12f, Color.cyan);

            if (Physics.Raycast(ray, out hit, 12f) && (hit.collider.gameObject.tag == "trigger" || hit.collider.gameObject.tag == "Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            if (Physics.Raycast(ray1, out hit, 12f) && (hit.collider.gameObject.tag == "trigger" || hit.collider.gameObject.tag == "Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            if (Physics.Raycast(ray2, out hit, 12f) && (hit.collider.gameObject.tag == "trigger" || hit.collider.gameObject.tag == "Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            if (Physics.Raycast(ray3, out hit, 12f) && (hit.collider.gameObject.tag == "trigger" || hit.collider.gameObject.tag == "Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            Destroy(gameObject);
        }        
    }

    void Explode()
    {
        

        Collider[] colliders = Physics.OverlapSphere(transform.position,4f);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag!="Wall") { Destroy(nearbyObject); }            
        }

        Destroy(gameObject);
    }

    
}
