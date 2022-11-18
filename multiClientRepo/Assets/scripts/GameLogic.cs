using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;

    public static GameLogic Singleton{
        get=>_singleton;
        private set{
            if(_singleton==null){
                _singleton=value;
            }
            else if(_singleton!=value){
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    public GameObject playerTeamMate;
    public GameObject player;
    public GameObject bullet;

    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;

    private void Awake(){
        Singleton=this;
    }

    [MessageHandler((ushort)ServerToClientId.playerDied)]

    public static void PlayerDied(Message message)
    {
        ushort deadPlayerId = message.GetUShort();
        //Vector3 bulletVelocity = message.GetVector3();
        Vector3 bulletVelocity = new Vector3(0, 0, 0);
        Player.Kill(deadPlayerId, bulletVelocity);
    }
}
