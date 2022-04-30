using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;

namespace Network
{
    public class SteamLobby : MonoBehaviour
    {
        // Callbacks
        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> JoinRequest;
        protected Callback<LobbyEnter_t> LobbyEntered;

        // Variables
        public ulong CurrentLobbyID;
        private const string HostAddressKey = "HostAddress";
        public CustomNetworkManager manager;

        // Gameobject
        public GameObject HostButton;
        public Text LobbyNameText; // Need to add this to menu

        bool serverOn = false;
        bool clientOn = false;

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            if(!SteamManager.Initialized) { return; }

            manager = GetComponent<CustomNetworkManager>();
            
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        private void Update()
        {
            if ((clientOn || serverOn) && !manager.isNetworkActive) // When server turns off
            {
                ResetMultiplayerScene();
            }
        }

        public void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
        }

        public void DisconnectLobby()
        {
            if (serverOn) { manager.StopHost(); }
            if (clientOn) { manager.StopClient(); }
        }

        private void ResetMultiplayerScene()
        {
            Debug.Log("Disconnect from server");
            manager.StopClient();
            serverOn = false;
            clientOn = false;
            manager.MainMenuSceneChange();
        }

        // Callback functions for Steamworks (Listeners for events)
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK) { return; }

            Debug.Log("Lobby created successfully");

            manager.StartHost();
            serverOn = true;

            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName() + "'s Lobby");
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Request to Join Lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            // Everyone
            // HostButton.SetActive(false);
            CurrentLobbyID = callback.m_ulSteamIDLobby;
            // LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");

            // Client
            if (NetworkServer.active) { return; } // This is to check if you're a client. The Network server would not be active if you're a client but would be if you're a host.

            manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

            clientOn = true;
            manager.StartClient();
        }
    }
}