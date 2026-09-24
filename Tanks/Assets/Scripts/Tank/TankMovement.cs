using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    private float m_Speed = 12f;
    public float m_NormalSpeed = 12f;
    public float m_SpeedBoost = 20f;
    public float m_TurnSpeed = 180f;       
    private bool canBoost = true;

    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;

    private string m_BoostButton;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_BoostButton = "Boost" + m_PlayerNumber;

        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

        EngineAudio();
        
        SpeedBoost();
        
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }


    }


    private void FixedUpdate()
    {
        // Move and turn the tank.

        //Physics.Raycast(transform.position, transform.forward)
       
        Move();
        Turn();       


    }
    

    private void Move()
    {
        Vector3 movement = transform.forward * (m_MovementInputValue * m_Speed * Time.deltaTime);
        
        Vector3 halfSize = new Vector3(0.75f, 0.5f, 0.8f);

        if (!Physics.BoxCast(transform.position, halfSize, transform.forward * m_MovementInputValue, Quaternion.identity, 0.5f))
        {        
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }
            
        
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.

        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }

    private void SpeedBoost()
    {
        if (canBoost && Input.GetButtonDown (m_BoostButton))
        {
            StartCoroutine(Boost());
        }
    }

    IEnumerator Boost()
    {
        canBoost = false;
        m_Speed = m_SpeedBoost;
        yield return new WaitForSecondsRealtime(3);
        m_Speed = m_NormalSpeed;
        
        yield return new WaitForSecondsRealtime(4);
        canBoost = true;
    }

   
}