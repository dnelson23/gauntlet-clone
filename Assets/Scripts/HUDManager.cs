using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    class PlayerHUD
    {
        public GameObject Parent;
        public Text Name;
        public Text HealthNum;
        public GameObject[] Keys = new GameObject[4];
        public GameObject[] Potions = new GameObject[4];

        public PlayerHUD(GameObject parent, Text name, Text health, GameObject[] keys, GameObject[] potions)
        {
            Parent = parent;
            Name = name;
            HealthNum = health;
            Keys = keys;
            Potions = potions;
        }
    }

    public sealed class HUDManager : MonoBehaviour
    {
        #region Singleton
        private static HUDManager _instance;
        public static HUDManager Instance
        {
            get { return _instance ?? (_instance = GameObject.FindObjectOfType<HUDManager>()); }
        }
        #endregion

        static Dictionary<int, PlayerHUD> PlayerHUDs = new Dictionary<int, PlayerHUD> 
        {
            { 1, null },
            { 2, null },
            { 3, null },
            { 4, null }
        };

        GameObject[] PlayerMenus = new GameObject[4];
        GameObject Menu;
        GameObject JoinGameTxt;
        GameObject StartGameTxt;

        GameObject HealthNotice;
        GameObject DeathNotice;
        GameObject SpawnNotice;
        GameObject PauseScreen;
        GameObject RestartScreen;

        int num = 1;

        void Start()
        {
            _instance = null;

            InitPlayerHUDs();

            // Set menu huds
            for (int i = 1; i <= 4; i++)
            {
                PlayerMenus[i - 1] = GameObject.Find("P" + i + "Menu");
            }
            Menu = GameObject.Find("Menu");
            JoinGameTxt = GameObject.Find("JoinGameTxt");
            StartGameTxt = GameObject.Find("StartGameTxt");
            StartGameTxt.SetActive(false);

            PauseScreen = GameObject.Find("PauseScreen");
            PauseScreen.SetActive(false);

            HealthNotice = GameObject.Find("HealthNotification");
            HealthNotice.SetActive(false);

            DeathNotice = GameObject.Find("DeathNotification");
            DeathNotice.SetActive(false);

            SpawnNotice = GameObject.Find("SpawnNotification");
            SpawnNotice.SetActive(false);

            RestartScreen = GameObject.Find("RestartScreen");
            RestartScreen.SetActive(false);
        }

        void InitPlayerHUDs()
        {
            for(int player = 1; player <= 4; player++)
            {
                GameObject parent = GameObject.Find("P" + player + "HUD");
                Text name = GameObject.Find("P" + player + "Name").GetComponent<Text>();
                Text health = GameObject.Find("P" + player + "HealthNum").GetComponent<Text>();
                if (SceneManager.Instance.Players[player] != null)
                {
                    health.text = SceneManager.Instance.Players[player].CurHitPoints.ToString();
                }

                GameObject[] keys = new GameObject[4];
                GameObject[] pots = new GameObject[4];

                for(int itemNum = 0; itemNum < 4; itemNum++)
                {
                    keys[itemNum] = GameObject.Find("P" + player + "Key" + (itemNum + 1));
                    keys[itemNum].SetActive(false);

                    pots[itemNum] = GameObject.Find("P" + player + "Pot" + (itemNum + 1));
                    pots[itemNum].SetActive(false);
                }

                parent.SetActive(false);
                PlayerHUDs[player] = new PlayerHUD(parent, name, health, keys, pots);
            }
        }

        public void ShowStartText()
        {
            if(JoinGameTxt.activeSelf)
            {
                JoinGameTxt.SetActive(false);
            }

            StartGameTxt.SetActive(true);
        }

        public void HideMenu()
        {
            Menu.SetActive(false);
        }

        public void ShowMenuHUD()
        {
            Menu.SetActive(true);
        }

        public void HidePlayerMenu(int player)
        {
            PlayerMenus[player - 1].SetActive(false);
        }

        public void ShowPlayerHUD(int player)
        {
            PlayerHUDs[player].Parent.SetActive(true);
            UpdatePlayerHealth(player);
        }

        public void UpdatePlayerHealth(int player)
        {
            PlayerHUDs[player].HealthNum.text = SceneManager.Instance.Players[player].CurHitPoints.ToString();
        }

        #region Notifications
        public void ShowLowHealth(string player)
        {
            StartCoroutine(LowHealthNotification(player));
        }

        IEnumerator LowHealthNotification(string player)
        {
            while(DeathNotice.activeSelf || SpawnNotice.activeSelf)
            {
                yield return 0;
            }

            HealthNotice.SetActive(true);
            CanvasGroup cGroup = HealthNotice.GetComponent<CanvasGroup>();
            string notification = player + "s Health is running out!";
            HealthNotice.GetComponentInChildren<Text>().text = notification;
            yield return new WaitForSeconds(3);

            while(cGroup.alpha > 0)
            {
                cGroup.alpha -= 0.05f;
                yield return new WaitForEndOfFrame();
            }

            cGroup.alpha = 1;
            HealthNotice.SetActive(false);
        }

        public void ShowPlayerDied(string player)
        {
            StartCoroutine(PlayerDeathNotification(player));
        }

        IEnumerator PlayerDeathNotification(string player)
        {
            while(HealthNotice.activeSelf || SpawnNotice.activeSelf)
            {
                yield return 0;
            }

            DeathNotice.SetActive(true);
            CanvasGroup cGroup = DeathNotice.GetComponent<CanvasGroup>();
            string notification = player + " has died!";
            DeathNotice.GetComponentInChildren<Text>().text = notification;
            yield return new WaitForSeconds(3);

            while (cGroup.alpha > 0)
            {
                cGroup.alpha -= 0.05f;
                yield return new WaitForEndOfFrame();
            }

            cGroup.alpha = 1;
            DeathNotice.SetActive(false);
        }

        public void ShowCantSpawn(string player)
        {
            StartCoroutine(PlayerSpawnNotification(player));
        }

        IEnumerator PlayerSpawnNotification(string player)
        {
            while (HealthNotice.activeSelf || DeathNotice.activeSelf)
            {
                yield return 0;
            }

            SpawnNotice.SetActive(true);
            CanvasGroup cGroup = SpawnNotice.GetComponent<CanvasGroup>();
            string notification = player + " cannot spawn, too many enemies!";
            SpawnNotice.GetComponentInChildren<Text>().text = notification;
            yield return new WaitForSeconds(3);

            while (cGroup.alpha > 0)
            {
                cGroup.alpha -= 0.05f;
                yield return new WaitForEndOfFrame();
            }

            cGroup.alpha = 1;
            SpawnNotice.SetActive(false);
        }
        #endregion

        public void TogglePauseScreen()
        {
            bool stateToToggle = PauseScreen.activeSelf ? false : true;
            PauseScreen.SetActive(stateToToggle);
        }

        public void UpdatePauseScreen(string name)
        {
            PauseScreen.GetComponentInChildren<Text>().text = "-- " + name + " Pause --";
        }

        public void ShowRestartScreen()
        {
            RestartScreen.SetActive(true);
        }

        public void ShowPlayerKey(int player, int key)
        {
            PlayerHUDs[player].Keys[key - 1].SetActive(true);
        }

        public void HidePlayerKey(int player, int key)
        {
            PlayerHUDs[player].Keys[key - 1].SetActive(false);
        }

        public void ShowPlayerPot(int player, int pot)
        {
            PlayerHUDs[player].Potions[pot - 1].SetActive(true);
        }

        public void HidePlayerPot(int player, int pot)
        {
            PlayerHUDs[player].Potions[pot - 1].SetActive(false);
        }
    }
}
