using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilitiesManager : MonoBehaviour
{
    [SerializeField] private PlayerBattle[] playerBattle;
    [SerializeField] private EnemyBattle enemyBattle;
    [SerializeField] private Button[] powerButtons;

    public static bool isShield;
    private bool IsCalled = false;

    private bool buttonsDisabled;
    private float disableTime;
    public ParticleSystem[] ParticleSystem;

    public void Start()
    {
        IsCalled = false;
        ParticleSystem[PlayerPrefs.GetInt("PlayerSelectionNumber")].Stop();
    }
    private void Update()
    {
        if (buttonsDisabled && Time.time >= disableTime + 5f)
        {
            EnableButtons();
          
        }
    }

    private void DisableButtons()
    {
        foreach (Button button in powerButtons)
        {
            button.interactable = false;
        }
        buttonsDisabled = true;
        disableTime = Time.time;
        ParticleSystem[PlayerPrefs.GetInt("PlayerSelectionNumber")].Play();
    }

    private void EnableButtons()
    {
        foreach (Button button in powerButtons)
        {
            button.interactable = true;
        }
        buttonsDisabled = false;
        ParticleSystem[PlayerPrefs.GetInt("PlayerSelectionNumber")].Stop();
    }

    public void SpeedBooster()
    {
       

        int playerSelectionNumber = PlayerPrefs.GetInt("PlayerSelectionNumber");
        PlayerBattle selectedPlayer = playerBattle[playerSelectionNumber];
        if (playerSelectionNumber == 1 || playerSelectionNumber == 2)
        {
            if (selectedPlayer.currentSpinSpeed <= 3400)
            {
                DisableButtons();
               
                selectedPlayer.currentSpinSpeed += 200;
                selectedPlayer.spinSpeedRatio_Text.text = selectedPlayer.currentSpinSpeed + "/" + selectedPlayer.startSpinSpeed;
                selectedPlayer.spinSpeedBar_Image.fillAmount += 0.05f;
            }
        } 
        else
        {
            if (selectedPlayer.currentSpinSpeed <= 4200)
            {
                DisableButtons();
                
                selectedPlayer.currentSpinSpeed += 200;
                selectedPlayer.spinSpeedRatio_Text.text = selectedPlayer.currentSpinSpeed + "/" + selectedPlayer.startSpinSpeed;
                selectedPlayer.spinSpeedBar_Image.fillAmount += 0.05f;
            }
        }
    }

    public void Heal()
    {
        

        int playerSelectionNumber = PlayerPrefs.GetInt("PlayerSelectionNumber");
        PlayerBattle selectedPlayer = playerBattle[playerSelectionNumber];
        if (!IsCalled && selectedPlayer.currentSpinSpeed <= 500)
        {
            DisableButtons();
            IsCalled = true;
            selectedPlayer.currentSpinSpeed += 800;
            selectedPlayer.spinSpeedRatio_Text.text = selectedPlayer.currentSpinSpeed + "/" + selectedPlayer.startSpinSpeed;
            selectedPlayer.spinSpeedBar_Image.fillAmount += 0.35f;
        }
    }

    public void Shield()
    {

        if (!isShield)
        {
            DisableButtons();
           
            isShield = true;
            StartCoroutine(DisableShieldAfterDelay(5f));
        }
    }

    private IEnumerator DisableShieldAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShield = false;
        EnableButtons();
    }

    public void Slow()
    {
        DisableButtons();
       
        enemyBattle.currentSpinSpeed -= 200;
    }
}
