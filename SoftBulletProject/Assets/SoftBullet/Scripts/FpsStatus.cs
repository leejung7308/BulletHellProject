using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsStatus : MonoBehaviour
{
    private float lfps = 10000;
    private float accumulatedDeltaTime = 0.0f;
    private float totalAccumulatedDeltaTime = 0.0f;
    private int frameCount = 0;
    private int totalFrameCount = 0;
    private float averageFps = 0.0f;
    private float totalAverageFps = 0.0f;
    private float updateInterval = 0.5f;
    private float timeSinceLastUpdate = 0.0f;

    void Update()
    {
        // 누적 시간과 프레임 수를 업데이트
        float deltaTime = Time.unscaledDeltaTime;
        accumulatedDeltaTime += Time.unscaledDeltaTime;
        totalAccumulatedDeltaTime += deltaTime;
        frameCount++;
        totalFrameCount++;

        timeSinceLastUpdate += deltaTime;

        // 0.5초마다 평균 FPS 계산
        if (timeSinceLastUpdate > updateInterval)
        {
            averageFps = frameCount / accumulatedDeltaTime;
            totalAverageFps = totalFrameCount / totalAccumulatedDeltaTime;
            if (lfps > averageFps) lfps = averageFps;
            timeSinceLastUpdate = 0;
            accumulatedDeltaTime = 0.0f;
            frameCount = 0;
        }
    }


    public int fontSize = 30;
    public Color Color = new Color(0, 0, 0, 1f);
    public float width, heigth;

    
    private void OnGUI()
    {
        Rect pos = new Rect(width, heigth, Screen.width, Screen.height);

        string text = string.Format("{0:N1} FPS ({1:N1}minFPS) ({2:N1} avgFPS)", averageFps,  lfps, totalAverageFps);

        GUIStyle style = new GUIStyle();

        style.fontSize = fontSize;
        style.normal.textColor = Color;

        GUI.Label(pos, text, style);
    }
    
}
