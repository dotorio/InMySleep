using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBone : MonoBehaviour
{
    private Vector3 spawn;

    // Start is called before the first frame update
    void Start()
    {
        spawn = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = spawn;
    }
}
