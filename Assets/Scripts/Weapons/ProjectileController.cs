// Name: ProjectileController.cs
// Author: Connor Larsen
// Date: 02/01/2022

using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Public Variables
    public float speed = 200f;
    public float bulletLife = 5.0f;

    // Private Variables
    Vector3 lastPosition;

    // Update is called once per frame
    void Update()
    {
        if (bulletLife > 0) // If bullet still has time left...
        {
            lastPosition = transform.position;                                  // Store the current position of the bullet before moving
            transform.position += transform.forward * speed * Time.deltaTime;   // Move the bullet forward
            bulletLife -= Time.deltaTime;                                       // Decrease bulletLife timer
        }
        else
        {
            Destroy(gameObject);    // Despawn the bullet
        }
    }
}