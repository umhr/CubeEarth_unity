using System; //Exception
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    // Use this for initialization
    private uint count = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        count++;
        float tz = (float)Math.Sin((float)count / 300) * 400 + 200;
        GetComponent<Transform>().position = new Vector3(0, 0, -tz);
    }
}
