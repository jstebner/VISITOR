using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_motion : MonoBehaviour
{

    float x;
    float y;
    float z;

    private float freq = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        x = 1;
        y = 163.456f;
        z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float x_ofst = Mathf.Sin(Time.time * freq);
        float y_ofst = Mathf.Sin(Time.time * freq * 1.618f);
        float z_ofst = Mathf.Sin(Time.time * freq * 1.414f);

        Camera.main.transform.rotation = Quaternion.Euler(
            x+x_ofst, 
            y+y_ofst,
            z+z_ofst
        );
    }
}
