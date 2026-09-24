using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRepairPack : MonoBehaviour
{

    public float restoredHealth;
    
    public Transform spawnLocation;
    public GameObject repairPack;
    
    public Slider cooldownSlider;
    private bool m_WrenchAvailable;
    public float coolDownTime;
    private float m_RespawnTicks;

    private GameObject m_Wrench;

    private void Start()
    {
        m_WrenchAvailable = true;
        SpawnWrench();

        m_RespawnTicks = coolDownTime / 100f;
    }

    public void SpawnWrench()
    {
        m_Wrench = Instantiate(repairPack, spawnLocation.position,quaternion.identity);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && m_WrenchAvailable)
        {
            other.GetComponent<TankHealth>().RegainHealth(restoredHealth);
            StartCoroutine(Cooldown());
        }
    }
    

    IEnumerator Cooldown()
    {
        cooldownSlider.value = 0;
        m_WrenchAvailable = false;
        m_Wrench.SetActive(false);
        while (cooldownSlider.value < 100)
        {
            cooldownSlider.value++;
            yield return new WaitForSecondsRealtime(m_RespawnTicks);
        }

        m_Wrench.SetActive(true);
        m_WrenchAvailable = true;

    }
}
