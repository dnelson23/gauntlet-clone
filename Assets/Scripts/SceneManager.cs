using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    class SceneManager : MonoBehaviour
    {
        #region Singleton
        private static SceneManager _instance;
        public static SceneManager Instance
        {
            get { return _instance ?? (_instance = GameObject.FindObjectOfType<SceneManager>()); }
        }
        #endregion

        public enum SceneState { Menu, Play, Pause };

        private static SceneState _gameState = SceneState.Play;
        public static SceneState GameState
        {
            get { return _gameState; }
        }

        public static Dictionary<int, Hero.HeroBase> Players = new Dictionary<int, Hero.HeroBase>
        {
            { 1, null },
            { 2, null },
            { 3, null },
            { 4, null }
        };

        static Enemy.EnemyBase[] enemies;
        public static bool IsGameStarted = false;

        public static bool IsPlayerAdded()
        {
            if(Players[1] == null && Players[2] == null &&
               Players[3] == null && Players[4] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPlayerAdded(int port)
        {
            return Players[port] == null ? false : true;
        }

        void Awake()
        {
            _instance = null;
        }

        void Start()
        {
            enemies = GameObject.FindObjectsOfType<Enemy.EnemyBase>();
        }

        void Update()
        {
            if (Input.InputManager.GetJoinGame(1) && IsPlayerAdded(1))
            {

            }

            if (Input.InputManager.GetJoinGame(2) && IsPlayerAdded(2))
            {

            }

            if (Input.InputManager.GetJoinGame(3) && IsPlayerAdded(3))
            {

            }

            if (Input.InputManager.GetJoinGame(4) && IsPlayerAdded(4))
            {

            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                HUDManager.Instance.TogglePauseScreen();
                switch(_gameState)
                {
                    case SceneState.Play:
                        Time.timeScale = 0;
                        _gameState = SceneState.Pause;
                        break;
                    case SceneState.Pause:
                        Time.timeScale = 1;
                        _gameState = SceneState.Play;
                        break;
                }
            }
        }

        Hero.HeroBase AddPlayer(Components.HeroType newType)
        {
            GameObject newHero = Resources.Load<GameObject>(newType.ToString());

            if(newHero == null)
            {
                Debug.Log("Could not load hero prefab for " + newType.ToString());
                return null;
            }

            if(IsGameStarted)
            {
                Vector3 newPos = FindClosestSpawnPoint();
            }
            else
            {
                newHero.transform.position = Vector3.zero;
            }

            return newHero.GetComponent<Hero.HeroBase>();
        }

        Vector3 FindClosestSpawnPoint()
        {
            int liveHero = 0;
            if (IsPlayerAdded(1) && Players[1].CurHitPoints > 0) { liveHero = 1; }
            else if (IsPlayerAdded(2) && Players[2].CurHitPoints > 0) { liveHero = 2; }
            else if (IsPlayerAdded(3) && Players[3].CurHitPoints > 0) { liveHero = 3; }
            else if (IsPlayerAdded(4) && Players[4].CurHitPoints > 0) { liveHero = 4; }
            else { return Vector3.zero; }

            Vector3 desiredPos = Vector3.zero;
            for(int i = 0; i < 360; i++)
            {
                // convert degrees to rads
                float x = Mathf.Cos(i * (Mathf.PI / 180));
                float z = Mathf.Sin(i * (Mathf.PI/180));

                desiredPos = Players[liveHero].transform.position + new Vector3(x, 0f, z);
                Debug.Log("Check " + i + ": " + desiredPos);
                if(Physics.OverlapSphere(desiredPos, 0.5f).Length == 0)
                {
                    return desiredPos;
                }
            }

            return desiredPos;
        }
    }
}
