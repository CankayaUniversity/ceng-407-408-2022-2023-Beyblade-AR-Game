using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySynchronizationScript : MonoBehaviour, IPunObservable
{
    private Rigidbody rb;
    private PhotonView photonView;

    private Vector3 networkedPosition;
    private Quaternion networkedRotation;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;
    
    private GameObject battleArenaGameobject;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();
        
        battleArenaGameobject = GameObject.Find("BattleArena");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        if (!photonView.IsMine)
        {
            // SerializationRate is the number of times per second OnPhotonSerializeView is called.
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance * (1.0f / PhotonNetwork.SerializationRate)); 
            
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate) );
        }
        

    }

    // PhotonStream is a container class that sends and receives data from and to a PhotonView
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        // This isWriting bool parameter will be true if the client is the owner of the PhotonView
        if (stream.IsWriting) // Sending data
        {
            // PhotonView is mine & I am the one who control this player
            // Should send position, velocity, rotation etc. data to other players
            stream.SendNext(rb.position - battleArenaGameobject.transform.position);
            stream.SendNext(rb.rotation);
            
            
            // Lag compensation
            if(synchronizeVelocity) 
                stream.SendNext(rb.velocity);
            
            if(synchronizeAngularVelocity) 
                stream.SendNext(rb.angularVelocity);
            
        }
        else // Receiving data
        {
            // Called on my player gameobject that exists in remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext() + battleArenaGameobject.transform.position;
            networkedRotation = (Quaternion)stream.ReceiveNext();


            
            // Lag compensation
            if (isTeleportEnabled)
            {
                /*
                 * So, if the distance is greater than a certain value, this means we need to take an action
                 * Because, for example, if the distance is bigger than 1, the player will appear very far away from its real position and it will effect the gamepla
                 */
                if (Vector3.Distance(rb.position, networkedPosition) > teleportIfDistanceGreaterThan)
                    rb.position = networkedPosition; // Fixing position
            }
            

            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                /*
                 * PhotonNetwork.Time is used to synchronize time for all players.
                    Since it is actually the server time, it will be the same in each client.
                 */
                
                // Delay time between sending and the receving the data (ping)
                float lag = Math.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();

                    networkedPosition += rb.velocity * lag; // Accuracy of networkedPosition by taking velocity and lag into account

                    distance = Vector3.Distance(rb.position, networkedPosition);
                }
                
                if (synchronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();

                    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;

                    angle = Quaternion.Angle(rb.rotation, networkedRotation);
                }
            }
        }
    }
}
