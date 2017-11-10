using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BikePrototype : MonoBehaviour {

    LodeProtocol protocol;

    public Text speedText;

    float bikeSpeed;

	void Start () {
        protocol = GetComponent<LodeProtocol>();
        if (protocol == null) {
            Debug.LogError("Lode Protocol script not found!");
            return;
        }

        protocol.Connect();

        protocol.ResponseReceived += ProtocolUpdate;
        protocol.AddCall(LodeProtocol.GET_SPEED_FLOAT);
    }

    private void ProtocolUpdate(Protocol usedProt, string data) {
        switch (usedProt.command) {
            case LodeProtocol.GET_SPEED_FLOAT:
                try {
                    bikeSpeed = int.Parse(data);
                } catch (System.Exception) {
                    string t = "";
                    foreach (char c in data.ToCharArray()) {
                        t += (int)c + " ";
                    }
                    Debug.LogWarning("test: " + data + "|" + data.Length + "|" + t + "|" + data.ToCharArray().Length);
                    Debug.Log("protocol " + usedProt.command);
                }
                break;
        }
    }

    void Update () {
        speedText.text = string.Format("Current speed: {0}", bikeSpeed);

    }
}
