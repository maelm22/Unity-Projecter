using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestHandlerCleaner : MonoBehaviour
{
    //The local address of the server - URI (Uniform Resource Identifier)
    private string URI = "http://localhost:3000/";

    public string playerName = "marco";
    public string playerPassword = "ocram";

    // Start is called before the first frame update
    private void Start()
    {
        ReadPlayer(playerName, playerPassword);
    }

    /// <summary>
    /// In this example I wanted to minimize the code from the RequestHandler script, so we don't have as much duplicated stuff
    /// One thing I added here, that we haven't seen before is the usage of this Action parameter: this is a way of passing a function as an input (in this case Action<string> means that it accepts a function that returns void, with 1 input of type string).
    /// This allows us to have only one definintion of the connection code, and lets us run some function at the end, since reading or writing to the database would likely require us to do something different.
    /// If you want to pass on a function that also returns something, check the Func class
    /// </summary>
    /// <param name="requestURI"></param>
    /// <param name="postProcessResult"></param>
    /// <returns></returns>
    private IEnumerator SendRequest(string requestURI, Action<string> postProcessResult)
    {
        //Sends a Web Request - HTTP Get "/createPlayer"
        UnityWebRequest request = UnityWebRequest.Get(requestURI);

        //Handles the send Request
        var handler = request.SendWebRequest();

        //Tracks the responseTime
        float responseTime = 0.0f;

        //While awaiting a response
        while (!handler.isDone)
        {
            //Adds the time since the last frame
            responseTime += Time.deltaTime;

            //Cancels the request if the responseTime exceeds 10 seconds
            if (responseTime > 10.0f)
            {
                Debug.Log("Cancel request - ResponseTime exceeds 10 seconds");
                break;
            }

            //Wait for the next frame and continue execution from this line
            yield return null;
        }

        //If the Request succesfully received a Response
        if (request.result == UnityWebRequest.Result.Success)
        {
            //here we call our post-process function that we passed as a parameter
            postProcessResult(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error - No response received");
        }
    }

    //------------Methods for the various interactions we want with the DB----//

    //CREATE player
    public void CreatePlayer(string name, string password)
    {
        //Sends a Web Request - HTTP Get "/createPlayer"
        string requestURL = $"{URI}createPlayer?name={name}&password={password}";

        StartCoroutine(SendRequest(requestURL, PrintInLog));
    }

    //READ player
    public void ReadPlayer(string name, string password)
    {
        //Sends a Web Request - HTTP Get "/readPlayer"
        string requestURL = $"{URI}readPlayer?name={name}&password={password}";

        StartCoroutine(SendRequest(requestURL, PrintInLog));
    }

    //UPDATE player score
    public void UpdatePlayer(string name, string password, int score)
    {
        //Sends a Web Request - HTTP Get "/updatePlayer"
        string requestURL = $"{URI}updatePlayer?name={name}&password={password}&score={score}";

        StartCoroutine(SendRequest(requestURL, PrintInLog));
    }

    //-------------------Methods for doing post processing of the data-----//
    private void PrintInLog(string text)
    {
        Debug.Log("Received response: ");
        Debug.Log(text);
    }
}
