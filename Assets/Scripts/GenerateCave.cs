using UnityEngine;

public static class GenerateCave
{

    public const float Pi_2 = Mathf.PI * 2;

    public static MeshData GenerateCircularMesh(float radius, int depth, float radiusStepSize, float depthStepSize, int zOffset)
    {
        float width = Pi_2 / radiusStepSize;
        MeshData meshData = new MeshData((int)width + 1, depth);
        int vertexIndex = 0;
        int startOfCircle = 0;

        for (int z = 0; z < depth; z++)
        {
            startOfCircle = vertexIndex;
            for (float theta = 0f; theta < Pi_2; theta += radiusStepSize)
            {

                meshData.vertices[vertexIndex] = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), zOffset + (z * depthStepSize));
                meshData.normals[vertexIndex] = (new Vector3(0f, 0f, meshData.vertices[vertexIndex].z) - meshData.vertices[vertexIndex]).normalized;
                meshData.uvs[vertexIndex] = new Vector2(theta / Pi_2, z / (float)depth);

                if (z < depth - 1)
                {
                    if (theta < Pi_2 - radiusStepSize)
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + (int)width + 1, vertexIndex + (int)width + 2);
                        meshData.AddTriangle(vertexIndex + (int)width + 2, vertexIndex + 1, vertexIndex);
                    }
                    else
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + (int)width + 1, startOfCircle + (int)width + 1);
                        meshData.AddTriangle(startOfCircle + (int)width + 1, startOfCircle, vertexIndex);
                    }
                }
                vertexIndex++;
            }
        }
        return meshData;
    }

}

public class MeshData
{
    public Vector3[] vertices;
    public Vector3[] normals;
    public int[] triangles;
    public Vector2[] uvs;

    public int triangleIndex, vertexCount;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertexCount = meshWidth * meshHeight;
        vertices = new Vector3[vertexCount];
        normals = new Vector3[vertexCount];
        uvs = new Vector2[vertexCount];
        triangles = new int[(meshWidth) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        return mesh;
    }

}