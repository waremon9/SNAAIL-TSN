using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance;
    
    [Header("References")]
    public GameObject game;
    
    [Header("Prefabs")]
    public ProjectileNetwork projectile;

    private void Awake() {
        if (Instance == false) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }
}
