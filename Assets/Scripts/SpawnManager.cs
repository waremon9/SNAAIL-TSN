using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{


    public GameObject target;


    private int _currentEnemy;


    private int _i = 0;
    public List<RoundEnemies> rounds;

    float _spawnTimerBase = 2;
    float _spawnTimer;
    float _roundTimer;
    

    private static SpawnManager instance = null;
    public static SpawnManager Instance => instance;


    private void Awake()
    {
        if (IsServer==false)
        {
            return;
        }
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        

    }
    // Start is called before the first frame update
    void Start()
    {

        _spawnTimer = _spawnTimerBase;
        _roundTimer = rounds[_i].timerRound;

    }



    // Update is called once per frame
    void Update()
    {

        Debug.Log(IsServer);
        Debug.Log(IsHost);

        if (IsServer == false)
        {
            return;
        }

        Debug.Log("coucou");
        if (_spawnTimer > 0)
            {
                _spawnTimer -= Time.deltaTime;
                if (_spawnTimer < 0)
                {
                    _spawnTimer = rounds[_i].spawnCD;
                    if (_currentEnemy < rounds[_i].maxEnemy)
                    {
                        Debug.Log("coucou2");
                        _currentEnemy++;
                        SpawnClientRpc();                       
                    
                    }

                }
            }



            if (_roundTimer > 0)
            {
                _roundTimer -= Time.deltaTime;
                if (_roundTimer < 0)
                {
                    
                    if (_i <= rounds.Count-1)
                    {
                        if (_i<rounds.Count-1)
                        {
                            _i++;
                        }
                        _roundTimer = rounds[_i].timerRound;

                    }

                }
            }
        
    }
    [ClientRpc]
    private void SpawnClientRpc()
    {

        GameObject nextEnemy = rounds[_i].enemies[Random.Range(0, rounds[_i].enemies.Count)];

        Instantiate(nextEnemy, rounds[_i].spawner[Random.Range(0, rounds[_i].spawner.Count)].transform.position, Quaternion.identity);
        nextEnemy.GetComponent<AEnemy>().target = target;
        nextEnemy.GetComponent<NetworkObject>().Spawn(true);





    }

}

[System.Serializable]
public class RoundEnemies
{
    public List<GameObject> enemies;
    public List<GameObject> spawner;
    public float spawnCD;
    public float timerRound;
    public int maxEnemy;
}