using UnityEngine;
using System.Collections;

public class CaveChunkHandler : MonoBehaviour
{

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    Mesh mesh;
    MeshData initialMeshData;

    Material material;
    int radius, depth, zOffset;
    float radiusStepSize, depthStepSize;
    int width;

    float noiseIntensity, waveIntensity;
    Vector2[] waveform;
    float[] heightMap;

    Job threadJob;
    CaveManager caveManager;

    int numberOfChild;

    void Start()
    {
        caveManager = FindObjectOfType<CaveManager>();

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        initialMeshData = GenerateCave.GenerateCircularMesh(radius, depth, radiusStepSize, depthStepSize, zOffset);
        mesh = initialMeshData.CreateMesh();
        
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        meshRenderer.sharedMaterial = material;

        heightMap = new float[width * depth];
        waveform = new Vector2[depth];
        caveManager.GetSubNoise(ref heightMap, (heightMap.Length-width) * numberOfChild);
        caveManager.GetSubWave(ref waveform, (waveform.Length-1) * numberOfChild);

        threadJob = new Job();
        threadJob.KeepAwake = true;

        threadJob.InputPos = initialMeshData.vertices;
        threadJob.Normals = initialMeshData.normals;

        threadJob.HeightMap = heightMap;
        threadJob.WaveLength = waveform;
        threadJob.OutputPos = new Vector3[initialMeshData.vertexCount];
        threadJob.width = width;

        StartCoroutine(UpdateChuck());
    }

    IEnumerator UpdateChuck()
    {
        threadJob.Start();
        while (true)
        {
            yield return StartCoroutine(threadJob.WaitFor());
            
            mesh.vertices = threadJob.OutputPos;
            mesh.RecalculateNormals();
            meshCollider.sharedMesh = meshFilter.sharedMesh;

            caveManager.GetSubNoise(ref heightMap, (heightMap.Length - width) * numberOfChild);
            caveManager.GetSubWave(ref waveform, (waveform.Length - 1) * numberOfChild);

            threadJob.noiseIntensity = noiseIntensity;
            threadJob.waveIntensity = waveIntensity;
   
            threadJob.Redo();
        }
    }

    public void ExitRoutine()
    {
        StopAllCoroutines();
        threadJob.KeepAwake = false;
    }

    public void SetMeshAttributes(int Iradius, int Idepth, int InumberOfChild, int Iwidth, float IradiusStepSize, float IdepthStepSize, Material Imaterial)
    {
        radius = Iradius;
        depth = Idepth;
        numberOfChild = InumberOfChild;
        zOffset = (int)((depth-1) * InumberOfChild * IdepthStepSize);
        width = Iwidth;
        radiusStepSize = IradiusStepSize;
        depthStepSize = IdepthStepSize;
        material = Imaterial;
    }

    public void SetNoiseAttributes(float InoiseIntensity, float IwaveIntensity)
    {
        noiseIntensity = InoiseIntensity;
        waveIntensity = IwaveIntensity;
    }

}
