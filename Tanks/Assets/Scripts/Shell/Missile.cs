using UnityEngine;

namespace Shell
{
    public class Missile : MonoBehaviour
    {
        public LayerMask m_TankMask;
        public ParticleSystem m_ExplosionParticles;       
        public AudioSource m_ExplosionAudio;              
        public float m_MaxDamage = 100f;                  
        public float m_ExplosionForce = 1000f;
        public float m_ExplosionRadius = 5f;

        private Transform m_Target;
        
        private void Update()
        {
            MoveTowardsTarget();
        }

        public void SetTarget(Transform transform)
        {
            m_Target = transform;
        }
        
        private Vector3 GetTarget()
        {
            if (Vector3.Distance(transform.position, m_Target.position) <= 10)
            {
                 return m_Target.position + Vector3.up;
            }

            return m_Target.position;

        }

        private void MoveTowardsTarget()
        {
            
            transform.position = Vector3.MoveTowards(transform.position, GetTarget(), 10 * Time.deltaTime );

            transform.LookAt(GetTarget());
            
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Find all the tanks in an area around the shell and damage them.

            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                if (!targetRigidbody) { continue; }

                targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

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
}