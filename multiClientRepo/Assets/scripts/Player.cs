using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Player : MonoBehaviour
{

    public static int TeamNumber;

    public int localTeamNumber;
    Renderer _renderer;
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id{ get; private set; }
    public bool IsLocal{ get; private set; }

    [SerializeField] private Transform camTransform;
    [SerializeField] private Interpolator interpolator;
    [SerializeField] private SpawnEffect spawnEffect;

    public string username;

    private void OnDestroy(){
        //list.Remove(Id);
    }

    [MessageHandler((ushort)ServerToClientId.playerHP)]

    private static void health(Message message)
    {
        int health = message.GetInt();
        GameObject.FindGameObjectWithTag("HPController").GetComponent<HPcontroller>().ChangeHP(health);
    }

    private void Move(ushort tick, bool isTeleport, Vector3 newPosition, Vector3 forward){
        interpolator.NewUpdate(tick, isTeleport, newPosition);
        //transform.position = newPosition;    //dierectly teleporting the player to incoming location
        if(!IsLocal){
            camTransform.forward = forward;
        }
    }

    public static void Spawn(ushort id, int teamNumber, string username, Vector3 position){
        Player player;
        GameObject playerObject;
        if(id==NetworkManager.Singleton.Client.Id){
            //playerObject = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity);
            playerObject = GameObject.FindGameObjectWithTag("localPlayer");
            player = playerObject.GetComponent<Player>();
            playerObject.GetComponent<CharacterController>().enabled=false;
            playerObject.transform.position = position;
            playerObject.GetComponent<CharacterController>().enabled=true;
            player.IsLocal = true;
        }else{
            if (teamNumber == TeamNumber)
            {
                player = Instantiate(GameLogic.Singleton.playerTeamMate, position, Quaternion.identity).GetComponent<Player>();
            }
            else
            {
                player = Instantiate(GameLogic.Singleton.player, position, Quaternion.identity).GetComponent<Player>();
            }
            player.IsLocal = false;
        }

        player.name = $"Player {id} {(string.IsNullOrEmpty(username) ? "Guest" : username)}";
        player.Id=id;
        player.username=username;
        player.localTeamNumber = teamNumber;

        list.Add(id, player);
        PlayerActiver.list.Add(id, player);
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]

    private static void SpawnPlayer(Message message){
        Spawn(message.GetUShort(), message.GetInt(), message.GetString(), message.GetVector3());
    }


    [MessageHandler((ushort)ServerToClientId.playerMovement)]

    private static void PlayerMovement(Message message){
        if(list.TryGetValue(message.GetUShort(),out Player player)){
            player.Move(message.GetUShort(), message.GetBool(), message.GetVector3(), message.GetVector3());
        }
    }

    [MessageHandler((ushort)ServerToClientId.playerReconnect)]

    private static void PlayerReconnect(Message message)
    {
        ushort oldId = message.GetUShort();
        ushort newId = message.GetUShort();
        foreach(Player player in list.Values)
        {
            if (player.Id==oldId)
            {
                if (!player.IsLocal)
                {
                    Player temp = player;
                    list.Remove(player.Id);
                    list[newId] = temp;

                    player.Id = newId;
                    Debug.Log(oldId.ToString() + newId.ToString());
                    return;
                }
            }
        }
        Debug.Log(oldId.ToString() + newId.ToString());
    }

    public static void Kill(ushort id,Vector3 bulletVelocity)
    {
        if(id == NetworkManager.Singleton.Client.Id)
        {
            SpectateAfterDeath();
        }
        foreach(Player otherplayer in list.Values)
        {
            if (otherplayer.Id == id)
            {
                Transform child = otherplayer.gameObject.transform.Find("Capsule");
                child.GetComponent<SpawnEffect>().StartAnimation(); //starting animation for body
                Transform childHead = child.Find("Sphere");
                childHead.GetComponent<SpawnEffect>().StartAnimation(); //starting animation for head
            }
        }
    }

    private static void SpectateAfterDeath()
    {
        List<GameObject> spectatingCams = new List<GameObject>();
        foreach (Player player in list.Values)
        {
            if (player.username == PlayerPrefs.GetString("localMainPlayerName"))
            {
                player.gameObject.transform.Find("Camera").gameObject.SetActive(false);
            }else if (player.localTeamNumber == TeamNumber)
            {
                spectatingCams.Add(player.gameObject.transform.Find("Camera").gameObject);
            }
        }
        foreach(GameObject cams in spectatingCams)
        {
            cams.SetActive(true);
        }
    }



    [MessageHandler((ushort)ServerToClientId.bullets)]
    private static void Bullets(Message message)
    {
        Vector3 bulletPosition = message.GetVector3();
        Vector3 bulletVelocity = message.GetVector3();
        GameObject bullet = Instantiate(GameLogic.Singleton.bullet, bulletPosition, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = bulletVelocity;
    }
}
