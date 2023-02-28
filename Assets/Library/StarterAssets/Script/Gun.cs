using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{
    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpredVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem ShootingSystem;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private float ShootDelay = 0.5f;
    private LayerMask Mask;
    private Animator Animator;
    private float LastShootTime;

    void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            ShootingSystem.Play();
            Vector3 dicrection = GetDirection();
            if(Physics.Raycast(BulletSpawnPoint.position, dicrection, out RaycastHit hit, float.MaxValue, Mask)){
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                LastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection(){
        Vector3 direction = transform.forward;
        if(AddBulletSpread){
            direction += new Vector3(
                Random.Range(-BulletSpredVariance.x, BulletSpredVariance.x),
                Random.Range(-BulletSpredVariance.y, BulletSpredVariance.y),
                Random.Range(-BulletSpredVariance.z, BulletSpredVariance.z)
            );
            direction.Normalize();
        }
        return direction;
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit){
        float time = 0;
        Vector3 startPos = Trail.transform.position;
        while(time < 1){
            Trail.transform.position = Vector3.Lerp(startPos, Hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = Hit.point;
        Instantiate(ImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }
}
