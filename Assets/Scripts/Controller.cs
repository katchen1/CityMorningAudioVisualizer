using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Public variables, passed in from the Unity editor
    public GameObject[] cities;
    public GameObject[] skies;
    public GameObject[] clouds;
    public GameObject[] wiresWF; // 9 waveforms
    public GameObject carWF; // waveform
    public GameObject roadST, rainST, meteorST; // spectrums
    public GameObject dude; // to move the dude in the car via code
    
    // Private variables
    private bool cityScrolling = true, skyScrolling = true;
    private float CITY_SCROLL_SPEED = 0.1f, SKY_SCROLL_SPEED = 0.1f;
    private GameObject cm; // the camera
    private Vector3[] cmPos = new Vector3[4]; // camera positions
    private Quaternion[] cmAng = new Quaternion[4]; // camera angles
    private Vector3[,] cloudsPos = new Vector3[2,2]; // cloud positions

    // Start is called before the first frame update
    void Start()
    {
        cm = this.gameObject;

        // Define camera path for narrative
        // View 1 - car
        cmPos[0] = new Vector3(0.735f, 1.293f, -46.538f);
        cmAng[0] = Quaternion.Euler(-4.3f, 11.608f, -3.406f);
        // View 2 - city, zoomed out
        cmPos[1] = new Vector3(0.735f, 7f, -60.538f);
        cmAng[1] = Quaternion.Euler(0f, 0f, 0f);
        // View 3 - wires
        cmPos[2] = new Vector3(0.735f, 15f, -47.7f);
        cmAng[2] = Quaternion.Euler(0f, 0f, 0f);
        // View 4 - sky
        cmPos[3] = new Vector3(0.735f, 135f, -95.538f);
        cmAng[3] = Quaternion.Euler(0f, 0f, 0f);
        cloudsPos[0,0] = new Vector3(27.53889f, 93.40513f, -92.01807f);
        cloudsPos[1,0] = new Vector3(-59.1f, 66.9f, 37.5f);
        cloudsPos[0,1] = new Vector3(148f, 93.40513f, -131f);
        cloudsPos[1,1] = new Vector3(-223f, 66.9f, 55.1f);

        // TODO: start in explore mode, go to narrative upon UI button click
        StartCoroutine(Narrative());
    }

    // Recorded narrative (about 70 seconds) submitted as part of HW2 final deliverables
    IEnumerator Narrative()
    {
        // View 1 - Closeup of Car (14s)
        ActivateSkyDetails(false);
        ActivateCityDetails(false);
        ActivateCarDetails(true);
        ActivateCityDetails(true);
        ActivateSkyDetails(true);
        MoveObjectToPosition(cm, cmPos[0]);
        RotateObjectToAngle(cm, cmAng[0]);
        PlayAudio();
        yield return new WaitForSeconds(14); // 14

        // View 2 - City (10s)
        StartCoroutine(MoveObjectOverTime(cm, cmPos[0], cmPos[1], 5));
        ActivateCarDetails(false);
        yield return RotateObjectOverTime(cm, cmAng[0], cmAng[1], 2); // 2
        yield return new WaitForSeconds(8); // 8

        // View 3 - Wires Between the Poles (7s)
        yield return MoveObjectOverTime(cm, cmPos[1], cmPos[2], 1) ; // 1
        yield return new WaitForSeconds(6.35f); // 6.35

        // Briefly repeat View 1 (1.7s)
        ActivateCarDetails(true);
        MoveObjectToPosition(cm, cmPos[0]);
        RotateObjectToAngle(cm, cmAng[0]);
        yield return new WaitForSeconds(1.7f); // 1.7

        // Briefly repeat View 2 (0.75s)
        ActivateCarDetails(false);
        MoveObjectToPosition(cm, cmPos[1]);
        RotateObjectToAngle(cm, cmAng[1]);
        yield return new WaitForSeconds(0.75f); // 0.75

        // Briefly repeat View 3 (0.75s)
        MoveObjectToPosition(cm, cmPos[2]);
        yield return new WaitForSeconds(0.75f); // 0.75

        // Zoom out to View 2 - City (4s)
        yield return MoveObjectOverTime(cm, cmPos[2], cmPos[1], 3); // 3
        yield return new WaitForSeconds(1); // 1

        //  Move towards sky and disperse clouds (9s)
        StartCoroutine(MoveObjectOverTime(cm, cmPos[1], cmPos[3], 8));
        yield return new WaitForSeconds(5); // 5
        ActivateCityDetails(false);
        StartCoroutine(MoveObjectOverTime(clouds[0], cloudsPos[0,0], cloudsPos[0,1], 4));
        yield return MoveObjectOverTime(clouds[1], cloudsPos[1,0], cloudsPos[1,1], 4);

        // View 4 - Sky (Without Meteor Rotation) (11s)
        meteorST.gameObject.GetComponent<AnimationScript>().isAnimated = false;
        yield return new WaitForSeconds(11);

        // View 4 - Sky (With Meteor Rotation) (7s)
        meteorST.gameObject.GetComponent<AnimationScript>().isAnimated = true;
        SKY_SCROLL_SPEED = 0.3f;
        yield return new WaitForSeconds(7);
    }

    // Helper function
    void ActivateCarDetails(bool state) 
    {
        rainST.gameObject.SetActive(state);
        carWF.gameObject.SetActive(state);
        dude.gameObject.SetActive(state);
    }

    // Helper function
    void ActivateCityDetails(bool state) 
    {
        cityScrolling = state;
        roadST.gameObject.SetActive(state);
        foreach (GameObject go in wiresWF) go.gameObject.SetActive(state);
    }

    // Helper function
    void ActivateSkyDetails(bool state)
    {
        skyScrolling = state;
        meteorST.gameObject.SetActive(state);
        foreach (GameObject go in skies) go.gameObject.SetActive(state);
    }

    // Update is called once per frame
    void Update()
    {
        if (cityScrolling) {
            for (int i = 0; i < cities.Length; i++) {
                cities[i].transform.position = new Vector3(
                    cities[i].transform.position.x + CITY_SCROLL_SPEED,
                    cities[i].transform.position.y,
                    cities[i].transform.position.z
                );
                if (cities[i].transform.position.x > -460f) {
                    cities[i].transform.position = new Vector3(
                        cities[i].transform.position.x - 3f * 85f,
                        cities[i].transform.position.y,
                        cities[i].transform.position.z
                    );
                }
            }
        }
        if (skyScrolling) {
            for (int i = 0; i < skies.Length; i++) {
                skies[i].transform.position = new Vector3(
                    skies[i].transform.position.x + SKY_SCROLL_SPEED,
                    skies[i].transform.position.y,
                    skies[i].transform.position.z
                );
                if (skies[i].transform.position.x > 41f) {
                    skies[i].transform.position = new Vector3(
                        skies[i].transform.position.x - 2f * 127f,
                        skies[i].transform.position.y,
                        skies[i].transform.position.z
                    );
                }
            }
        }

        // Switch between the views
        if (Input.GetKeyDown("1")) {
            ActivateCarDetails(true);
            ActivateCityDetails(true);
            ActivateSkyDetails(false);
            MoveObjectToPosition(cm, cmPos[0]);
        } else if (Input.GetKeyDown("2")) {
            ActivateCarDetails(false);
            ActivateCityDetails(true);
            ActivateSkyDetails(false);
            MoveObjectToPosition(cm, cmPos[1]);
        } else if (Input.GetKeyDown("3")) {
            ActivateCarDetails(false);
            ActivateCityDetails(false);
            ActivateSkyDetails(true);
            MoveObjectToPosition(cm, cmPos[3]);
        }
    }

    // Move an object to a new position
    void MoveObjectToPosition(GameObject obj, Vector3 pos)
    {
        obj.transform.position = pos;
    }

    // Rotate an object to a new angle
    void RotateObjectToAngle(GameObject obj, Quaternion angle)
    {   
        obj.transform.rotation = angle;
    }

    // Move an object from start to end in t seconds
    IEnumerator MoveObjectOverTime(GameObject obj, Vector3 start, Vector3 end, float t)
    {
        obj.transform.position = start;
        Vector3 currentPos = start;
        float ratio = 0f;
        while (ratio < 1)
        {
            ratio += Time.deltaTime / t;
            obj.transform.position = Vector3.Lerp(currentPos, end, ratio);
            yield return null;
        }
    }

    // Rotate an object from start to end in t seconds
    IEnumerator RotateObjectOverTime(GameObject obj, Quaternion start, Quaternion end, float t)
    {
        obj.transform.rotation = start;
        Quaternion currentAngle = start;
        float ratio = 0f;
        while (ratio < 1)
        {
            ratio += Time.deltaTime / t;
            obj.transform.rotation = Quaternion.Slerp(currentAngle, end, ratio);
            yield return null;
        }
    }

    void PlayAudio()
    {
        float intensity = 0.5f;
        GetComponent<ChuckSubInstance>().RunCode( string.Format( @"
            SndBuf gunjouBuf => LPF filter => HPF filter2 => dac;
            me.dir() + ""Gunjou.wav"" => gunjouBuf.read;

            // start at the beginning of the clip
            0 => gunjouBuf.pos;

            // set gain: least intense is quiet, most intense is loud; range 0.05 to 1
            0.05 + 0.95 * {0} => gunjouBuf.gain;
            1.5::second => now;
            
            Math.random2f( 300, 1000 ) => filter.freq;
            Math.random2f( 300, 1000 ) => filter2.freq;
            14::second => now;

            Math.random2f( 300, 1000 ) => filter.freq;
            Math.random2f( 300, 1000 ) => filter2.freq;
            10::second => now;

            Math.random2f( 300, 1000 ) => filter.freq;
            Math.random2f( 300, 1000 ) => filter2.freq;
            10.5::second => now;

            Math.random2f( 300, 1000 ) => filter.freq;
            Math.random2f( 300, 1000 ) => filter2.freq;
            13::second => now;

            Math.random2f( 300, 1000 ) => filter.freq;
            Math.random2f( 300, 1000 ) => filter2.freq;
            11::second => now;

            Math.random2f( 300, 1000 ) => filter.freq;
            Math.random2f( 300, 1000 ) => filter2.freq;
            16.5::second => now;

            0.05 => gunjouBuf.gain;
            172::second => now;
	    ", intensity ) );
    }
}
