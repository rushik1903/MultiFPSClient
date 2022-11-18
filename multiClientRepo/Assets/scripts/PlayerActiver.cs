using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class PlayerActiver : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    public RemoveSpawnBarriers removeWallsScript;

    [MessageHandler((ushort)ServerToClientId.newRound)]

    private static void NewRound(Message message)
    {
        int roundPhase = message.GetInt();
        if (roundPhase == 0)
        {
            GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<RemoveSpawnBarriers>().RebuildSpawnWalls();
            Debug.Log("newRound");
            foreach (Player player in list.Values)
            {
                //turning off spectating cams
                player.transform.Find("Camera").gameObject.SetActive(false);
                //visibiling player torso
                if(player.username == PlayerPrefs.GetString("localMainPlayerName"))
                {
                    continue;
                }

                //player.gameObject.GetComponentInChildren<SpawnEffect>().AnimBackToVisible();
                player.transform.Find("Capsule").GetComponent<SpawnEffect>().AnimBackToVisible();

                //visibiling player head
                player.transform.Find("Capsule").transform.Find("Sphere").GetComponent<SpawnEffect>().AnimBackToVisible();
            }
            GameObject.FindGameObjectWithTag("localPlayer").transform.Find("Camera").gameObject.SetActive(true);
        }
        else if (roundPhase == 1)
        {
            Debug.Log("shootStrated");
            GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<RemoveSpawnBarriers>().RemoveSpawnWalls();
        }
    }
}
