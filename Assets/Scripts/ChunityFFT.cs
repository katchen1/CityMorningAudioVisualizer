﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//-----------------------------------------------------------------------------
// name: ChunityFFT.cs
// desc: fast fourier computations and related datatypes
//   adapted from:
//   https://answers.unity.com/questions/974565/how-to-do-a-fft-in-unity.html
//
// authors: Kunwoo Kim (kunwoo@ccrma.stanford.edu)
//          Jack Atherton (lja@ccrma.stanford.edu)
//          Ge Wang (ge@ccrma.stanford.edu)
// date: Fall 2020
//-----------------------------------------------------------------------------
public class ChunityFFT
{
    // compute the FFT (Complex in => Complex out; result stored in-place in 'signal')
    public static void ComputeFFT( Complex[] signal, bool reverse )
    {
        int power = (int)(System.Math.Log(signal.Length, 2)+.5);
        int count = 1;
        for (int i = 0; i < power; i++)
            count <<= 1;

        int mid = count >> 1; // mid = count / 2;
        int j = 0;
        for (int i = 0; i < count - 1; i++)
        {
            if (i < j)
            {
                var tmp = signal[i];
                signal[i] = signal[j];
                signal[j] = tmp;
            }
            int k = mid;
            while (k <= j)
            {
                j -= k;
                k >>= 1;
            }
            j += k;
        }
        Complex r = new Complex(-1, 0);
        int l2 = 1;
        for (int l = 0; l < power; l++)
        {
            int l1 = l2;
            l2 <<= 1;
            Complex r2 = new Complex(1, 0);
            for (int n = 0; n < l1; n++)
            {
                for (int i = n; i < count; i += l2)
                {
                    int i1 = i + l1;
                    Complex tmp = r2 * signal[i1];
                    signal[i1] = signal[i] - tmp;
                    signal[i] += tmp;
                }
                r2 = r2 * r;
            }
            r.img = System.Math.Sqrt((1d - r.real) / 2d);
            if (!reverse)
                r.img = -r.img;
            r.real = System.Math.Sqrt((1d + r.real) / 2d);
        }

        // if forward FFT
        if (!reverse)
        {
            double scale = 1d / count;
            for (int i = 0; i < count; i++)
                signal[i] *= scale;
        }
        else
        {
            // do nothing?
        }
    }

    // compute the FFT (float in => Complex out; magnitude spectrum in signalReal; complex bins in signal)
    public static void ComputeFFT( Complex[] signal, bool reverse, float[] magBins )
    {
        // compute complex FFT
        ComputeFFT(signal, reverse);

        // store the magnitude 
        if (!reverse)
        {
            for (int i = 0; i < signal.Length / 2; i++)
            {
                magBins[i] = signal[i].fMagnitude;
            }
        }
        else
        {
            for (int i = 0; i < signal.Length / 2; i++)
            {
                magBins[i] = System.Math.Sign(signal[i].real) * signal[i].fMagnitude;
            }
        }
    }
}




//-----------------------------------------------------------------------------
// name: struct Windowing
// desc: windowing functions
//-----------------------------------------------------------------------------
public struct Windowing
{
    // apply window (in-place; results stored in signal)
    public static void Apply(float[] signal, float[] window )
    {
        // the window
        for( int i = 0; i < window.Length; i++ )
        {
            // point-wise multiplication
            signal[i] *= window[i];
        }

        // any zero padding after the window
        for( int i = window.Length; i < signal.Length; i++ )
        {
            signal[i] = 0.0f;
        }
    }

    // hanning window
    public static float[] Hanning(int windowSize)
    {
        // allocate window
        float[] w = new float[windowSize];
        float phase = 0;
        float delta = 2 * Mathf.PI / windowSize;

        // compute window
        for( int i = 0; i < windowSize; i++ )
        {
            w[i] = (.5f * (1.0f - Mathf.Cos(phase)));
            phase += delta;
        }

        // done
        return w;
    }
}




