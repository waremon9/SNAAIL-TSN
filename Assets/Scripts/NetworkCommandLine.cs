using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class NetworkCommandLine : MonoBehaviour {
    private NetworkManager _netManager;

    private void Start() {
        this._netManager = this.GetComponentInParent<NetworkManager>();

        if (Application.isEditor) {
            return;
        }

        Dictionary<string, string> args = this.GetCommandlineArgs();

        if (args.TryGetValue("-mode", out string mode)) {
            switch (mode) {
                case "server":
                    this._netManager.StartServer();
                    break;
                case "host":
                    this._netManager.StartHost();
                    break;
                case "client":

                    this._netManager.StartClient();
                    break;
            }
        }
    }

    private Dictionary<string, string> GetCommandlineArgs() {
        Dictionary<string, string> argDictionary = new();

        string[] args = Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i) {
            string arg = args[i].ToLower();
            if (arg.StartsWith("-")) {
                string value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = value?.StartsWith("-") ?? false ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}