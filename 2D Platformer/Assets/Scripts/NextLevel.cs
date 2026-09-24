using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;
    public Text winText;
    
    public PLayerController PLayerController;

    // Start is called before the first frame update
    void Start()
    {
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PLayerController.coinsTotal == PLayerController.coinsCollected)
        {
            winText.text = "Congratulations on to next Level!";
        
            victory();
        }
        
    }

    private IEnumerator victory()
    {
        
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(nextLevel);
        
    }

}
