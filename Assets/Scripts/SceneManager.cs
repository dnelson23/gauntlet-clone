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

        static Dictionary<int, Hero.HeroBase> _players = new Dictionary<int, Hero.HeroBase>
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
            if(_players[1] == null && _players[2] == null &&
               _players[3] == null && _players[4] == null)
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
            return _players[port] == null ? false : true;
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
            if (IsPlayerAdded(1) && _players[1].CurHitPoints > 0) { liveHero = 1; }
            else if (IsPlayerAdded(2) && _players[2].CurHitPoints > 0) { liveHero = 2; }
            else if (IsPlayerAdded(3) && _players[3].CurHitPoints > 0) { liveHero = 3; }
            else if (IsPlayerAdded(4) && _players[4].CurHitPoints > 0) { liveHero = 4; }
            else { return Vector3.zero; }

            Vector3 desiredPos = Vector3.zero;
            RaycastHit hit;

            for(int i = 0; i < 360; i++)
            {
                desiredPos = new Vector3(Mathf.Cos(i), 0f, Mathf.Sin(i));
                if(!Physics.SphereCast(_players[liveHero].transform.position + desiredPos, 0.5f, Vector3.zero, out hit))
                {
                    return desiredPos;
                }
            }

            return desiredPos;
        }
    }
}
