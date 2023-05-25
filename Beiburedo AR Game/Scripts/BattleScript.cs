using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleScript : MonoBehaviourPun
{
    public Spinner spinnerScript;

    public GameObject uI_3D_Gameobject;
    public GameObject deathPanelUIPrefab;
    private GameObject deathPanelUIGameobject;
    
    private Rigidbody rb;
    
    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text;

    public float common_Damage_Coefficient = 0.04f;

    public bool isAttacker;
    public bool isDefender;
    private bool isDead = false;

    [Header("Player Type Damage Coefficients")]
    public float doDamage_Coefficient_Attacker = 10f; // Do more damage than defender - ADVANTAGE
    public float getDamaged_Coefficient_Attacker = 1.2f; // Gets more damage - DISADVANTAGE
    
    public float doDamage_Coefficient_Defender = 0.75f; // Do less damage - DISADVANTAGE 
    public float getDamaged_Coefficient_Defender = 0.2f; // Gets less damage - ADVANTAGE

    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }else if (gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;

            /*
             * If attacker and defender types have the same amount of health(spin speed),
             * attacker type will have more advantage than defender type so we set the defender type's health upper
             */
            spinnerScript.spinSpeed = 4400;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;
        
            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
            spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
        }
    }

    // This method is called when the collider of this game object has begun touching another collider
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (photonView.IsMine) // Instantiate sfx and vfx for local player only
            {
                Vector3 effectPosition = (gameObject.transform.position + collision.transform.position) / 2 + new Vector3(0, 0.05f, 0);

                //Instantiate Collision Effect ParticleSystem
                GameObject collisionEffectGameobject = GetPooledObject();
                if (collisionEffectGameobject != null)
                {
                    collisionEffectGameobject.transform.position = effectPosition;
                    collisionEffectGameobject.SetActive(true);
                    collisionEffectGameobject.GetComponentInChildren<ParticleSystem>().Play();

                    //De-activate Collision Effect Particle System after some seconds.
                    StartCoroutine(DeactivateAfterSeconds(collisionEffectGameobject, 0.5f));

                }
            }
            
            
            // Comparing speeds of the beyblades
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            
            Debug.Log("My speed: " + mySpeed + "-------------- Other player speed: " + otherPlayerSpeed);

            if (mySpeed > otherPlayerSpeed)
            {
                Debug.Log("YOU DAMAGED to the other player!!!");
                
                float default_Damage_Amount =
                    gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * common_Damage_Coefficient;


                if (isAttacker) { default_Damage_Amount *= doDamage_Coefficient_Attacker; }
                else if (isDefender) { default_Damage_Amount *= doDamage_Coefficient_Defender; }
                
                // If we don't make this if check below, this DoDamage RPC method will be called for every player that got hit in both local and remote players game
                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    // Apply damage to the slower player
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_Damage_Amount); // RPCs are the method calls on the remote clients in the same room
                }
                
            }
        }
    }
    
    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if (!isDead)
        {
            if (isAttacker)
            {
                _damageAmount *= getDamaged_Coefficient_Attacker;

                if (_damageAmount > 1000)
                {
                    _damageAmount = 400f;
                }
            }
            else if (isDefender)
            {
                _damageAmount *= getDamaged_Coefficient_Defender;

            }

            spinnerScript.spinSpeed -= _damageAmount;
            currentSpinSpeed = spinnerScript.spinSpeed;
        
            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
            spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed; // This F0 method will make sure that we will see the current spin speed always with no decimal places like as 3000, 3500 etc.

            if (currentSpinSpeed < 100)
            {
                // Die
                Die();
            }
        }
        

    }

    void Die()
    {
        isDead = true;
        
        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        spinnerScript.spinSpeed = 0f;
        
        uI_3D_Gameobject.SetActive(false);

        if (photonView.IsMine)
        {
            // Countdown for respawn
            StartCoroutine(ReSpawn());
        }
        
        
    }

    IEnumerator ReSpawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");

        // Game can't instantiate this death panel gameobject everytime player get killed, it's non-sense, so check if it is null
        if (deathPanelUIGameobject == null)
        {
            // With this line of code death panel UI object will be instantiated under canvas object in the scene
            deathPanelUIGameobject = Instantiate(deathPanelUIPrefab, canvasGameObject.transform); 
        }
        // If not null, this means game already instantiated death panel so we will just activate it since it will be probably be de-active
        else
        {
            deathPanelUIGameobject.SetActive(true);
        }

        Text respawnTimeText = deathPanelUIGameobject.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8.0f;

        respawnTimeText.text = respawnTime.ToString(".00");

        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00");


            GetComponent<MovementController>().enabled = false;
        }
        
        deathPanelUIGameobject.SetActive(false);
        
        GetComponent<MovementController>().enabled = true;

        photonView.RPC("ReBorn",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ReBorn()
    {
        spinnerScript.spinSpeed = startSpinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        
        uI_3D_Gameobject.SetActive(true);

        isDead = false;

    }
    
    public List<GameObject> pooledObjects;
    public int amountToPool = 8;
    public GameObject CollisionEffectPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();
        rb = GetComponent<Rigidbody>();
        
        
        if (photonView.IsMine)
        {
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(CollisionEffectPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public GameObject GetPooledObject()
    {
        
        for (int i = 0; i < pooledObjects.Count; i++)
        {
           
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
       
        return null;
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);

    }


}
