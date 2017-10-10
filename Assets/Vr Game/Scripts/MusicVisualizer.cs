using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVisualizer : MonoBehaviour {

    AudioSource audioSource;
    LineRenderer lineRenderer;

    public bool musicSelected;

    float[] samples = new float[128];

	void Start () {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = samples.Length;
	}
	
	void Update () {
        //audioSource.GetSpectrumData(samples, 0, FFTWindow.Rectangular);
        audioSource.GetOutputData(samples, 0);

        for (int i = 0; i < samples.Length; i++) {
            lineRenderer.SetPosition(i, new Vector3(-0.1f + (i * (0.2f / samples.Length)), Mathf.Clamp(samples[i] / (musicSelected ? 4.0f : 1.0f), -0.1f, 0.1f), 0));
        }
    }
}
