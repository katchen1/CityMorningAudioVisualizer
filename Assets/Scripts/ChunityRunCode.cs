using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: ChunityRunCode.cs
// desc: run ChucK code from here, either as text or from a .ck file
//-----------------------------------------------------------------------------
public class ChunityRunCode : MonoBehaviour
{
    // the chuck subinstance
    ChuckSubInstance chuck;

    // Start is called before the first frame update
    void Start()
    {
        // get the chuck subinstance on the object this script is attached to
        chuck = GetComponent<ChuckSubInstance>();
        // run code
        runMic();
    }

    void runMic()
    {   
        // run code -- this is the microphone input from chuck
        chuck.RunCode(
            @"adc => Gain g => dac; while( true ) 1::second => now;"
        );
    }

    void runDope()
    {
        // run code -- this constructs a sound loop
        chuck.RunCode(
            @"SndBuf buffy => dac;
              ""special:dope"" => buffy.read;
              while( true ) { 0 => buffy.pos; 400::ms => now; }"
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
