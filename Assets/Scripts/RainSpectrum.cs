using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//-----------------------------------------------------------------------------
// name: Spectrum.cs
// desc: set up and draw thw Spectrum
//-----------------------------------------------------------------------------
public class RainSpectrum : MonoBehaviour
{
    // prefab reference
    public GameObject the_pfCube;
    // array of cubes
    public GameObject[,] the_cubes = new GameObject[32, 512];
    // spectrum history matrix
    public float[,] history = new float[32, 512];
    // controllable scale
    public float MY_SCALE = 8000f;

    public float EARTH_RADIUS = 1000;
    public float INITIAL_RADIUS = 100;
    private float increment = 0.02f;
    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        origin = new Vector3(0.35f, 1.45f, -46.1f);
        for (int ring = 0; ring < 32; ring++) {
            for( int i = 0; i < 512; i++ )
            {
                // instantiate a prefab game object
                GameObject go = Instantiate(the_pfCube);
                // color material
                go.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(.8f, .8f, 1f));
                // default position
                go.transform.position = PolarToCartesian(
                    (increment + 0.001f * ring) * ring + INITIAL_RADIUS, // radius
                    (2f * Mathf.PI) * i/512f + (0f * Mathf.PI) // angle
                ) + origin;
                // give a name!
                go.name = "cube" + i;
                // set a child of this spectrum
                go.transform.parent = this.transform;
                // put into array
                the_cubes[ring, i] = go;
                history[ring, i] = 0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // local refernce to the Spectrum
        float[] st = ChunityAudioInput.the_spectrum;

        for (int i = 0; i < 512; i++) {
            for (int ring = 31; ring >= 1; ring--) {
                history[ring, i] = history[ring-1, i];
            }
            history[0, i] = st[511-i];
        }

        // position the cubes
        for (int ring = 0; ring < 32; ring++) 
        {
            for( int i = 0; i < 512; i++ )
            {
                float shift = MY_SCALE * history[ring, i] * Mathf.Sqrt(history[ring, i]);
                the_cubes[ring, i].transform.position = PolarToCartesian(
                    (increment + 0.001f * ring) * ring + INITIAL_RADIUS + shift, // radius
                    (2f * Mathf.PI) * i/512f + (0f * Mathf.PI) // angle
                ) + origin;
            }
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
