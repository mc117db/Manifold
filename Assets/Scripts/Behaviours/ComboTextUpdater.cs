using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ComboTextUpdater : MonoBehaviour {

    public Text textComponent;
    Queue<string> messageQueue = new Queue<string>();
    public string curComboString;
    bool displayingMessage = false;
    public float messageLifeTime = 3f;
    public string COMBO_appendText = "COMBO";
	// Use this for initialization
	void Start () {
        ScoreController.ComboUpdateEvent += UpdateText;
        MatchController.PendingMatchClearedEventType += ReactToEvent;
        //textComponent.enabled = false;
//            AddMessageIntoQueue("TEST 1");
//            AddMessageIntoQueue("TEST 2");
//            AddMessageIntoQueue("TEST 3");
//            AddMessageIntoQueue("TEST 4");
//            AddMessageIntoQueue("TEST 5");
	}
	
	// Update is called once per frame
	void UpdateText (int curCombo) {
        if (textComponent != null)
        {
            //textComponent.enabled = curCombo > 0 ? true : false;
            if (curCombo > 1)
            {
                curComboString = curCombo.ToString() + " " + COMBO_appendText;
            }
            else
            {
                curComboString = "";
            }
        }
        if (!displayingMessage)
        {
            textComponent.text = curComboString;
        }
	}
    void AddMessageIntoQueue(string message)
    {
        messageQueue.Enqueue(message);
        if (!displayingMessage)
        {
            StartCoroutine("DisplayMessageInQueue");
        }
    }
    IEnumerator DisplayMessageInQueue()
    {
        Debug.Log("Displaying Message");
        displayingMessage = true;
        if (textComponent.enabled == false)
        {
            textComponent.enabled = true;
        }
        string curText = messageQueue.Dequeue();
        textComponent.text = curText;
        // Play animation here maybe?
        textComponent.rectTransform.anchoredPosition3D = new Vector3(0, 60, 0); //TODO Hardcoded value, fetch the height of control instead
        textComponent.rectTransform.DOAnchorPos3D(Vector3.zero,0.5f);
        yield return new WaitForSeconds(messageLifeTime);
        textComponent.rectTransform.DOAnchorPos3D(new Vector3(0, 60, 0),0.5f);
        yield return new WaitForSeconds(0.6f);
        if (messageQueue.Count > 0)
        {
            StartCoroutine("DisplayMessageInQueue");
        }
        else
        {
            textComponent.text = curComboString;
            displayingMessage = false;
            yield return null;
        }
    }
    void ReactToEvent(SpawnType eventType)
    {
        switch (eventType)
        {
            case SpawnType.Anomaly:
                AddMessageIntoQueue("ANOMALY MATCH");
                break;
            case SpawnType.ForceDrop:
                AddMessageIntoQueue("FORCEDROP MATCH");
                break; 
        }
    }
}
