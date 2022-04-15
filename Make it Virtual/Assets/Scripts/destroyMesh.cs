using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyMesh : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 3f);
    }
}
