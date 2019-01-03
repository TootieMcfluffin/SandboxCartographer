using Delaunay;
using Delaunay.Geo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriangleNet;
using TriangleNet.Geometry;
using System.Linq;
using System;
using UnityEngine.UI;

public class TerrainGenerator : MonoBehaviour
{

    public int pointCount = 300;
    public int mapWidth = 500;
    public int mapHeight = 500;
    public int trianglesInChunk = 20000;
    public float elevScale = 100.0f;
    public float sampSize = 1f;
    public float frequencyBase = 2;
    //public int seed = 0;
    public Transform chunkPrefab = null;
    public Transform waterPrefab = null;
    public int octaves = 8;
    public float persistence = 1.1f;
    public GameObject tree = null;
    //public int treeSites = 5;
    //public int minTreesPerSite = 500;
    //public int maxTreesPerSite = 1000;
    //public float treePlacementRadius = 100;
    //public float treeVariance = .05f;


    private float waterLevel = 100f;
    public float WaterLevel
    {
        get { return waterLevel; }
        set { waterLevel = value; }
    }

    public List<GameObject> TreeList { get; set; }
    public List<GameObject> Houses { get; set; }
    public float MaxHeight { get; set; }
    public float MinHeight { get; set; }

    private List<Vector2> points;
    private Voronoi v;
    private TriangleNet.Mesh mesh;
    private List<float> elev = new List<float>();
    private List<Vector3> meshPoints = new List<Vector3>();

    // Use this for initialization
    void Start()
    {
        GameObject temp = GameObject.Find("NamePanelCollapse");
        temp.GetComponent<Collapse>().CollapseRight();
        DataHolder.PlaceableName = "Fir";
        DataHolder.TreeVariance = 0.3f;
        DataHolder.ShowNames = true;
        Houses = new List<GameObject>();
        TreeList = new List<GameObject>();
        pointCount = DataHolder.PointCount;
        mapWidth = DataHolder.MapWidth;
        mapHeight = DataHolder.MapHeight;
        elevScale = DataHolder.ElevationScale;
        sampSize = DataHolder.SampleSize;
        frequencyBase = DataHolder.FrequencyBase;
        octaves = DataHolder.Octaves;
        Run();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    List<GameObject> children = new List<GameObject>();
        //    foreach (Transform child in transform) children.Add(child.gameObject);
        //    children.ForEach(child => Destroy(child));
        //    Run();
        //}
        GameObject water = GameObject.Find("Water(Clone)");
        if (water.transform.localPosition.y != WaterLevel - ((MaxHeight + MinHeight) / 2))
        {
            water.transform.localPosition = new Vector3(water.transform.localPosition.x, WaterLevel - ((MaxHeight + MinHeight) / 2), water.transform.localPosition.z);
            foreach (GameObject t in TreeList)
            {
                ;
                if (t.transform.localPosition.y < WaterLevel)
                {
                    t.SetActive(false);
                }
                else
                {
                    t.SetActive(true);
                }
            }
            foreach (GameObject h in Houses)
            {
                if (h.transform.localPosition.y - 1.5 < WaterLevel)
                {
                    h.SetActive(false);
                }
                else
                {
                    h.SetActive(true);
                }
            }
        }
    }

