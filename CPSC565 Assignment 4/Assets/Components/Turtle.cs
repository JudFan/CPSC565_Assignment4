using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assign4.UI;

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

    /// <summary>
    /// The UI it grabs info from
    /// </summary>
    public UI_for_Setup ui;

    /// <summary>
    /// The list of petals it will control to bloom
    /// </summary>
    private List<GameObject> petals;

    public GameObject shape;
    private int height;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LSystemCommand = null;
        memoryPositions = new Stack<Vector3>();
        position = new Vector3(0,0,0);
        height = 0;

        Debug.Log("Making a shape");
        List<Vector3> list =  new List<Vector3>();
        list.Add(new Vector3(0,1,0));
        list.Add(new Vector3(1,0,0));
        list.Add(new Vector3(0,0,1));
        shape = LGeom.FilledPolygon_Fan(list, material: MaterialList[1]);

    }

    // Update is called once per frame
    void Update()
    {
        if(LSystemCommand is not null)
        {
            Debug.Log("Turtle received: " + LSystemCommand);
            LSystemCommand = null;
        }
        shape.transform.RotateAround(new Vector3(1,0,1), new Vector3(-1,0,1), 2);
        // Divide ui.Angle by 2 so you know how many times we need to unfurl the petal

        // Also make a symbol to add a game object (petal) to a list of objects to rotate (and what axis to rotate by)
    }


    // A helper function that grows a plant stem: Activates on Symbol 'G'
    void Grow()
    {
        Vector3 endpt = new Vector3(0, height + 1, 0);

        GameObject stem = LGeom.Cylinder(position, endpt, 0.5f, material: MaterialList[0]);
        position = endpt;
    }
}
