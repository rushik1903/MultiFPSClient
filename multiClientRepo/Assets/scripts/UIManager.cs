using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;
using RiptideNetworking.Utils;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton{
        get=>_singleton;
        private set{
            if(_singleton==null){
                _singleton=value;
            }
            else if(_singleton!=value){
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField,teamNumberField;

    private void Awake() {
        Singleton=this;
    }

    public void ConnectClicked(){
        usernameField.interactable=false;
        teamNumberField.interactable = false;
        connectUI.SetActive(false);

        NetworkManager.Singleton.Connect();
    }

    public void BackToMain(){
        usernameField.interactable=true;
        teamNumberField.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName(){
        PlayerPrefs.SetString("localMainPlayerName", usernameField.text);
        Message message = Message.Create(MessageSendMode.reliable,(ushort)ClientToServerId.name);
        message.AddString(usernameField.text);
        message.AddInt(int.Parse(teamNumberField.text));
        Player.TeamNumber = int.Parse(teamNumberField.text);
        NetworkManager.Singleton.Client.Send(message);
        PlayerPrefs.SetInt("TeamNumber", int.Parse(teamNumberField.text));
    }
}
