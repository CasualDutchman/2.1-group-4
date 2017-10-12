using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LodeProtocol))]
public class BikeManager : MonoBehaviour {

    public bool useProtocol;

    LodeProtocol protocol;
    WorldManager manager;

    float bikeSpeed;

    void Start () {
        if (useProtocol) {
            protocol = GetComponent<LodeProtocol>();
            manager = GetComponent<WorldManager>();

            if (protocol == null) {
                Debug.LogError("Lode Protocol script not found!");
                return;
            }

            protocol.Connect();

            protocol.ResponseReceived += ProtocolGetSpeed;
            protocol.AddCall(LodeProtocol.GET_SPEED_FLOAT);
        }
    }

    void Update() {
        if (useProtocol) {
            manager.speed = bikeSpeed;
        }
    }

    private void ProtocolGetSpeed(Protocol usedProt, string data) {
        switch (usedProt.command) {
            case LodeProtocol.GET_SPEED_FLOAT:
                try {
                    bikeSpeed = int.Parse(data) / 10;
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
}
