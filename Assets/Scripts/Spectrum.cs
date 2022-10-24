using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: Spectrum.cs
// desc: set up and draw thw Spectrum
//-----------------------------------------------------------------------------
public class Spectrum : MonoBehaviour
{
    // prefab reference
    public GameObject the_pfCube;
    // array of cubes
    public GameObject[] the_cubes = new GameObject[512];

    // arrays for circular effect
    public Vector3[] target_positions = new Vector3[512];
    public float[] current_radii = new float[512];

    // spectrum history matrix
    public float[,] history = new float[32, 512];

    public float MY_SCALE = 600;
    public float EARTH_RADIUS = 1000;
    public float INITIAL_RADIUS = 100;
    private int history_index;

    // Start is called before the first frame update
    void Start()
    {
        history_index = 0;
        for( int i = 0; i < the_cubes.Length; i++ )
        {
            // instantiate a prefab game object
            GameObject go = Instantiate(the_pfCube);
            // color material
            go.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(.5f, 1, .5f));
            // default position
            go.transform.position = PolarToCartesian(INITIAL_RADIUS, 2f * Mathf.PI * i/512f);
            Debug.Log(i + " before: " + INITIAL_RADIUS + "," + 2f * Mathf.PI * i/512f + " after: " + go.transform.position);
            // give a name!
            go.name = "cube" + i;
            // set a child of this spectrum
            go.transform.parent = this.transform;
            // put into array
            the_cubes[i] = go;
            target_positions[i] = PolarToCartesian(EARTH_RADIUS, 2f * Mathf.PI * i/512f);
            current_radii[i] = INITIAL_RADIUS;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // local refernce to the Spectrum
        float[] st = ChunityAudioInput.the_spectrum;

        // position the cubes
        for( int i = 0; i < the_cubes.Length; i++ )
        {
            current_radii[i] = MY_SCALE * st[i] + INITIAL_RADIUS;
            the_cubes[i].transform.position = PolarToCartesian(current_radii[i], 2f * Mathf.PI * i/512f);
        }
    }

    // // Expand the Waveform outward towards the earth surface
    // public void Move()
    // {
    //     for( int i = 0; i < the_cubes.Length; i++ )
    //     {
    //         the_cubes[i].transform.position = Vector3.MoveTowards(
    //             the_cubes[i].transform.position,
    //             target_positions[i],
    //             10
    //         );
    //         current_radii[i] += (EARTH_RADIUS - current_radii[i]) + 10;
    //     } 
    // }

    public void SetColor(Color c)
    {
        for (int i = 0; i < the_cubes.Length; i++) {
            the_cubes[i].GetComponent<Renderer>().material.SetColor("_BaseColor", c);
        }
    }

    public Vector3 PolarToCartesian(float polarDistance, float polarAngle) {
        float x = polarDistance * Mathf.Cos(polarAngle);
        float y = polarDistance * Mathf.Sin(polarAngle);
        // Transform the reference vector by the rotation
        Vector3 res = new Vector3(x, y, 0);
        return res;
    }
}