//-----------------------------------------------------------------------------
// name: struct Complex
// desc: Complex datatype
//   adapted from: https://pastebin.com/LiTws7ND
//-----------------------------------------------------------------------------
public struct Complex
{
    // real component
    public double real;
    // imaginary component
    public double img;

    // constructor from (r,i)
    public Complex( double aReal, double aImg )
    {
        real = aReal;
        img = aImg;
    }
    // constructor from (angle, mag)
    public static Complex FromAngle(double aAngle, double aMagnitude)
    {
        return new Complex(System.Math.Cos(aAngle) * aMagnitude, System.Math.Sin(aAngle) * aMagnitude);
    }
    // convert float to Complex
    public static void Float2Complex(float[] input, Complex[] result)
    {
        for (int i = 0; i < input.Length; i++)
        {
            result[i] = new Complex(input[i], 0);
        }
    }

    // properties
    public Complex conjugate { get { return new Complex(real, -img); } }
    public double magnitude { get { return System.Math.Sqrt(real * real + img * img); } }
    public double sqrMagnitude { get { return real * real + img * img; } }
    public double angle { get { return System.Math.Atan2(img, real); } }

    // float values
    public float fReal { get { return (float)real; } set { real = value; } }
    public float fImg { get { return (float)img; } set { img = value; } }
    public float fMagnitude { get { return (float)System.Math.Sqrt(real * real + img * img); } }
    public float fSqrMagnitude { get { return (float)(real * real + img * img); } }
    public float fAngle { get { return (float)System.Math.Atan2(img, real); } }

    // overloaded operators
    #region Basic operations + - * /
    public static Complex operator +(Complex a, Complex b)
    {
        return new Complex(a.real + b.real, a.img + b.img);
    }
    public static Complex operator +(Complex a, double b)
    {
        return new Complex(a.real + b, a.img);
    }
    public static Complex operator +(double b, Complex a)
    {
        return new Complex(a.real + b, a.img);
    }
    public static Complex operator -(Complex a)
    {
        return new Complex(-a.real, -a.img);
    }
    public static Complex operator -(Complex a, Complex b)
    {
        return new Complex(a.real - b.real, a.img - b.img);
    }
    public static Complex operator -(Complex a, double b)
    {
        return new Complex(a.real - b, a.img);
    }
    public static Complex operator -(double b, Complex a)
    {
        return new Complex(b - a.real, -a.img);
    }
    public static Complex operator *(Complex a, Complex b)
    {
        return new Complex(a.real * b.real - a.img * b.img, a.real * b.img + a.img * b.real);
    }
    public static Complex operator *(Complex a, double b)
    {
        return new Complex(a.real * b, a.img * b);
    }
    public static Complex operator *(double b, Complex a)
    {
        return new Complex(a.real * b, a.img * b);
    }
    public static Complex operator /(Complex a, Complex b)
    {
        double d = 1d / (b.real * b.real + b.img * b.img);
        return new Complex((a.real * b.real + a.img * b.img) * d, (-a.real * b.img + a.img * b.real) * d);
    }
    public static Complex operator /(Complex a, double b)
    {
        return new Complex(a.real / b, a.img / b);
    }
    public static Complex operator /(double a, Complex b)
    {
        double d = 1d / (b.real * b.real + b.img * b.img);
        return new Complex(a * b.real * d, -a * b.img);
    }
    #endregion Basic operations + - * /
    // end overloaded operators

