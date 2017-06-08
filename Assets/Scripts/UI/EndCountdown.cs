using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCountdown : MonoBehaviour {

    private CountdownCanvas CDS;

    public void SetCountdownNow()
    {
        GameController.SetOff();
        CountdownCanvas.Hide();
    }
}
