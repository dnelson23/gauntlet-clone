using System;
using UnityEngine;
using System.Collections;
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

        public enum SceneState { Menu, Play, Pause, Transition };

        private static SceneState _gameState = SceneState.Menu;
        public static SceneState GameState
        {
            get { return _gameState; }
        }

        int pausedPlayer;
        bool justPaused = false;

        public readonly Dictionary<int, Hero.HeroBase> Players = new Dictionary<int, Hero.HeroBase>
        {
            { 1, null },
            { 2, null },
            { 3, null },
            { 4, null }
        };

        Vector3[] playerSpawnPoints = new Vector3[4]
        {
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, -1)
        };

        Enemy.EnemyBase[] enemies;
        EnemySpawner[] enemySpawners;

        public bool IsPlayerAdded()
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

        void Awake()
        {
            _instance = null;
            _gameState = SceneState.Menu;
        }

        void Start()
        {
            enemies = GameObject.FindObjectsOfType<Enemy.EnemyBase>();
            enemySpawners = GameObject.FindObjectsOfType<EnemySpawner>();
            Time.timeScale = 0;
        }

        void Update()
        {
            switch(_gameState)
            {
                case SceneState.Menu:
                    DoMenuState();
                    break;
                case SceneState.Play:
                    DoPlayState();
                    break;
                case SceneState.Pause:
                    DoPauseState();
                    break;
                default:
                    break;
            }
        }

        void DoMenuState()
        {
            for (int pIndex = 1; pIndex <= 4; pIndex++)
            {
                // Check if joined players starts game
                if (Input.InputManager.GetStart(pIndex) && IsPlayerAdded(pIndex))
                {
                    BeginGame();
                }

                // Check for joining players
                if (Input.InputManager.GetStart(pIndex) && !IsPlayerAdded(pIndex))
                {
                    Players[pIndex] = AddPlayer((Components.HeroType)pIndex - 1);
                    HUDManager.Instance.ShowStartText();
                    HUDManager.Instance.HidePlayerMenu(pIndex);
                    HUDManager.Instance.ShowPlayerHUD(pIndex);
                }
            }
        }

        void DoPlayState()
        {
            foreach(Enemy.EnemyBase eObj in enemies)
            {
                eObj.isVisible = eObj.GetComponent<Renderer>().IsVisibleFrom(Camera.main);
            }

            for(int pNum = 1; pNum <= 4; pNum++)
            {
                if(Players[pNum] == null && Input.InputManager.GetStart(pNum))
                {
                    Components.HeroType hType = (Components.HeroType)(pNum - 1);
                    Players[pNum] = AddPlayer(hType);
                    if(Players[pNum] != null)
                    {
                        HUDManager.Instance.HidePlayerMenu(pNum);
                        HUDManager.Instance.ShowPlayerHUD(pNum);
                    }
                }

                if (Players[pNum] != null && (Players[pNum].HasWon || Players[pNum].CurHitPoints <= 0f))
                {
                    PlayAgain();
                }
            }
        }

        void DoPauseState()
        {
            if (justPaused) { justPaused = false;  return; }

            if(Input.InputManager.GetStart(pausedPlayer))
            {
                TogglePause(pausedPlayer);
            }
        }

        void BeginGame()
        {
            HUDManager.Instance.HideMenu();
            _gameState = SceneState.Play;
            Camera.main.GetComponent<CameraFollow>().InitCamPlayers();
            Time.timeScale = 1f;
        }

        public void TogglePause(int player)
        {
            HUDManager.Instance.TogglePauseScreen();
            pausedPlayer = player;
            switch (_gameState)
            {
                case SceneState.Play:
                    HUDManager.Instance.UpdatePauseScreen(Players[player].Type.ToString());
                    _gameState = SceneState.Pause;
                    Time.timeScale = 0f;
                    justPaused = true;
                    break;
                case SceneState.Pause:
                    if (pausedPlayer == player)
                    {
                        _gameState = SceneState.Play;
                        Time.timeScale = 1f;
                    }
                    break;
                default:
                    break;
            }
        }

        public void DestroyEnemiesOnSCreen()
        {
            for(int i = 0; i < enemySpawners.Length; i++)
            {
                enemySpawners[i].DestroyVisibleEnemies();
            }
        }
        
        public bool IsPlayerAdded(int port)
        {
            return Players[port] == null ? false : true;
        }

        Hero.HeroBase AddPlayer(Components.HeroType newType)
        {
            string prefabPath = Hero.HeroBase.HeroResourcePath[(int)newType];
            GameObject heroPrefab = Resources.Load<GameObject>(prefabPath);
            
            if(heroPrefab == null)
            {
                Debug.Log("Could not load hero prefab for " + newType.ToString());
                return null;
            }

            Vector3 spawnPos = Vector3.zero;

            if(_gameState == SceneState.Menu)
            {
                spawnPos = playerSpawnPoints[(int)newType];
            }
            else if(_gameState == SceneState.Play)
            {
                spawnPos = FindClosestSpawnPoint();
            }

            if(spawnPos == Vector3.zero)
            {
                HUDManager.Instance.ShowCantSpawn(newType.ToString());
                return null;
            }

            GameObject newHero = GameObject.Instantiate<GameObject>(heroPrefab);
            newHero.transform.position = spawnPos;
            return newHero.GetComponent<Hero.HeroBase>();
        }

        public void PlayerIsLowHealth(int pNum)
        {
            string pName = Players[pNum].Type.ToString();
            HUDManager.Instance.ShowLowHealth(pName);
        }

        public void PlayerDied(int pNum)
        {
            string pName = Players[pNum].Type.ToString();
            HUDManager.Instance.ShowPlayerDied(pName);
        }

        void PlayAgain()
        {
            Time.timeScale = 0f;
            HUDManager.Instance.ShowRestartScreen();
            for(int i = 1; i <= Players.Count; i++)
            {
                if(Input.InputManager.GetStart(i))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("PlayableLevel1");
                }
            }
        }

        Vector3 FindClosestSpawnPoint()
        {
            int liveHero = 0;
            if (IsPlayerAdded(1) && Players[1].CurHitPoints > 0) { liveHero = 1; }
            else if (IsPlayerAdded(2) && Players[2].CurHitPoints > 0) { liveHero = 2; }
            else if (IsPlayerAdded(3) && Players[3].CurHitPoints > 0) { liveHero = 3; }
            else if (IsPlayerAdded(4) && Players[4].CurHitPoints > 0) { liveHero = 4; }
            else { return Vector3.zero; }

            Vector3 spawnPos = Vector3.zero;
            Vector3 desiredPos;
            for (int i = 0; i < 360; i++)
            {
                // convert degrees to rads
                float x = Mathf.Cos(i * (Mathf.PI / 180));
                float z = Mathf.Sin(i * (Mathf.PI / 180));

                desiredPos = Players[liveHero].transform.position + new Vector3(x, 0f, z);
                if (Physics.OverlapSphere(desiredPos, 0.5f).Length == 0)
                {
                    return desiredPos;
                }
            }

            return spawnPos;
        }
    }
}
