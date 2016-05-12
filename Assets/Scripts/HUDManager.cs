using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    class PlayerHUD
    {
        GameObject ParentObj;
        public Text Name;
        public Text HealthNum;
        public GameObject[] Keys = new GameObject[4];
        public GameObject[] Potions = new GameObject[4];

        public PlayerHUD(GameObject parent, Text name, Text health, GameObject[] keys, GameObject[] potions)
        {
            ParentObj = parent;
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

        GameObject HealthNotice;
        GameObject PauseScreen;

        int num = 1;

        void Start()
        {
            InitPlayerHUDs();

            PauseScreen = GameObject.Find("PauseScreen");
            PauseScreen.SetActive(false);

            HealthNotice = GameObject.Find("HealthNotification");
            HealthNotice.SetActive(false);
        }

        void Update()
        {
        }

        public void TogglePauseScreen()
        {
            bool stateToToggle = PauseScreen.activeSelf ? false : true;
            Debug.Log(stateToToggle);
            PauseScreen.SetActive(stateToToggle);
        }

        void InitPlayerHUDs()
        {
            for(int player = 1; player <= 4; player++)
            {
                GameObject parent = GameObject.Find("P" + player + "HUD");
                Text name = GameObject.Find("P" + player + "Name").GetComponent<Text>();
                Text health = GameObject.Find("P" + player + "HealthNum").GetComponent<Text>();
                if (SceneManager.Players[player] != null)
                {
                    health.text = SceneManager.Players[player].CurHitPoints.ToString();
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

                PlayerHUDs[player] = new PlayerHUD(parent, name, health, keys, pots);
            }
        }

        public void ShowLowHealth(int player)
        {
            StartCoroutine(LowHealthNotification(player));
        }

        IEnumerator LowHealthNotification(int player)
        {
            HealthNotice.SetActive(true);
            CanvasGroup cGroup = HealthNotice.GetComponent<CanvasGroup>();
            string notification = PlayerHUDs[player].Name.text + "s Health is running out!";
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
