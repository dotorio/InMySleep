using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class StartController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startBtn;
    public GameObject waitingBtn;
   

    // Update is called once per frame
    public void buttonUpdate()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startBtn.SetActive(true);
            waitingBtn.SetActive(false);
        }
        else
        {
            startBtn.SetActive(false);
            waitingBtn.SetActive(true);
        }
    }

}
