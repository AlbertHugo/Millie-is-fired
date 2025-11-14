using UnityEngine;

public class BuildSettings : MonoBehaviour
{
    public GameObject player;
    private PlayerStats playerStats;

    public GameObject buildPanel;

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

    [Header("UI Buttons")]
    public GameObject btnDamage;
    public GameObject btnHP;
    public GameObject btnSpeed;

    public GameObject btnRuler;
    public GameObject btnStapler;
    public GameObject btnPaper;

    public GameObject btnPen;

    private void Start()
    {

        choseDamage = PlayerPrefs.GetInt("choseDamage", 0) == 1;
        choseHP = PlayerPrefs.GetInt("choseHP", 0) == 1;
        choseSpeed = PlayerPrefs.GetInt("choseSpeed", 0) == 1;

        chosePaper = PlayerPrefs.GetInt("chosePaper", 0) == 1;
        choseStapler = PlayerPrefs.GetInt("choseStapler", 0) == 1;
        choseRuler = PlayerPrefs.GetInt("choseRuler", 0) == 1;

        chosePen = PlayerPrefs.GetInt("chosePen", 0) == 1;

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();

            if (choseDamage) playerStats.ATKBuff += 0.1f;
            if (choseHP) playerStats.HPBuff += 1f;
            if (choseSpeed) playerStats.SPDBuff += 0.1f;

            if (chosePaper)
            {
                PlayerAttack.projectile = paper;
                PlayerAttack.projectileIndex = 2;
            }

            if (choseStapler)
            {
                PlayerAttack.projectile = stapler;
                PlayerAttack.projectileIndex = 1;
            }

            if (choseRuler)
            {
                PlayerAttack.projectile = ruler;
                PlayerAttack.projectileIndex = 0;
            }

            if (chosePen)
                Pen.SetActive(true);
        }

        RefreshButtons();
    }



    public void BuildDamage()
    {
        choseDamage = true;
        PlayerPrefs.SetInt("choseDamage", 1);
        RefreshButtons();
    }

    public void BuildHP()
    {
        choseHP = true;
        PlayerPrefs.SetInt("choseHP", 1);
        RefreshButtons();
    }

    public void BuildSpeed()
    {
        choseSpeed = true;
        PlayerPrefs.SetInt("choseSpeed", 1);
        RefreshButtons();
    }

    public void BuildRuler()
    {
        choseRuler = true;
        PlayerPrefs.SetInt("choseRuler", 1);
        RefreshButtons();
    }

    public void BuildStapler()
    {
        choseStapler = true;
        PlayerPrefs.SetInt("choseStapler", 1);
        RefreshButtons();
    }

    public void BuildPaper()
    {
        chosePaper = true;
        PlayerPrefs.SetInt("chosePaper", 1);
        RefreshButtons();
    }

    public void BuildPen()
    {
        chosePen = true;
        PlayerPrefs.SetInt("chosePen", 1);
        PlayerUltimate.haveUlt = true;
        PlayerUltimate.ultIndex = 1;
        RefreshButtons();
    }


    public void RefundBuild()
    {
        if (player != null)
        {
            playerStats.HPBuff = 0;
            playerStats.SPDBuff = 1f;
            playerStats.ATKBuff = 1f;
        }

        PlayerAttack.projectile = null;

        chosePen = false;
        choseDamage = false;
        choseHP = false;
        choseSpeed = false;

        chosePaper = false;
        choseStapler = false;
        choseRuler = false;

        PlayerPrefs.DeleteKey("choseDamage");
        PlayerPrefs.DeleteKey("choseHP");
        PlayerPrefs.DeleteKey("choseSpeed");

        PlayerPrefs.DeleteKey("chosePaper");
        PlayerPrefs.DeleteKey("choseStapler");
        PlayerPrefs.DeleteKey("choseRuler");

        PlayerPrefs.DeleteKey("chosePen");

        PlayerPrefs.Save();

        RefreshButtons();
    }

    void RefreshButtons()
    {

        bool anyUpgradeChosen = choseDamage || choseHP || choseSpeed;

        btnDamage.SetActive(!anyUpgradeChosen || choseDamage);
        btnHP.SetActive(!anyUpgradeChosen || choseHP);
        btnSpeed.SetActive(!anyUpgradeChosen || choseSpeed);


        bool anyWeaponChosen = choseRuler || choseStapler || chosePaper;

        btnRuler.SetActive(!anyWeaponChosen || choseRuler);
        btnStapler.SetActive(!anyWeaponChosen || choseStapler);
        btnPaper.SetActive(!anyWeaponChosen || chosePaper);


        PlayerUltimate.haveUlt = false; 
        PlayerUltimate.ultIndex = 0;
    }

    public void HideBuildPanel()
    {
        buildPanel.SetActive(false);
    }
}
