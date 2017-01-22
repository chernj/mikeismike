using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBeats : MonoBehaviour, JumpNumbers.AudioCallbacks {

    // Use this for initialization
    private AudioSource audioSource;
    public float updateStep = 0.1f;
    private float beatStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;
    private float currentBeatTime = 0f;

    private float beatShowTime = 0f;

    public float clipLoudness;
    public float bpm = 120;
    private float beatMaxTime;
    public bool beat;
    private float[] clipSampleData;
    private float count;

    void Start () {
        JumpNumbers processor = FindObjectOfType<JumpNumbers>();
        processor.addAudioCallback(this);
        audioSource = GetComponent<AudioSource>();
        clipSampleData = new float[sampleDataLength];
        beatMaxTime = 1 / (bpm / 60);
        count = 0;
    }
	
	// Update is called once per frame
	void Update () {
        float timePlus = Time.deltaTime;
        count += timePlus;
        currentUpdateTime += timePlus;
        currentBeatTime += timePlus;
        if (beat == true)
        {
            beatShowTime += timePlus;
        }
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
        }
        if (currentBeatTime >= beatMaxTime)
        {
            print("beat time");
            print(count);
            currentBeatTime = currentBeatTime - beatMaxTime;
            beat = true;
        }
        if (beatShowTime >= beatStep)
        {
            print("jojo turns the beat off");
            beat = false;
            beatShowTime = 0f;
        }
    }
    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    public void onOnbeatDetected()
    {
        //beat = true;
        //print("Beat!!!");
    }

    //This event will be called every frame while music is playing
    public void onSpectrum(float[] spectrum)
    {
        //The spectrum is logarithmically averaged
        //to 12 bands
        /*
        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
        */
    }
}
