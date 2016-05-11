using System;
using UnityEngine;

namespace Assets.Scripts.Input
{
    class InputManager : MonoBehaviour
    {
        public static bool GetJoinGame(int port)
        {
            return UnityEngine.Input.GetButtonDown(port + "Start");
        }

        int _controllerIndex;

        public InputManager(int portNum)
        {
            _controllerIndex = portNum;
        }

        public float GetHorizontalAxis()
        {
            return UnityEngine.Input.GetAxis(_controllerIndex + "Hor");
        }

        public float GetVerticalAxis()
        {
            return UnityEngine.Input.GetAxis(_controllerIndex + "Vert");
        }

        public bool IsAttacking()
        {
            return UnityEngine.Input.GetButtonDown(_controllerIndex + "Attack");
        }

        public bool IsPausing()
        {
            return UnityEngine.Input.GetButtonDown(_controllerIndex + "Pause");
        }

        public bool IsUsingSpecial()
        {
            return UnityEngine.Input.GetButtonDown(_controllerIndex + "Special");
        }
    }
}
