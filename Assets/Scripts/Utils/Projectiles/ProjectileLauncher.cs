using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour//must have to recieve RPC
{
    [Header("references")]
    [SerializeField] GameObject serverProjectile;//prefab ref for server
    [SerializeField] GameObject clientProjectile;//prefab ref for client
    [SerializeField] Transform projectileSpawnPoint;//we will add child obj to the tank barrel
                                                    //model to spawn it in a place that we want
                                                    //so we can read the position as to where to spawn it in
                                                    //and we can also read it rotation to decide what direction
                                                    //to fire projectile in.
    [SerializeField] InputReaderSO inputReaderSO;//need it to hook in to the fire event
    [Header("settings")]
    [Range(10f, 100f)]
    [SerializeField] float projectileSpeed = 10f;
    [Range(1f, 5f)]
    [SerializeField] float fireCoolDown = 1f;
    private bool fire;//will check wether or not we are allowed to fire

    public override void OnNetworkSpawn()//subscribing to firing event on network spawn
    {
        if (!IsOwner) return;
        inputReaderSO.PrimaryFireEvent += HandleFire;

    }
    private void HandleFire(bool fire)
    {
        this.fire = fire;//transfering the value to our local value
    }
    void Update()
    {
        if (!IsOwner) return;
        if (!fire) return;
        //here we spawning the dummy projectile for client and for server
        PrimaryFireServerRPC(projectileSpawnPoint.position,projectileSpawnPoint.up);
        SpawnClientProjectile(projectileSpawnPoint.position,projectileSpawnPoint.up);
        
       
    }
    private void SpawnClientProjectile(Vector3 spawnPos, Vector3 direction)//private void SpawnClientProjectile(Vector3 spawnPos, Vector3 direction)
    {
        
        GameObject projectileInstance = Instantiate(
                                                 clientProjectile,
                                                 spawnPos,
        /*Using Quaternion.identity 
        to not convert between vector2 and Quaternion
        we will do this in next line*/          Quaternion.identity
                                        );
        projectileInstance.transform.up = direction;//now it getting the turret direction
        //next is to do: add velocity to rigidbody

    }

    [ServerRpc]//Http post like header before method,it will take the same parametrs as a SpawnClientProjectile
    private void PrimaryFireServerRPC(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(
                                                serverProjectile,
                                                spawnPos,
                                                Quaternion.identity
                                       );
        projectileInstance.transform.up = direction;//now it getting the turret direction
        SpawnClientRPC(spawnPos, direction);
       
    }

    [ClientRpc]
    private void SpawnClientRPC(Vector3 spawnPos, Vector3 direction)
    {
        if (!IsOwner)
        {
            SpawnClientProjectile(spawnPos, direction);
        }
    }

    public override void OnNetworkDespawn()
    //unsubscribing to firing event on network despawn
    {
        if (!IsOwner) return;
        inputReaderSO.PrimaryFireEvent -= HandleFire;
    }
}
