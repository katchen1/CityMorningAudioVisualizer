using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpectrum : MonoBehaviour
{
    // prefab reference
    public GameObject the_pfCube;
    public float MY_SCALE;
    // array of cubes
    public GameObject[,] the_cubes = new GameObject[16, 512];
    private float[,] history = new float[16, 512];
    private float originX = -21f, originY = -0.3f, originZ = -47f;
    // Start is called before the first frame update
    void Start()
    {
        // increment
        // float xIncrement = the_pfCube.transform.localScale.x * 2;
        float xIncrement = 1f;
        float zIncrement = 0.5f;


        for (int d = 0; d < 16; d++) {
            for( int i = 0; i < 512; i++ )
            {
                // instantiate a prefab game object
                GameObject go = Instantiate(the_pfCube);
                // color material
                go.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(0f, 0f, 1f-Mathf.Clamp((i/25f), 0f, 1f)));
                // default position
                go.transform.position = new Vector3(originX + i*xIncrement, originY, originZ + d * zIncrement);
                go.transform.localScale = new Vector3(1f, 1f, 0.25f);
                // give a name!
                go.name = "cube" + i;
                // set a child of this spectrum
                go.transform.parent = this.transform;
                // put into array
                the_cubes[d, i] = go;
                history[d, i] = 0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // local reference to the spectrum
        float[] st = ChunityAudioInput.the_spectrum;

        for (int i = 0; i < 512; i++) {
            for (int d = 15; d >= 1; d--) {
                history[d, i] = history[d-1, i];
            }
            history[0, i] = st[i];
        }

        // position the cubes
        for (int d = 0; d < 16; d++) 
        {
            for( int i = 0; i < 512; i++ )
            {
                float y = MY_SCALE * Mathf.Sqrt(history[d, i]);
                the_cubes[d, i].transform.position =
                    new Vector3(
                        the_cubes[d, i].transform.position.x,
                        originY + y,
                        the_cubes[d, i].transform.position.z);
            }
        }
    }
}
