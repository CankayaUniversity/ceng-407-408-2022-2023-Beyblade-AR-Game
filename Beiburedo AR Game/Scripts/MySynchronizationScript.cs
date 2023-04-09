using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MySynchronizationScript : MonoBehaviour, IPunObservable
{

    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
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
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, Time.fixedDeltaTime);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, Time.fixedDeltaTime * 100);
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//player controling
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
        }
        else// remote player
        {
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
