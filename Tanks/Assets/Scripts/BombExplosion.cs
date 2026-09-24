using System;
using System.Collections;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage = 150f;
    public float m_ExplosionForce = 1500f;
    public float m_MaxLifeTime = 3f;
    public float m_ExplosionRadius = 5f;

    Color colorOff = Color.black;
    Color colorOn = Color.red;
    public float colorChangeSpeed = 3;

    private Material bombMaterial;

    public GameObject range;

    private Material rangeColor;
    
    void Start()
    {
        bombMaterial = GetComponent<Renderer>().material;
        Destroy(gameObject, m_MaxLifeTime);

        range.transform.localScale = new Vector3(m_ExplosionRadius, m_ExplosionRadius, 1);

         rangeColor = range.GetComponent<Renderer>().material;

    }

    private void Update()
    {
        bombMaterial.color = Color.Lerp(colorOff, colorOn, Mathf.PingPong(Time.time * colorChangeSpeed, 1));
        
        rangeColor.color = Color.Lerp(colorOn, colorOff, Mathf.PingPong(Time.time * colorChangeSpeed, 1));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        StartCoroutine(Explosion());

        
    }

    private IEnumerator Explosion()
    {
        
        yield return new WaitForSecondsRealtime(2);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody) { continue; }

            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth) { continue; }

            float damage = CalculateDamage(targetRigidbody.position);

            targetHealth.TakeDamage(damage);
        }

        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
        Destroy(gameObject);
        
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 exsplosionToTarget = targetPosition - transform.position;

        float exsplosionDistance = exsplosionToTarget.magnitude;
        float relativeDistance = (m_ExplosionRadius - exsplosionDistance) / m_ExplosionRadius;

        float damage = relativeDistance * m_MaxDamage;
        damage = Mathf.Max(0f, damage);

        return damage;
    }
}
