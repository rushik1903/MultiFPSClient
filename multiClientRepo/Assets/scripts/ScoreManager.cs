using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private static TMP_Text myTeamScoreText, OppTeamScoreText;
    [SerializeField] private static GameObject gamingCanavs;
    private static int myTeamScore = 0, OppTeamScore = 0;

    private void Start()
    {
        gamingCanavs = GameObject.FindGameObjectWithTag("gamingCanvas");
        myTeamScoreText = gamingCanavs.transform.Find("MyTeamScore").GetComponent<TMP_Text>();
        OppTeamScoreText = gamingCanavs.transform.Find("OppTeamScore").GetComponent<TMP_Text>();
    }

    [MessageHandler((ushort)ServerToClientId.teamScore)]

    private static void teamScore(Message message)
    {
        myTeamScore = message.GetInt();
        OppTeamScore = message.GetInt();
        
        UpdateScores();
    }

    private static void UpdateScores()
    {
        myTeamScoreText.text = myTeamScore.ToString();
        OppTeamScoreText.text = OppTeamScore.ToString();
    }
}
