using UnityEngine;
using System.Collections;

public class CaveManager : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;

    Mesh mesh;
    MeshData initialMeshData;

    [Header("Mesh Properties")]
    public Material material;
    public int radius, depth, zOffset;
    public float radiusStepSize, depthStepSize;
    int width;

    [Header("Noise Properties")]
    public float scale;
    public float persistance, lacunarity;
    public int octaves;
    public float speed;
    public float noiseIntensity, waveIntensity;
    float[] heightMap;
    Vector2[] waveform;

    [Header("Compute Shader")]
    public ComputeShader csFile;
    ComputeBuffer vertices;
    ComputeBuffer normals;
    ComputeBuffer displacement;
    ComputeBuffer points;
    ComputeBuffer wavelength;
    Vector3[] computedVertices;
    int length;

    Vector2 dir;
    Vector2 offset;

    GameManager manager;
    int seed;

    [Header("Shader Support")]
    [SerializeField]
    public bool useShader;

    public GameObject[] chunks;
    CaveChunkHandler[] chunkHandlers;

    PlayerMovement player;

    void Awake()
    {
        seed = (int)System.DateTime.Now.Ticks;
        width = (int)(Mathf.PI * 2 / radiusStepSize) + 1;
        Random.InitState(seed);
        dir = Random.insideUnitCircle;
        waveform = Noise.InitialDirectionalNoise(depth, dir);
        heightMap = Noise.GenerateNoiseMap(width, depth, seed, scale, octaves, persistance, lacunarity, offset);

        useShader = SystemInfo.supportsComputeShaders;
        if (useShader)
        {
            foreach (GameObject o in chunks)
                o.SetActive(false);

            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();

            initialMeshData = GenerateCave.GenerateCircularMesh(radius, depth, radiusStepSize, depthStepSize, zOffset);
            mesh = initialMeshData.CreateMesh();
            computedVertices = initialMeshData.vertices;
            length = initialMeshData.vertexCount;
            offset = Vector2.zero - Vector2.up * zOffset;
            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshRenderer.sharedMaterial = material;


            vertices = new ComputeBuffer(length, 12);
            normals = new ComputeBuffer(length, 12);
            points = new ComputeBuffer(length, 12);
            displacement = new ComputeBuffer(length, 4);
            wavelength = new ComputeBuffer(depth, 8);

            csFile.SetBuffer(0, "InputPos", vertices);
            csFile.SetBuffer(0, "Normals", normals);
            csFile.SetBuffer(0, "OutputPos", points);
            csFile.SetBuffer(0, "HeightMap", displacement);
            csFile.SetBuffer(0, "WaveLength", wavelength);
            csFile.SetInt("length", length);
            csFile.SetInt("width", width);
            vertices.SetData(initialMeshData.vertices);
            normals.SetData(initialMeshData.normals);
        }
        else
        {
            chunkHandlers = new CaveChunkHandler[chunks.Length];
            for (int i = 0; i < chunks.Length; i++)
            {
                chunkHandlers[i] = chunks[i].GetComponent<CaveChunkHandler>();
                chunkHandlers[i].SetMeshAttributes(radius, depth / chunks.Length, i, width, radiusStepSize, depthStepSize, material);
                chunkHandlers[i].SetNoiseAttributes(noiseIntensity, waveIntensity);
            }
        }
    }

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerMovement>();
        player.movementIncreasedEvents += AdjustSpeed;
        AdjustSpeed();
        StartCoroutine(UpdateDirectionalNoise(2));
        StartCoroutine(UpdateWavelength(0.05f));
    }

    void Update()
    {
        offset -= Vector2.up * speed * Time.deltaTime;
        heightMap = Noise.GenerateNoiseMap(width, depth, seed, scale, octaves, persistance, lacunarity, offset);


        if (useShader)
        {
            csFile.SetFloat("noiseIntensity", noiseIntensity);
            csFile.SetFloat("waveIntensity", waveIntensity);
            displacement.SetData(heightMap);
            wavelength.SetData(waveform);
            csFile.Dispatch(0, length, 1, 1);
            points.GetData(computedVertices);
            mesh.vertices = computedVertices;
            mesh.RecalculateNormals();
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }
        else
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                chunkHandlers[i].SetNoiseAttributes(noiseIntensity, waveIntensity);
            }
        }
    }

    void OnDisable()
    {
        if (useShader)
        {
            vertices.Release();
            normals.Release();
            displacement.Release();
            points.Release();
            wavelength.Release();
        }
    }

    public void StopRoutine()
    {
        if (useShader)
            StopAllCoroutines();
        else
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                chunkHandlers[i].StopAllCoroutines();
            }

        }
    }

    IEnumerator UpdateDirectionalNoise(float seconds)
    {
        while (true)
        {
            dir = Random.insideUnitCircle;
            yield return new WaitForSeconds(seconds);
        }
    }

    IEnumerator UpdateWavelength(float seconds)
    {
        while (true)
        {
            if (manager.currentState == GameManager.GameState.INGAME)
                Noise.UpdateDirectionalNoise(ref waveform, dir);
            yield return new WaitForSeconds(seconds);
        }
    }

    public void GetSubNoise(ref float[] dest, int offset)
    {
        System.Array.Copy(heightMap, offset, dest, 0, dest.Length);
    }

    public void GetSubWave(ref Vector2[] dest, int offset)
    {
        System.Array.Copy(waveform, offset, dest, 0, dest.Length);
    }

    public Vector2 GetWaveOffset()
    {
        return waveform[waveform.Length/2] * waveIntensity;
    }

    public void AdjustSpeed()
    {
        speed = player.movementSpeed / 4;
    }
}
