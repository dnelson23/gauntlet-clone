﻿using System;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        int _controllerIndex;

        public static bool GetStart(int port)
        {
            return UnityEngine.Input.GetButtonDown("P" + port + "Start");
        }

        public void SetPortNum(int port)
        {
            _controllerIndex = port;
        }

        public float GetHorizontalAxis()
        {
            return UnityEngine.Input.GetAxis("P" + _controllerIndex + "Hor");
        }

        public float GetVerticalAxis()
        {
            return UnityEngine.Input.GetAxis("P" + _controllerIndex + "Vert");
        }

        public bool IsAttacking()
        {
            return UnityEngine.Input.GetButtonDown("P" + _controllerIndex + "XButton");
        }

        public bool IsPausing()
        {
            return UnityEngine.Input.GetButtonDown("P" + _controllerIndex + "Start");
        }

        public bool IsUsingSpecial()
        {
            return UnityEngine.Input.GetButtonDown("P" + _controllerIndex + "AButton");
        }
    }
}
