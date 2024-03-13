using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class OpenAI : MonoBehaviour
{
    private string apiKey = "";
    private string apiURL = "https://api.openai.com/v1/chat/completions";
    private string lastResponseContent; 

    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class ResponseData
    {
        public List<Choice> choices;
    }

    [Serializable]
    public class Choice
    {
        public Message message;
    }

    [Serializable]
    public class RequestData
    {
        public string model = "gpt-3.5-turbo";
        public List<Message> messages;
        public double temperature = 0.5;
        public int max_tokens = 3000;
    }

    void MakePoem(string name)
    {
        StartCoroutine(SendRequestToChatGPT(name));
    }
    void Start()
    {
        StartCoroutine(SendRequestToChatGPT(name));
    }

    IEnumerator SendRequestToChatGPT(string name)
    {
        Message userMessage = new Message
        {
            role = "user",
            content = $"������ ���� ��� ������ , ������� ��������� �����. �� ��� ��� ������. � �� ������ ������. � ����� �������"
        };

        RequestData requestData = new RequestData
        {
            messages = new List<Message> { userMessage }
        };

        string json = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(apiURL, "POST")
        {
            uploadHandler = (UploadHandler)new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)),
            downloadHandler = (DownloadHandler)new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        }
        else
        {
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
            if (responseData.choices != null && responseData.choices.Count > 0)
            {
                lastResponseContent = responseData.choices[0].message.content;
                Debug.Log(lastResponseContent); 
            }
        }
    }
}