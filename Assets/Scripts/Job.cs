using UnityEngine;

public class Job : ThreadJob
{
    public Vector3[] InputPos;
    public Vector3[] Normals;
    public float[] HeightMap;
    public Vector2[] WaveLength;
    
    public Vector3[] OutputPos;

    public float noiseIntensity, waveIntensity;
    public int width;

    protected override void ThreadFunction()
    {
        for (int i = 0; i < InputPos.Length; i++) {
            OutputPos[i] = InputPos[i] + (1 - HeightMap[i]) * Normals[i] * noiseIntensity;

            int k = (i / width);
            OutputPos[i].x += WaveLength[k].x * waveIntensity;
            OutputPos[i].y += WaveLength[k].y * waveIntensity;
        }
    }

}
