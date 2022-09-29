using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{

    [SerializeField]
    private int MapSize;

    [SerializeField]
    private float Scale;

    [SerializeField]
    private int HeightScale;

    private MeshFilter meshFilter;      
    private void Awake()
    {
        meshFilter = transform.GetComponent<MeshFilter>();   
    }

    private Mesh CreateQuad(Vector3 StartPoint, Vector3 Point0, Vector3 Point1) {

        var normal = Vector3.Cross(Point0, Point1).normalized;

        var mesh = new Mesh()
        {

            vertices = new[] { StartPoint, StartPoint + Point0, StartPoint + Point1, StartPoint + Point0 + Point1 },

            normals = new[] { normal, normal, normal, normal },

            triangles = new[] { 0, 1, 2, 1, 3, 2 }

        };

        return mesh;
    }

    private Mesh CreateCube(Vector3 StartPoint) {

        Vector3 Point0 = new Vector3(1, 0, 0);

        Vector3 Point1 = new Vector3(0, 1, 0);

        Vector3 Point2 = new Vector3(0, 0, 1);

        Vector3 EndPoint = StartPoint + new Vector3(1, 1, 1);

        var combine = new CombineInstance[6];

        combine[0].mesh = CreateQuad(StartPoint, Point0, Point2);
        combine[1].mesh = CreateQuad(StartPoint, Point1, Point0);
        combine[2].mesh = CreateQuad(StartPoint, Point2, Point1);
        combine[3].mesh = CreateQuad(EndPoint, -Point1, -Point2);
        combine[4].mesh = CreateQuad(EndPoint, -Point2, -Point0);
        combine[5].mesh = CreateQuad(EndPoint, -Point0, -Point1);

        var mesh = new Mesh();

        mesh.CombineMeshes(combine, true, false);

        return mesh;
    }

    private Mesh CreateMap(int size, float scale, int heightScale) {
       
        var mesh = new Mesh();

        var combine = new CombineInstance[size*size];

        var z = 0;

        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++)
            {

                var height = Mathf.Round(heightScale * Mathf.PerlinNoise((x + Time.time) * scale, (y + Time.time) * scale));

                combine[z].mesh = CreateCube(new Vector3(x, height, y));

                z++;
            }
        }

        mesh.CombineMeshes(combine, true, false);

        return mesh;
    }

    public void ShowMesh() {

        meshFilter.mesh = CreateMap(MapSize, Scale, HeightScale);

    }

    public void asc() {
        var mesh = new Mesh();

        Vector3[] verteces;

        verteces = mesh.vertices;
    }
}
