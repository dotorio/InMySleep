using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonButton : MonoBehaviour
{
    public Canon canon; // Canon 객체

    private void OnDestroy()
    {
        if (canon != null)
        {
            canon.OnBreakableDestroyed(); // Breakable 객체가 파괴될 때 Canon에 신호를 보냄
        }
    }
}
