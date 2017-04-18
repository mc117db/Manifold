using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnomalyIndicatorController : MonoBehaviour {

	public bool active;
    public TextMesh textMeshComponent;
    public TextMesh textComponent;
	public ReferencePointBehaviour target;
    public float lerpSpeed = 20f;
    public Vector3 inActivePos = new Vector3(0.5f, 6f, 5f);
    public SpriteRenderer firstColor, secondColor, thirdColor;
    public Color inactiveIndicatorColor;
    public Color anomalyWarningStateColor;
    public Color anomalyWarningStateColorTEXT;
    public Color anomalyFufilledStateColor;
    public Color anomalyFufilledStateColorTEXT;
    private Vector3 initScale;
	
	// Use this for initialization
	void Start () {
        GameController.TargetReferencePointToSpawnWhenStagingSetUpdateChange += ChangeTargetPoint;
        GameController.RingDataToSpawnWhenStagingSetUpdateChange += ChangeIndicatorColors;
        GameController.LoseEvent += delegate { active = false; };
        initScale = transform.localScale;
        if (textMeshComponent == null)
        {
            textMeshComponent = GetComponent<TextMesh>();
        }
	}
	
    void ChangeTargetPoint(ReferencePointBehaviour point)
    {
        textMeshComponent.color = anomalyWarningStateColor;
        textComponent.text = "ANOMALY";
        textComponent.color = anomalyWarningStateColorTEXT;
        if (point == null)
        {
            if (target)
            {
                target.GetComponent<RingPointManager>().stateChange -= AnomalyFufilled;
                target = null;
            }
        }
        else if (point != target && target != null)
        {
            //Deregister from previous target
            target.GetComponent<RingPointManager>().stateChange -= AnomalyFufilled;
            target = point;
            target.GetComponent<RingPointManager>().stateChange += AnomalyFufilled;
        }
        else if (target == null)
        {
            target = point;
            target.GetComponent<RingPointManager>().stateChange += AnomalyFufilled;
        }
        
        if (target)
        {

            active = true;
        }
        else
        {
            active = false;
        }
    }
    void AnomalyFufilled()
    {
        textComponent.text = "CLEAR";
        textComponent.color = anomalyFufilledStateColorTEXT;
        transform.DOPunchScale(Vector3.right * 0.2f, 0.2f).OnComplete(delegate { transform.localScale = initScale; });
        textMeshComponent.color = anomalyFufilledStateColor;
    }
    void ChangeIndicatorColors(RingData data)
    {
        if (data.Outer)
        {
            firstColor.color = ColorManager.instance.FetchColorInformation(data.ringColors[0]);
        }
        else
        {
            firstColor.color = inactiveIndicatorColor;
        }
        if (data.Middle)
        {
            secondColor.color = ColorManager.instance.FetchColorInformation(data.ringColors[1]);
        }
        else
        {
            secondColor.color = inactiveIndicatorColor;
        }
        if (data.Inner)
        {
            thirdColor.color = ColorManager.instance.FetchColorInformation(data.ringColors[2]);
        }
        else
        {
            thirdColor.color = inactiveIndicatorColor;
        }
    } 
	// Update is called once per frame
	void Update () {
		if (active)
        {
            if (target)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y,transform.position.z), lerpSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, inActivePos, lerpSpeed * Time.deltaTime);
        }
	}
}
