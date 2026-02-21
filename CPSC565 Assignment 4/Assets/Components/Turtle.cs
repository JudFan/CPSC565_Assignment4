using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turtle : MonoBehaviour
{

    // Fields:
    public List<Material> MaterialList;
    public string LSystemCommand;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Making a shape");
        List<Vector3> list =  new List<Vector3>();
        list.Add(new Vector3(0,0,0));
        list.Add(new Vector3(1,1,0));
        list.Add(new Vector3(0,0,1));
        GameObject shape = LGeom.FilledPolygon_Triangles(list, material: MaterialList[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Execute()
    {
        Debug.Log("Executed");
    }
}
