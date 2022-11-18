using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HPcontroller : MonoBehaviour
{
    public TextMeshProUGUI hpText,heartText;
    Color goodHPColor = new Color(0,255,0,255);
    Color mediumHPColor = new Color(255, 255, 0, 255);
    Color lowHealthColor = new Color(255, 0, 0, 255);
    int maxHealth = 100;
    int goodHealth = 75;
    int mediumHealth = 35;
    int lowHealth = 0;

    private void Start()
    {
        hpText.color = new Color(0, 255, 0, 255);
    }
    public void ChangeHP(int hp)
    {
        hpText.text = hp.ToString();
        if (hp > goodHealth)
        {
            hpText.color = goodHPColor;
            heartText.color = goodHPColor;
        }
        else if (hp > mediumHealth)
        {
            hpText.color = mediumHPColor;
            heartText.color = mediumHPColor;
        }
        else if (hp >= lowHealth)
        {
            hpText.color = lowHealthColor;
            heartText.color = lowHealthColor;
        }

    }
}
