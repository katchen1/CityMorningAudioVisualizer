using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: Waveform.cs
// desc: set up and draw the audio Waveform
//-----------------------------------------------------------------------------
public class Waveform : MonoBehaviour
{
    // prefab reference
    public GameObject the_pfCube;
    // array of game objects
    public GameObject[] the_cubes = new GameObject[1024];
    // controllable scale
    public float MY_SCALE = 600;
    

    // Start is called before the first frame update
    void Start()
    {
        // x, y, z
        float x = -512, y = 0, z = 0;
        // increment
        float xIncrement = the_pfCube.transform.localScale.x * 2;
        // place the cubes initially
        for ( int i = 0; i < the_cubes.Length; i++ )
        {
            // instantiate
            GameObject go = Instantiate(the_pfCube);
            // color material
            go.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(.5f, .5f, 1));
            // transform it
            go.transform.position = new Vector3(x, y, z);
            // increment x
            x += xIncrement;
            // scale it to be 2x wider
            go.transform.localScale = new Vector3(2, 0, 1);
            // give it name
            go.name = "bin" + i;
            // set this as a child of this Spectrum
            go.transform.parent = this.transform;
            // put into array
            the_cubes[i] = go;
        }

        // position this
        this.transform.position = new Vector3(this.transform.position.x, -100, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // local reference to the time domain Waveform
        float[] wf = ChunityAudioInput.the_waveform;

        for( int i = 0; i < the_cubes.Length; i++ )
        {
            float y = MY_SCALE * wf[i];
            the_cubes[i].transform.localPosition =
                new Vector3(the_cubes[i].transform.localPosition.x,
                y/2,
                the_cubes[i].transform.localPosition.z);
        }
    }

    public void SetColor(Color c)
    {
        for (int i = 0; i < the_cubes.Length; i++) {
            the_cubes[i].GetComponent<Renderer>().material.SetColor("_BaseColor", c);
        }
    }
}
