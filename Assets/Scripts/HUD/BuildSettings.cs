using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildSettings : MonoBehaviour
{
    public GameObject player;
    private PlayerStats playerStats;


    bool choseDamage = false;
    bool choseHP = false;
    bool choseSpeed = false;

    bool chosePaper = false;
    bool choseStapler = false;
    bool choseRuler = false;

    public GameObject Pen;
    bool chosePen = false;

    public GameObject paper;
    public GameObject ruler;
    public GameObject stapler;

    private void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();

        if (choseDamage == true)
        {
            playerStats.ATKBuff += 0.1f;
        }

        if (choseHP == true)
        {
            playerStats.HPBuff += 1f;
        }

        if (choseSpeed == true)
        {
            playerStats.SPDBuff += 0.1f;
        }

        if (chosePaper == true)
        {
            PlayerAttack.projectile = paper;
            PlayerAttack.projectileIndex = 2;

        }

        if (choseStapler == true)
        {
            PlayerAttack.projectile = stapler;
            PlayerAttack.projectileIndex = 1;
        }

        if (choseRuler == true)
        {
            PlayerAttack.projectile = ruler;
            PlayerAttack.projectileIndex = 0;
        }

        if (chosePen == true)
        {
            Pen.SetActive(true);
        }
    }
  
    void Update()
    {
        
    }


    void BuildDamage()
    {
       choseDamage = true;
    }

    void BuildHP() 
    {
        choseHP = true;
    }

    void BuildSpeed()
    {
        choseSpeed = true;
    }

    void BuildRuler()
    {
        choseRuler = true;
    }

    void BuildStapler()
    {
        choseStapler = true;
    }

    void BuildPaper()
    {
        chosePaper = true;
    }

    void BuildPen()
    {
        PlayerUltimate.haveUlt=true;
        PlayerUltimate.ultIndex=1;
    }


    public void RefundBuild()
    {
        playerStats.HPBuff = 0;
        playerStats.SPDBuff = 1f;
        playerStats.ATKBuff = 1f;

        PlayerAttack.projectile = null;

        PlayerUltimate.haveUlt=false;;
        PlayerUltimate.ultIndex=0;
        choseDamage = false;
        choseHP = false;
        choseSpeed = false;

        chosePaper = false;
        choseStapler = false;
        choseRuler = false;
    }
}
