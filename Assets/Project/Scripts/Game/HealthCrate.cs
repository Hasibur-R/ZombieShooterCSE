using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrate : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject container;
    public float rotationSpeed = 180f; //rotating speed manual input

    [Header("Gameplay")]
    public int health = 50; // buller manual input


    // Update is called once per frame
    void Update() // Time deltatime for various device, rotataion speed given
    {
        container.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
