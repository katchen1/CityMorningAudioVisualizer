using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarWaveform : MonoBehaviour
{
    public float MY_SCALE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // local reference to the time domain Waveform
        float[] wf = ChunityAudioInput.the_waveform;

        float totalAbs = 0f;
        float total = 0f;
        float coinFlip = Random.Range(0f, 1f);
        coinFlip = (coinFlip >= 0.5f)? 1f: -1f;

        for( int i = 0; i < wf.Length; i++ )
        {
            totalAbs += Mathf.Abs(wf[i]);
            total += wf[i];
        }

        float shift = MY_SCALE * totalAbs / 15f;
        float sign = (total < 0f)? -1f: 1f;
        float res = sign * shift * coinFlip;
        this.transform.localScale = new Vector3(1.5f + res, 1.5f + res, 1.5f + res);
    }
}
