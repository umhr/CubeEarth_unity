using System; //Exception
using System.Collections.Generic;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using UnityEngine;

public class MainCube : MonoBehaviour
{

    private GameObject cube0;
    private List<GameObject> cubeList;
    private float RADTODEG = 57.29577951308232f;
    // Use this for initialization
    void Start()
    {
        cube0 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 v3 = cube0.transform.position;
        //v3.x = -3;
        cube0.transform.position = v3;
        cube0.GetComponent<BoxCollider>().enabled = false;
        cube0.GetComponent<MeshRenderer>().enabled = false;
        cube0.GetComponent<Renderer>().material.color = new Color(0, 1, 0);

        addCubes();

    }

    // Update is called once per frame
    void addCubes()
    {
        Debug.Log(Application.dataPath);
        Debug.Log(Application.persistentDataPath);
        string stCurrentDir = System.IO.Directory.GetCurrentDirectory();
        Debug.Log(stCurrentDir);

        Texture2D readTexture = ReadTexture("Assets/earthmap500b.png");

        cubeList = new List<GameObject>();
        float r = 200;
        int n = 41;
        for (int i = 0; i < n; i++)
        {
            float dy = (float)(r * Math.Cos(Math.PI * ((float)i / n)));
            float dx = (float)Math.Sqrt(r * r - dy * dy);
            int m = (int)Math.Floor(n * 2 * (dx / r));

            for (int j = 0; j < m; j++)
            {
                float radian = (float)Math.PI * 2 * ((float)j / m);

                for (int k = 0; k < 5; k++)
                {
                    float scale = 1.0f;
                    if (k > 0)
                    {
                        scale = UnityEngine.Random.Range(0.1f, 0.95f);
                    }

                    Vector3 rotate = new Vector3((180 * i / n + 90), -(radian * RADTODEG + 90));
                    Color color = readTexture.GetPixel(500 * j / m, 250 * i / n);
                    cubeList.Add(getCube((float)(dx * Math.Cos(radian)), -dy, (float)(dx * Math.Sin(radian)), rotate, color, scale));
                }

            }
        }

    }

    GameObject getCube(float x, float y, float z, Vector3 rotate, Color color, float scale)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 position = cube.transform.position;
        position.x = x * scale;
        position.y = y * scale;
        position.z = z * scale;
        // 親を指定する。
        cube.transform.parent = cube0.transform;
        cube.transform.position = position;
        cube.transform.Rotate(rotate);
        cube.transform.localScale = new Vector3(scale * 11.0f, scale * 11.0f, scale * 11.0f);
        cube.GetComponent<Renderer>().material.color = color;

        cube.GetComponent<BoxCollider>().enabled = false;
        cube.GetComponent<MeshRenderer>().receiveShadows = false;
        cube.GetComponent<MeshRenderer>().useLightProbes = false;
        cube.GetComponent<MeshRenderer>().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        cube.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return cube;
    }

    // http://macomu.sakura.ne.jp/blog/?p=55
    byte[] ReadPngFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);
        bin.Close();
        return values;
    }

    Texture2D ReadTexture(string path)
    {
        // http://qiita.com/r-ngtm/items/6cff25643a1a6ba82a6c
        byte[] readBinary = ReadPngFile(path);
        int pos = 16; // 16バイトから開始

        int width = 0;
        for (int i = 0; i < 4; i++)
        {
            width = width * 256 + readBinary[pos++];
        }

        int height = 0;
        for (int i = 0; i < 4; i++)
        {
            height = height * 256 + readBinary[pos++];
        }

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(readBinary);
        return texture;
    }
    // Update is called once per frame
    void Update()
    {
        cube0.transform.Rotate(0.0f, -0.4f, 0.0f);
    }
}