    private void Run()
    {
        List<uint> colors = new List<uint>();
        points = new List<Vector2>();

        for (int i = 0; i < pointCount; i++)
        {
            colors.Add(0);
            points.Add(new Vector2(
                    UnityEngine.Random.Range(0, mapWidth),
                    UnityEngine.Random.Range(0, mapHeight))
            );
        }
        v = new Voronoi(points, colors, new Rect(0, 0, mapWidth, mapHeight));
        v.LloydRelaxation(2);
        points = new List<Vector2>();
        foreach (KeyValuePair<Vector2, Site> kv in v.SitesIndexedByLocation)
        {
            points.Add(new Vector2(kv.Key.x, kv.Key.y));
            //kv.Value.Height = Mathf.PerlinNoise(kv.Key.x, kv.Key.y) * 10000;
        }
        TriangleNet.Geometry.Polygon poly = new TriangleNet.Geometry.Polygon();
        for (int i = 0; i < points.Count; i++)
        {
            poly.Add(new TriangleNet.Geometry.Vertex(points[i].x, points[i].y));
        }
        TriangleNet.Meshing.ConstraintOptions options = new TriangleNet.Meshing.ConstraintOptions() { ConformingDelaunay = true };
        mesh = (TriangleNet.Mesh)poly.Triangulate(options);
        //gameObject.transform.Rotate(0, 90, 0);

        float[] seed = new float[octaves];
        for (int i = 0; i < octaves; i++)
        {
            //Generates a random seed for map generation.
            seed[i] = UnityEngine.Random.Range(0f, 100f);
        }
        foreach (TriangleNet.Geometry.Vertex vert in mesh.Vertices)
        {
            float elevation = 0.0f;
            float amplitude = Mathf.Pow(persistence, octaves);
            float frequency = 1.0f;
            float maxVal = 0.0f;
            for (int i = 0; i < octaves; i++)
            {
                //Seed is used to get a random number from Perlin noise along with user defined values
                float sample = Mathf.PerlinNoise(seed[i] + ((float)vert.x * sampSize / (float)mapWidth * frequency),
                    seed[i] + ((float)vert.y * sampSize / (float)mapHeight * frequency));
                //Sample from Perlin noise is then used to make height map
                elevation += sample;
                maxVal += amplitude;
                //Changing the height of "hills" and "valleys"
                amplitude /= persistence;
                //Increase the amount of "hills" looked at.
                frequency *= frequencyBase;
            }
            elevation = elevation / maxVal;
            elev.Add(elevation * elevScale);
        }
        MakeMesh();
        //Destroy(GameObject.Find("Chunk"));
        //Debug.Log(elev.Max(x => x));
        //Debug.Log(elev.Min(x => x));
        ChangeCubes();
        //PlaceTrees();
        //this.transform.position = new Vector3(-mapWidth / 2, 0, -mapHeight / 2);
    }

