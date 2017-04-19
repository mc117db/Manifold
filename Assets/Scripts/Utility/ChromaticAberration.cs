using UnityEngine;
using System.Collections;
using DG.Tweening;
[ExecuteInEditMode]
public class ChromaticAberration : MonoBehaviour {
	private Shader shader;
	private Material material;

	float chromaticAberration = 1.0f;
    public bool onTheScreenEdges = true;
    public bool shakeOnStagingAreaEvent = true;
    public bool shiftOnDragEvent = true;
    Vector3 shakeVector = new Vector3();
    public float shakeDuration = 0.2f;
    public float shakeStrength = 10f;
    public int shakeVibrationAmount = 10;
    public float shakeRandomness = 20f;
    public void Start()
    {
		shader = new Shader();
		shader = Shader.Find("Hidden/ChromaticAberration");
		material = new Material(shader);

		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}

		if (!shader && !shader.isSupported) {
			enabled = false;
		}
        //Subscribe to RingFactory
        RingFactory.onRefreshSetEvent += ShakeScreen;
        DragOrbitTouchScript.deltaChange += DragEffect;
	}
    public void ShakeScreen()
    {
        DOTween.Shake(() => shakeVector, x => shakeVector = x, 
            shakeDuration, 
            shakeStrength,
            shakeVibrationAmount,
            shakeRandomness, false);
    }
    public void DragEffect(Vector2 delta)
    {
        if (shiftOnDragEvent)
        shakeVector = new Vector2(delta.x/100, delta.y/100);
    }

    public void Update()
    {
        chromaticAberration = shakeVector.magnitude;
        if (shakeVector.magnitude != 0)
        {
            shakeVector = Vector3.MoveTowards(shakeVector, Vector3.zero, 50f * Time.deltaTime);
        }
    }

    public void OnRenderImage(RenderTexture inTexture, RenderTexture outTexture) {
		if (shader != null) {
			material.SetFloat("_ChromaticAberration", 0.01f * chromaticAberration);

            if (onTheScreenEdges)
				material.SetFloat("_Center", 0.5f);

            else
				material.SetFloat("_Center", 0);

            Graphics.Blit(inTexture, outTexture, material);
		}
		else {
			Graphics.Blit (inTexture, outTexture);
		}
	}
}