using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour 
{
    public static KeyCode[] keyCodes =
        { 
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
            KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.W, KeyCode.V, KeyCode.X, KeyCode.Y, KeyCode.Z
        };

    public static KeyCode RandomKey()
    {
        return keyCodes[Random.Range(0, keyCodes.Length)];
    }
}