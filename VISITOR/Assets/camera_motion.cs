using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_motion : MonoBehaviour
{

    float x;
    float y;
    float z;
    
    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = 163.456f;
        z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.rotation = Quaternion.Euler(x, 0, 0);
        x+= 0.1f;
    }
}