    // trig operations
    #region Trigonometic operations
    public static Complex Sin(Complex a)
    {
        return new Complex(System.Math.Sin(a.real) * System.Math.Cosh(a.img), System.Math.Cos(a.real) * System.Math.Sinh(a.img));
    }
    public static Complex Cos(Complex a)
    {
        return new Complex(System.Math.Cos(a.real) * System.Math.Cosh(a.img), -System.Math.Sin(a.real) * System.Math.Sinh(a.img));
    }
    private static double arcosh(double a)
    {
        return System.Math.Log(a + System.Math.Sqrt(a * a - 1));
    }
    private static double artanh(double a)
    {
        return 0.5 * System.Math.Log((1 + a) / (1 - a));

    }
    public static Complex ASin(Complex a)
    {
        double r2 = a.real * a.real;
        double i2 = a.img * a.img;
        double sMag = r2 + i2;
        double c = System.Math.Sqrt((sMag - 1) * (sMag - 1) + 4 * i2);
        double sr = a.real > 0 ? 0.5 : a.real < 0 ? -0.5 : 0;
        double si = a.img > 0 ? 0.5 : a.img < 0 ? -0.5 : 0;

        return new Complex(sr * System.Math.Acos(c - sMag), si * arcosh(c + sMag));
    }
    public static Complex ACos(Complex a)
    {
        return System.Math.PI * 0.5 - ASin(a);
    }
    public static Complex Sinh(Complex a)
    {
        return new Complex(System.Math.Sinh(a.real) * System.Math.Cos(a.img), System.Math.Cosh(a.real) * System.Math.Sin(a.img));
    }
    public static Complex Cosh(Complex a)
    {
        return new Complex(System.Math.Cosh(a.real) * System.Math.Cos(a.img), System.Math.Sinh(a.real) * System.Math.Sin(a.img));
    }
    public static Complex Tan(Complex a)
    {
        return new Complex(System.Math.Sin(2 * a.real) / (System.Math.Cos(2 * a.real) + System.Math.Cosh(2 * a.img)), System.Math.Sinh(2 * a.img) / (System.Math.Cos(2 * a.real) + System.Math.Cosh(2 * a.img)));
    }
    public static Complex ATan(Complex a)
    {
        double sMag = a.real * a.real + a.img * a.img;
        double i = 0.5 * artanh(2 * a.img / (sMag + 1));
        if (a.real == 0)
            return new Complex(a.img > 1 ? System.Math.PI * 0.5 : a.img < -1 ? -System.Math.PI * 0.5 : 0, i);
        double sr = a.real > 0 ? 0.5 : a.real < 0 ? -0.5 : 0;
        return new Complex(0.5 * (System.Math.Atan((sMag - 1) / (2 * a.real)) + System.Math.PI * sr), i);
    }
    #endregion Trigonometic operations
    // end trig operations

    // more operations
    public static Complex Exp(Complex a)
    {
        double e = System.Math.Exp(a.real);
        return new Complex(e * System.Math.Cos(a.img), e * System.Math.Sin(a.img));
    }
    public static Complex Log(Complex a)
    {
        return new Complex(System.Math.Log(System.Math.Sqrt(a.real * a.real + a.img * a.img)), System.Math.Atan2(a.img, a.real));
    }
    public Complex sqrt(Complex a)
    {
        double r = System.Math.Sqrt(System.Math.Sqrt(a.real * a.real + a.img * a.img));
        double halfAngle = 0.5 * System.Math.Atan2(a.img, a.real);
        return new Complex(r * System.Math.Cos(halfAngle), r * System.Math.Sin(halfAngle));
    }

    // what does this do? we don't know! convertion to Vector, we think
#if UNITY
    public static explicit operator UnityEngine.Vector2(Complex a)
    {
        return new UnityEngine.Vector2((float)a.real, (float)a.img);
    }
    public static explicit operator UnityEngine.Vector3(Complex a)
    {
        return new UnityEngine.Vector3((float)a.real, (float)a.img);
    }
    public static explicit operator UnityEngine.Vector4(Complex a)
    {
        return new UnityEngine.Vector4((float)a.real, (float)a.img);
    }
    public static implicit operator Complex(UnityEngine.Vector2 a)
    {
        return new Complex(a.x, a.y);
    }
    public static implicit operator Complex(UnityEngine.Vector3 a)
    {
        return new Complex(a.x, a.y);
    }
    public static implicit operator Complex(UnityEngine.Vector4 a)
    {
        return new Complex(a.x, a.y);
    }
#endif
}
