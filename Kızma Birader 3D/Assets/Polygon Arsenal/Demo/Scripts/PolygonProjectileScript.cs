﻿using UnityEngine;
using System.Collections;

namespace PolygonArsenal
{
    public class PolygonProjectileScript : MonoBehaviour
    {
        public float speed;
        public float size;
        public GameObject impactParticle;
        public GameObject projectileParticle;
        public GameObject muzzleParticle;
        public GameObject[] trailParticles;
        [Header("Adjust if not using Sphere Collider")]
        public float colliderRadius = 1f;
        [Range(0f, 1f)]
        public float collideOffset = 0.15f;
        public bool hasCollided;
        Rigidbody rigidbody;
        private Vector3 impactNormal;
        public bool isFireBallGame = false;

        void Start()
        { 
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
            projectileParticle.transform.localScale = Vector3.one * Random.Range(2,5);
            speed = Random.Range(3f, 5f);
            if (muzzleParticle)
            {
                muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
                Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
            }
            rigidbody = GetComponent<Rigidbody>();
        }
        public Vector3 shipForward=Vector3.up;
        void FixedUpdate()
        {
            RaycastHit hit;
            if(!isFireBallGame)
           rigidbody.velocity = shipForward  * speed;
            else
                rigidbody.velocity = transform.up * speed;
            float rad;
            if (transform.GetComponent<SphereCollider>())
                rad = transform.GetComponent<SphereCollider>().radius;
            else
                rad = colliderRadius;

            Vector3 dir = transform.GetComponent<Rigidbody>().velocity;
            if (transform.GetComponent<Rigidbody>().useGravity)
                dir += Physics.gravity * Time.deltaTime;
            dir = dir.normalized;

            float dist = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;

            if (Physics.SphereCast(transform.position, rad, dir, out hit, dist))
            {
                transform.position = hit.point + (hit.normal * collideOffset);

                GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;

                if (hit.transform.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
                {
                    Destroy(hit.transform.gameObject);
                }

                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }
                Destroy(projectileParticle, 3f);
                Destroy(impactP, 3.5f);
              //  Destroy(gameObject);

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
        }

        //private bool hasCollided = false;
        public int shipIndex = 0;
        void OnCollisionEnter(Collision hit)
        {
            //Debug.Log("GEMİNİN ADI: " + hit.collider.name);
            //if (!hasCollided)
            //{
            //    hasCollided = true;
            //    impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            //    //hit.gameObject.tag == "Destructible" 
            //    if (hit.collider.GetComponent<ShipControll>() && hit.collider.GetComponent<ShipControll>().ShipIndex!=shipIndex) // Projectile will destroy objects tagged as Destructible
            //    {
            //        // Destroy(hit.gameObject);
            //        hit.collider.GetComponent<ShipControll>().Death(shipIndex);
            //    }

            //    foreach (GameObject trail in trailParticles)
            //    {
            //        GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
            //        curTrail.transform.parent = null;
            //        Destroy(curTrail, 3f);
            //    }
            //    Destroy(projectileParticle, 3f);
            //    Destroy(impactParticle, 5f);
            //    Destroy(gameObject, 1f);

            //    ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
            //    //Component at [0] is that of the parent i.e. this object (if there is any)
            //    for (int i = 1; i < trails.Length; i++)
            //    {

            //        ParticleSystem trail = trails[i];

            //        if (trail.gameObject.name.Contains("Trail"))
            //        {
            //            trail.transform.SetParent(null);
            //            Destroy(trail.gameObject, 2f);
            //        }
            //    }
               
            //}
            //if (hit.gameObject.GetComponentInParent<FireArena.FireBallPlayer>())
            //    hit.gameObject.GetComponentInParent<FireArena.FireBallPlayer>().DiePlayer();
        }
        //private void OnCollisionEnter(Collision collision)
        //{
            
        //}
    }
}