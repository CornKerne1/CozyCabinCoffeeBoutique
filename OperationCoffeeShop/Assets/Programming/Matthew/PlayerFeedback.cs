using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerFeedback : MonoBehaviour {
    [SerializeField] private TMPro.TextMeshProUGUI txtData;
    [SerializeField] private UnityEngine.UI.Button btnSubmit;
    [SerializeField] private CollectionOption option;
 
    private enum CollectionOption { openEmailClient, openGFormLink, sendGFormData };
    private static bool _dontSpam;
    private const string kReceiverEmailAddress = "me@gmail.com";
 
    private const string kGFormBaseURL = "https://docs.google.com/forms/d/e/1FAIpQLSdvIjg0oxptd_6jGN6YRnrqnjY1PLjnU5_aJVhJtsXLuYMVAA/";
    private const string kGFormEntryID = "entry.772093831";
 
    void Start() {
        UnityEngine.Assertions.Assert.IsNotNull( txtData );
        UnityEngine.Assertions.Assert.IsNotNull( btnSubmit );
        btnSubmit.onClick.AddListener( delegate {
            switch ( option ) {
                case CollectionOption.openEmailClient:
                    OpenEmailClient( txtData.text );
                    break;
                case CollectionOption.openGFormLink:
                    OpenGFormLink();
                    break;
                case CollectionOption.sendGFormData:
                    var trimmedTxt = txtData.text.Trim();
                    if (_dontSpam||txtData.text.Length<2||trimmedTxt == ""||string.IsNullOrEmpty(trimmedTxt)) return;
                    string logFilePath = Path.Combine(Application.persistentDataPath, "Player.log");
                    string playerLogs = File.ReadAllText(logFilePath);
                    var txt = txtData.text+ "\n\n" + "Player Logs:\n" + playerLogs;
                    StartCoroutine( SendGFormData( txt) );
                    break;
            }
        } );
    }

    private void Update()
    {
        Debug.Log(txtData.text.Length+txtData.text);
    }

    private static void OpenEmailClient( string feedback ) {
        string email = kReceiverEmailAddress;
        string subject = "Feedback";
        string body = "<" + feedback + ">";
        OpenLink( "mailto:" + email + "?subject=" + subject + "&body=" + body );
    }
 
    private static void OpenGFormLink() {
        string urlGFormView = kGFormBaseURL + "viewform";
        OpenLink( urlGFormView );
    }
 
    private static IEnumerator SendGFormData<T>( T dataContainer )
    {
        _dontSpam = true;
        bool isString = dataContainer is string;
        string jsonData = isString ? dataContainer.ToString() : JsonUtility.ToJson(dataContainer);
 
        WWWForm form = new WWWForm();
        form.AddField( kGFormEntryID, jsonData );
        string urlGFormResponse = kGFormBaseURL + "formResponse";
        using ( UnityWebRequest www = UnityWebRequest.Post( urlGFormResponse, form ) ) {
            yield return www.SendWebRequest();
        }
        yield return new WaitForSeconds(10);
        _dontSpam = false;
    }

    public void CloseButton()
    {
        SceneManager.LoadScene(1);
    }
 
    // We cannot have spaces in links for iOS
    public static void OpenLink( string link ) {
        bool googleSearch = link.Contains( "google.com/search" );
        string linkNoSpaces = link.Replace( " ", googleSearch ? "+" : "%20");
        Application.OpenURL( linkNoSpaces );
    }
}