using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: ChunityAudioInput.cs
// desc: listens to Chunity/Unity audio; end up with waveform and spectrum data
//       to access...
//       waveform: "ChunityAudioInput.the_waveform" (see Waveform.cs)
//       spectrum: "ChunityAudioInput.the_spectrum" (see Spectrum.cs)
//-----------------------------------------------------------------------------
public class ChunityAudioInput : MonoBehaviour
{
    // number of samples used to represent time amplitude (power of 2) => can be changed
    public static int waveformSize = 1024;
    // max number of samples
    public static int waveformMax = 1024;
    // window size
    public static int windowSize = 1024;
    // number of bins to represent frequency magnitude (power of 2) => can be changed
    public static int spectrumSize = 512;

    // time domain samples
    public static float[] the_waveform = new float[waveformSize];
    // waveform, windowed
    public static float[] the_waveformWindowed = new float[waveformMax];

    // window
    float[] window = new float[windowSize];

    // freq domain bins
    public static Complex[] the_spectrumComplex = new Complex[waveformMax];
    // magnitude spectrums (bins)
    public static float[] the_spectrum = new float[spectrumSize];

    void Awake()
    {
        // allocate window
        window = Windowing.Hanning(windowSize);
    }

    // Called every audio block
    private void OnAudioFilterRead(float[] data, int channels)
    {
        // get the number of frames
        int numFrames = data.Length / channels;
        // number of samples to copy, whichever is shorter
        waveformSize = Math.Min(waveformMax, numFrames);
        // zero pad if necessary
        if (waveformSize < waveformMax)
        { for (int i = waveformSize; i < waveformMax; i++) the_waveform[i] = 0; }
        // copy data
        for (int i = 0; i < waveformSize; i++)
        {
            // copy the first channel into the_waveform
            the_waveform[i] = data[i * channels + 0];
            the_waveformWindowed[i] = the_waveform[i];
        }

        // regenerate window if needed
        if (waveformSize < windowSize)
        {
            windowSize = waveformSize;
            window = Windowing.Hanning(windowSize);
        }
        // apply our window
        Windowing.Apply(the_waveformWindowed, window);
        // convert float to complex
        Complex.Float2Complex(the_waveformWindowed, the_spectrumComplex);
        // calc freq domain
        ChunityFFT.ComputeFFT(the_spectrumComplex, false, the_spectrum);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
