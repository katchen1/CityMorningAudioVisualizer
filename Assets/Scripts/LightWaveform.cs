using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: Waveform.cs
// desc: set up and draw the audio Waveform
//-----------------------------------------------------------------------------
public class LightWaveform : MonoBehaviour
{
    // prefab reference
    public GameObject the_pfCube;
    // array of game objects
    public GameObject[] the_cubes = new GameObject[1024];
    // controllable scale
    public float MY_SCALE = 200f;
    

    // Start is called before the first frame update
    void Start()
    {
        // x, y, z
        float x = 0f, y = 0f, z = 0f;
        // increment
        float xIncrement = 0.083f;
        // place the cubes initially
        for ( int i = 0; i < the_cubes.Length; i++ )
        {
            // instantiate
            GameObject go = Instantiate(the_pfCube);
            // color material
            go.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(1f, .5f, .5f));
            // transform it
            go.transform.localPosition = new Vector3(x, y, z);
            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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

        // x, y, z
        float x = 0f, z = 0f;
        // increment
        float xIncrement = 0.083f;

        for( int i = 0; i < the_cubes.Length; i++ )
        {
            float y = MY_SCALE * wf[i];
            the_cubes[i].transform.localPosition =
                new Vector3(x, y/2, z);
            x += xIncrement;
        }
    }

    public void SetColor(Color c)
    {
        for (int i = 0; i < the_cubes.Length; i++) {
            the_cubes[i].GetComponent<Renderer>().material.SetColor("_BaseColor", c);
        }
    }
}
