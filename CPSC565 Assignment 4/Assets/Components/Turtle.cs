using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turtle : MonoBehaviour
{

    // Fields:

    /// <summary>
    /// The Material list used to make the shapes via LGeom
    /// </summary>
    public List<Material> MaterialList;

    /// <summary>
    /// The output L System mutated string passed by WorldManager
    /// </summary>
    public string LSystemCommand;

    /// <summary>
    /// This stack helps turtle memorise and load the position to spawn new shapes from
    /// </summary>
    public Stack<Vector3> memoryPositions;

    /// <summary>
    /// This is the position of the shape turtle spawns it at. It changes as shapes get spawned.
    /// </summary>
    public Vector3 position;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LSystemCommand = null;
        memoryPositions = new Stack<Vector3>();
        position = new Vector3(0,0,0);

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
        if(LSystemCommand is not null)
        {
            Debug.Log("Turtle received: " + LSystemCommand);
            LSystemCommand = null;
        }
    }
}