    public void PlaceTrees()
    {
        //Debug.Log(meshPoints.Count);
        List<Vector3> terrainICanPlaceOn = meshPoints.Where(x => x.y > waterLevel).ToList();
        for (int i = 0; i < DataHolder.ForestNumber; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, terrainICanPlaceOn.Count - 1);
            GameObject treeClone = Instantiate(tree);//, highEnough[randIndex], transform.rotation);
            TreeList.Add(treeClone);
            treeClone.AddComponent<MeshRenderer>();
            treeClone.transform.localPosition = terrainICanPlaceOn[randIndex];
            treeClone.transform.parent = transform;
            //float randFloat = (float)Math.Log(1 - UnityEngine.Random.Range(0, 0.9999999f));
            int randNumOfTrees = UnityEngine.Random.Range(DataHolder.AverageTrees - 300, DataHolder.AverageTrees + 300);//(int)(-1 * randFloat * DataHolder.AverageTrees);
            Debug.Log(randNumOfTrees);
            for (int j = 0; j < randNumOfTrees; j++)
            {
                var hitInfo = new RaycastHit();
                float height = terrainICanPlaceOn.Max(x => x.y) + 1;
                if (Physics.Raycast(
                    new Vector3(treeClone.transform.position.x + UnityEngine.Random.Range(-DataHolder.ForestRadius, DataHolder.ForestRadius) + UnityEngine.Random.Range(-DataHolder.ForestRadius / 3 * 2, DataHolder.ForestRadius / 3 * 2),
                        height,
                        treeClone.transform.position.z + UnityEngine.Random.Range(-DataHolder.ForestRadius, DataHolder.ForestRadius) + UnityEngine.Random.Range(-DataHolder.ForestRadius / 3 * 2, DataHolder.ForestRadius / 3 * 2)),
                    Vector3.down, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.name.ToString() == "Chunk(Clone)")
                    {
                        GameObject tempTreeClone = Instantiate(tree);
                        TreeList.Add(tempTreeClone);
                        tempTreeClone.AddComponent<MeshRenderer>();
                        float xZScale = 1 + UnityEngine.Random.Range(-DataHolder.TreeVariance, DataHolder.TreeVariance);
                        tempTreeClone.transform.localScale = new Vector3(xZScale,
                            1 + UnityEngine.Random.Range(-DataHolder.TreeVariance, DataHolder.TreeVariance),
                            xZScale);
                        Vector3 euler = transform.eulerAngles;
                        euler.y = UnityEngine.Random.Range(0f, 360f);
                        tempTreeClone.transform.eulerAngles = euler;
                        tempTreeClone.transform.localPosition = hitInfo.point;
                        tempTreeClone.transform.parent = transform;
                    }
                }
            }
        }
    }

    private void ChangeCubes()
    {
        GameObject c1 = GameObject.Find("CubeOne");
        GameObject c2 = GameObject.Find("CubeTwo");
        GameObject c3 = GameObject.Find("CubeThree");
        GameObject c4 = GameObject.Find("CubeFour");
        c1.transform.localScale = new Vector3(mapWidth, elev.Max(x => x), 30);
        c1.transform.localPosition = new Vector3(mapWidth / 2, (elev.Max() + elev.Min()) / 2, 0);
        c2.transform.localScale = new Vector3(mapWidth, elev.Max(x => x), 30);
        c2.transform.localPosition = new Vector3(mapWidth / 2, (elev.Max() + elev.Min()) / 2, mapHeight);
        c3.transform.localScale = new Vector3(30, elev.Max(x => x), mapHeight);
        c3.transform.localPosition = new Vector3(0, (elev.Max() + elev.Min()) / 2, mapHeight / 2);
        c4.transform.localScale = new Vector3(30, elev.Max(x => x), mapHeight);
        c4.transform.localPosition = new Vector3(mapWidth, (elev.Max() + elev.Min()) / 2, mapHeight / 2);
    }

    private void MakeMesh()
    {
        MinHeight = float.MaxValue;
        MaxHeight = float.MinValue;
        IEnumerator<TriangleNet.Topology.Triangle> triEnum = mesh.Triangles.GetEnumerator();
        for (int i = 0; i < mesh.Triangles.Count; i += trianglesInChunk)
        {
            //V E R T I C E S   
            List<Vector3> vertices = new List<Vector3>();
            //P E R - V E R T E X   N O R M A L S
            List<Vector3> normals = new List<Vector3>();
            //P E R - V E R T E X   U V S ( U N U S E D )
            List<Vector2> uvs = new List<Vector2>();
            //T R I A N G L E S
            List<int> triangles = new List<int>();
            int chunkEnd = i + trianglesInChunk;
            for (int j = i; j < chunkEnd; j++)
            {
                if (!triEnum.MoveNext())
                {
                    break;
                }
                TriangleNet.Topology.Triangle tri = triEnum.Current;
                Vector3 v0 = GetPoint3D(tri.vertices[2].id);
                Vector3 v1 = GetPoint3D(tri.vertices[1].id);
                Vector3 v2 = GetPoint3D(tri.vertices[0].id);
                if (v0.y > MaxHeight)
                {
                    MaxHeight = v0.y;
                }
                else if (v0.y < MinHeight)
                {
                    MinHeight = v0.y;
                }
                if (v1.y > MaxHeight)
                {
                    MaxHeight = v1.y;
                }
                else if (v1.y < MinHeight)
                {
                    MinHeight = v1.y;
                }
                if (v2.y > MaxHeight)
                {
                    MaxHeight = v2.y;
                }
                else if (v2.y < MinHeight)
                {
                    MinHeight = v2.y;
                }

                triangles.Add(vertices.Count);
                triangles.Add(vertices.Count + 1);
                triangles.Add(vertices.Count + 2);

                vertices.Add(v0);
                vertices.Add(v1);
                vertices.Add(v2);

                //if(!meshPoints.Contains(v0))
                //{
                meshPoints.Add(v0);
                //}
                //if (!meshPoints.Contains(v1))
                //{
                meshPoints.Add(v1);
                //}
                //if (!meshPoints.Contains(v2))
                //{
                meshPoints.Add(v2);
                //}

                Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0);
                normals.Add(normal);
                normals.Add(normal);
                normals.Add(normal);

                uvs.Add(new Vector2(0.0f, 0.0f));
                uvs.Add(new Vector2(0.0f, 0.0f));
                uvs.Add(new Vector2(0.0f, 0.0f));
            }
            UnityEngine.Mesh chunkMesh = new UnityEngine.Mesh();
            chunkMesh.vertices = vertices.ToArray();
            chunkMesh.uv = uvs.ToArray();
            chunkMesh.triangles = triangles.ToArray();
            chunkMesh.normals = normals.ToArray();

            Transform chunk = Instantiate<Transform>(chunkPrefab, transform.position, transform.rotation);
            chunk.GetComponent<MeshFilter>().mesh = chunkMesh;
            chunk.GetComponent<MeshCollider>().sharedMesh = chunkMesh;
            chunk.transform.parent = transform;
            //this.GetComponent<MeshFilter>().mesh = chunkMesh;
            //this.GetComponent<MeshCollider>().sharedMesh = chunkMesh;
        }
        waterLevel = (MaxHeight + MinHeight) / 2;
        Debug.Log(MaxHeight + " " + MinHeight + " " + waterLevel);
        UnityEngine.Mesh waterMesh = new UnityEngine.Mesh();
        waterMesh.vertices = new Vector3[] { new Vector3(mapWidth, waterLevel, mapHeight),
            new Vector3(0, waterLevel, mapHeight),
            new Vector3(0 , waterLevel, 0),
            new Vector3(mapWidth, waterLevel, 0) };
        waterMesh.triangles = new int[] { 3, 1, 0,
            3, 2, 1
             };
        //waterMesh.uv = new Vector2[] { new Vector2(0f, 0f),
        //    new Vector2(0f, 0f),
        //    new Vector2(0f, 0f),
        //    new Vector2(0f, 0f) };
        waterMesh.normals = new Vector3[] { Vector3.up,
         Vector3.up,
         Vector3.up,
         Vector3.up};
        Transform water = Instantiate<Transform>(waterPrefab, transform.position, transform.rotation);
        water.GetComponent<MeshFilter>().mesh = waterMesh;
        water.GetComponent<MeshCollider>().sharedMesh = waterMesh;
        water.transform.parent = transform;
    }

    public Vector3 GetPoint3D(int index)
    {
        TriangleNet.Geometry.Vertex vertex = mesh.vertices[index];
        float elevation = elev[index];
        return new Vector3((float)vertex.x, elevation, (float)vertex.y);
    }

    private void OnDrawGizmos()
    {
        if (mesh == null)
        {
            // We're probably in the editor
            return;
        }

        Gizmos.color = Color.red;
        foreach (TriangleNet.Geometry.Edge edge in mesh.Edges)
        {
            TriangleNet.Geometry.Vertex v0 = mesh.vertices[edge.P0];
            TriangleNet.Geometry.Vertex v1 = mesh.vertices[edge.P1];
            Vector3 p0 = new Vector3((float)v0.x, 0.0f, (float)v0.y);
            Vector3 p1 = new Vector3((float)v1.x, 0.0f, (float)v1.y);
            Gizmos.DrawLine(p0, p1);
        }


        ////draw points
        //Gizmos.color = Color.red;
        //if (points != null)
        //{
        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        Gizmos.DrawSphere(points[i], .2f);
        //    }
        //}

        ////draw triangles
        //Gizmos.color = Color.magenta;
        //if (delaunayTriangulation != null)
        //{
        //    for (int i = 0; i < delaunayTriangulation.Count; i++)
        //    {
        //        Vector2 left = (Vector2)delaunayTriangulation[i].p0;
        //        Vector2 right = (Vector2)delaunayTriangulation[i].p1;
        //        Gizmos.DrawLine((Vector3)left, (Vector3)right);
        //    }
        //}

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(new Vector2(0, 0), new Vector2(0, mapHeight));
        //Gizmos.DrawLine(new Vector2(0, 0), new Vector2(mapWidth, 0));
        //Gizmos.DrawLine(new Vector2(mapWidth, 0), new Vector2(mapWidth, mapHeight));
        //Gizmos.DrawLine(new Vector2(0, mapHeight), new Vector2(mapWidth, mapHeight));
    }
}
