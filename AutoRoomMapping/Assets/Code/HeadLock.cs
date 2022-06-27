using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headLock : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
        gameObject.transform.position = Camera.main.transform.position;
    }
}
