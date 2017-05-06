using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ComboTextUpdater : MonoBehaviour {

    public Text textComponent;
    Queue<string> messageQueue = new Queue<string>();
    private string curComboString;
    bool displayingMessage;
    public float messageLifeTime = 3f;
    public string COMBO_appendText = "COMBO";
	// Use this for initialization
	void Start () {
        ScoreController.ComboUpdateEvent += UpdateText;
        MatchController.PendingMatchClearedEventType += ReactToEvent;
        textComponent.enabled = false;
	}
	
	// Update is called once per frame
	void UpdateText (int curCombo) {
        if (textComponent != null)
        {
            textComponent.enabled = curCombo > 0 ? true : false;
            if (curCombo > 0)
            {
                curComboString = curCombo.ToString() + " " + COMBO_appendText;
            }
            else
            {
                curComboString = "";
            }
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
        displayingMessage = true;
        if (textComponent.enabled == false)
        {
            textComponent.enabled = true;
        }
        string curText = messageQueue.Dequeue();
        textComponent.text = curText;
        // Play animation here maybe?
        textComponent.rectTransform.anchoredPosition3D = new Vector3(0, 60, 0); //TODO Hardcoded value, fetch the height of control instead
        textComponent.rectTransform.DOMoveY(0, 0.4f);
        yield return new WaitForSeconds(messageLifeTime);
        textComponent.rectTransform.DOMoveY(60, 0.4f);
        if (messageQueue.Count != 0)
        {
            StartCoroutine("DisplayMessageInQueue");
        }
        else
        {
            textComponent.text = curComboString;
            displayingMessage = false;
        }
    }
    void ReactToEvent(SpawnType eventType)
    {

    }
}
