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
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Collider2D playerCollider;//our player collider to not collide when we firing with ourselvs
    [SerializeField] InputReaderSO inputReaderSO;//need it to hook in to the fire event
    [Header("settings")]
    [Range(10f, 100f)]
    [SerializeField] float projectileSpeed = 10f;
    [Range(1f, 5f)]
    [SerializeField] float fireRate = 1f;
    [SerializeField] float muzzleFlashDuration=0.5f;
    float perviousFireTime;
    float muzzleFlashTimer;
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
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if (muzzleFlashTimer <= 0f) muzzleFlash.SetActive(false);
        }


        if (!IsOwner) return;
        if (!fire) return;
        //checing if we allowed to fire (FIRERATE)
        if (Time.time/*time of the game*/< 1 / fireRate + perviousFireTime) { return; }
        //here we spawning the dummy projectile for client and for server
        PrimaryFireServerRPC(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnClientProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
        perviousFireTime = Time.time; //potential issue if server is 
                                     // on for years without restart we will have overflow 
                                    //max value of the float number
        
       
    }
    private void SpawnClientProjectile(Vector3 spawnPos, Vector3 direction)//private void SpawnClientProjectile(Vector3 spawnPos, Vector3 direction)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;//adding timer value(in update we will coundown every tick until<=0 and disable it)
        GameObject projectileInstance = Instantiate(
                                                 clientProjectile,
                                                 spawnPos,
        /*Using Quaternion.identity 
        to not convert between vector2 and Quaternion
        we will do this in next line*/          Quaternion.identity
                                        );
        projectileInstance.transform.up = direction;//now it getting the turret direction
                                                    //next is to do: add velocity to rigidbody
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());//ignoring own collider with own projectile:

        /**ADDING VELOCITY TO PROJECTILE, BY USING RIGIDBODY 2D OF IT
            also perfroming check if there's rigidbody and will use out for extracting
        */
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up*projectileSpeed;
        }
        
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
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());//ignoring own collider with own projectile:
        
        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up*projectileSpeed;
        }
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
