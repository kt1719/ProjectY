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
        protected Callback<P2PSessionConnectFail_t> LobbyDisconnected;

        // Variables
        public ulong CurrentLobbyID;
        private const string HostAddressKey = "HostAddress";
        private CustomNetworkManager manager;

        // Gameobject
        public GameObject HostButton;
        public Text LobbyNameText;

        private void Start()
        {
            if(!SteamManager.Initialized) { return; }

            manager = GetComponent<CustomNetworkManager>();
            
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            LobbyDisconnected = Callback<P2PSessionConnectFail_t>.Create(OnLobbyDisconnected);
        }

        public void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK) { return;  }

            Debug.Log("Lobby created successfully");

            manager.StartHost();

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
            HostButton.SetActive(false);
            CurrentLobbyID = callback.m_ulSteamIDLobby;
            LobbyNameText.gameObject.SetActive(true);
            LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");

            // Client
            if (NetworkServer.active) { return; } // This is to check if you're a client. The Network server would not be active if you're a client but would be if you're a host.

            manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

            manager.StartClient();
        }

        private void OnLobbyDisconnected(P2PSessionConnectFail_t callback)
        {
            manager.StopClient();
            manager.Reset();

            HostButton.SetActive(true);
            LobbyNameText.gameObject.SetActive(false);
            LobbyNameText.text = "";
        }
    }
}