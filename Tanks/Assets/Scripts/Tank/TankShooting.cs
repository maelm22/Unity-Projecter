using System.Collections;
using Shell;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

    private Vector3 m_MiniGun;
    
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;
    private bool m_ShellAvailiable = true;

    public Rigidbody m_Bomb;
    public Transform m_BombTransform;
    private bool m_BombAvailiable = true;
    private string m_BombButton;


    public Rigidbody m_MiniShell;
    public Transform m_MiniTransform;
    private string m_MiniGunButton;
    private bool canShoot = true;
    
    public Rigidbody m_Missile;
    private string m_MissileButton;
    private bool MissileAvailiable = true;
    private Transform m_Target;
     
    
   // private RaycastHit hit;
    
    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_MiniGun = m_MaxLaunchForce * m_FireTransform.forward;
        
        m_MiniGunButton = "Minigun" + m_PlayerNumber;
        
        m_FireButton = "Fire" + m_PlayerNumber;

        m_BombButton = "Bomb" + m_PlayerNumber;

        m_MissileButton = "Missile" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.

        m_AimSlider.value = m_MinLaunchForce;

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired && m_ShellAvailiable)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();

        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
            
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();

        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired && m_ShellAvailiable)
        {
            Fire();
        }

        if (Input.GetButtonDown(m_BombButton) && m_BombAvailiable)
        {
            Bomb();
        }
        
        if (Input.GetButtonDown(m_MissileButton) && MissileAvailiable)
        {
            MissileFire();
        }

        if (Input.GetButtonDown(m_MiniGunButton) && canShoot)
        {
            StartCoroutine(miniGun());
        }
        

    }

    IEnumerator miniGun()
    {
        while (Input.GetButton(m_MiniGunButton))
        {
            Rigidbody minishellInstance = Instantiate(m_MiniShell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            
            minishellInstance.velocity = m_MaxLaunchForce/2 * m_FireTransform.forward;

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();
            yield return new WaitForSeconds(0.1f);
        }

        canShoot = false;
        m_ShellAvailiable = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
        m_ShellAvailiable = true;
    }

    private void Fire()
    {
        
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;


        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;

        StartCoroutine(Reload());
        

    }

    private void MissileFire()
    {
        Rigidbody missileInstance = Instantiate(m_Missile, m_FireTransform.position, m_FireTransform.rotation);
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        missileInstance.GetComponent<Missile>().SetTarget(m_Target);
        StartCoroutine(Reload());
    }

    

    public void SetGameManager(Transform gameManager)
    {
        m_Target = gameManager;
    }

    private IEnumerator Reload()
    {
        MissileAvailiable = false;
        m_ShellAvailiable = false;
        canShoot = false;
        yield return new WaitForSecondsRealtime(0.5f);
        canShoot = true;
        m_ShellAvailiable = true;
        MissileAvailiable = true;
    }

    private void Bomb()
    {
        

        Rigidbody bombInstance = Instantiate(m_Bomb, m_BombTransform.position + new Vector3(0, 0.35f, 0), m_BombTransform.rotation) as Rigidbody;
        

        bombInstance.velocity = m_BombTransform.forward * -1;

        StartCoroutine(Cooldown());
        
    }

    IEnumerator Cooldown()
    {
        m_BombAvailiable = false;
        yield return new WaitForSecondsRealtime(2);
        m_BombAvailiable = true;
    }
}