using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: Waveform.cs
// desc: set up and draw the audio Waveform
//-----------------------------------------------------------------------------
public class CarWaveform : MonoBehaviour
{
    // prefab reference
    public GameObject the_pfCube;
    public GameObject the_car;
    // array of game objects
    public GameObject[] the_cubes = new GameObject[1024];
    // controllable scale
    public float MY_SCALE = 600;
    

    // Start is called before the first frame update
    void Start()
    {
        // x, y, z
        float x = 0.35f, y = 1.15f, z = -46.1f;
        // increment
        float xIncrement = 0.001f;
        // place the cubes initially
        for ( int i = 0; i < the_cubes.Length; i++ )
        {
            // instantiate
            GameObject go = Instantiate(the_pfCube);
            // color material
            go.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(1f, .5f, .5f));
            // transform it
            go.transform.position = new Vector3(x, y, z);
            // increment x
            x += xIncrement;
            // give it name
            go.name = "bin" + i;
            // set this as a child of this Spectrum
            go.transform.parent = this.transform;
            // put into array
            the_cubes[i] = go;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // local reference to the time domain Waveform
        float[] wf = ChunityAudioInput.the_waveform;

        float totalAbs = 0f;
        float total = 0f;

        for( int i = 0; i < the_cubes.Length; i++ )
        {
            totalAbs += Mathf.Abs(wf[i]);
            total += wf[i];
            float y = 10 * wf[i];
            the_cubes[i].transform.localPosition =
                new Vector3(the_cubes[i].transform.localPosition.x,
                y/2,
                the_cubes[i].transform.localPosition.z);
        }

        float sign = (total < 0f)? -1f: 1f;
        float CAR_MOVEMENT_SCALE = 1f;
        float avg = totalAbs / 1024f;
        float carX = 0f, carY = 0f, carZ = -45.39f;
        the_car.transform.position = new Vector3(carX, carY + sign * (CAR_MOVEMENT_SCALE * avg), carZ);
        
    }

    public void SetColor(Color c)
    {
        for (int i = 0; i < the_cubes.Length; i++) {
            the_cubes[i].GetComponent<Renderer>().material.SetColor("_BaseColor", c);
        }
    }
}
