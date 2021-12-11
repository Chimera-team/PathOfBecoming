using System;
using UnityEngine;

namespace PlayerControls
{
    public static class PCButtons
    {
            public static KeyCode LEFT = KeyCode.A;
            public static KeyCode RIGHT = KeyCode.D;
            public static KeyCode RUN = KeyCode.LeftShift;

            public static KeyCode JUMPandUP = KeyCode.W;

            public static KeyCode INVENTORYandDOWN = KeyCode.S;

            public static KeyCode SWITCHSPELL = KeyCode.Q;

            public static KeyCode USESPELL = KeyCode.Space;

            public static KeyCode INTERACT = KeyCode.E;

            public static int xAxisRaw
            {
                get
                {
                    return -Convert.ToInt32(Input.GetKey(LEFT)) + Convert.ToInt32(Input.GetKey(RIGHT));
                }
            }

            public static int yAxisRaw
            {
                get
                {
                    return -Convert.ToInt32(Input.GetKey(INVENTORYandDOWN)) + Convert.ToInt32(Input.GetKey(JUMPandUP));
                }
            }
    }
}
