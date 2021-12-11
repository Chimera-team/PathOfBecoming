using UnityEngine;

namespace PlayerControls
{
    public static class ControlButtonsHold
    {
        public static bool LEFT
        { 
            get
            {
                return Input.GetKey(PCButtons.LEFT);
            } 
        }

        public static bool RIGHT
        {
            get
            {
                return Input.GetKey(PCButtons.RIGHT);
            }
        }

        public static bool RUN
        {
            get
            {
                return Input.GetKey(PCButtons.RUN);
            }
        }

        public static bool UP
        {
            get
            {
                return Input.GetKey(PCButtons.JUMPandUP);
            }
        }
        public static bool DOWN
        {
            get
            {
                return Input.GetKey(PCButtons.INVENTORYandDOWN);
            }
        }
        public static bool SWITCHSPELL
        {
            get
            {
                return Input.GetKey(PCButtons.SWITCHSPELL);
            }
        }
        public static bool USESPELL
        {
            get
            {
                return Input.GetKey(PCButtons.USESPELL);
            }
        }
        public static bool INTERACT
        {
            get
            {
                return Input.GetKey(PCButtons.INTERACT);
            }
        }
    }

    public static class ControlButtonsPress
    {
        public static bool LEFT
        {
            get
            {
                return Input.GetKeyDown(PCButtons.LEFT);
            }
        }

        public static bool RIGHT
        {
            get
            {
                return Input.GetKeyDown(PCButtons.RIGHT);
            }
        }

        public static bool RUN
        {
            get
            {
                return Input.GetKeyDown(PCButtons.RUN);
            }
        }

        public static bool UP
        {
            get
            {
                return Input.GetKeyDown(PCButtons.JUMPandUP);
            }
        }
        public static bool DOWN
        {
            get
            {
                return Input.GetKeyDown(PCButtons.INVENTORYandDOWN);
            }
        }
        public static bool SWITCHSPELL
        {
            get
            {
                return Input.GetKeyDown(PCButtons.SWITCHSPELL);
            }
        }
        public static bool USESPELL
        {
            get
            {
                return Input.GetKeyDown(PCButtons.USESPELL);
            }
        }
        public static bool INTERACT
        {
            get
            {
                return Input.GetKeyDown(PCButtons.INTERACT);
            }
        }
    }

    public static class ControlButtonsAxis
    {
        public static int xAxisRaw
        {
            get
            {
                return PCButtons.xAxisRaw;
            }
        }

        public static int yAxisRaw
        {
            get
            {
                return PCButtons.yAxisRaw;
            }
        }
    }
}