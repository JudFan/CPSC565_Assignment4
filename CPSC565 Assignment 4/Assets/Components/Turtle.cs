using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assign4.UI;

public struct petal {
    public GameObject item;
    public Vector3 positionToRotateAbout;
    public Vector3 axisToRotateAbout;
}

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
    public Stack<Vector3> memoryPos;

    /// <summary>
    /// This is the position of the shape turtle spawns it at. It changes as shapes get spawned.
    /// </summary>
    public Vector3 position;
    

    /// <summary>
    /// The UI it grabs info from
    /// </summary>
    public UI_for_Setup ui;

    /// <summary>
    /// The list of GameObjects
    /// </summary>
    private List<GameObject> objects;

    /// <summary>
    /// The list of petals shapes
    /// </summary>
    private List<petal> petals;

    /// <summary>
    /// The list of petals shapes that will bloom
    /// </summary>
    private List<petal> bloomingPetals;

    /// <summary>
    /// The height of the plant
    /// </summary>
    private int height;

    /// <summary>
    /// The angle used to set polar coordinates for the petals
    /// </summary>
    private float polarAngle;

    /// <summary>
    /// The number of moves needed to bloom the flower
    /// </summary>
    private int move;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LSystemCommand = null;
        memoryPos = new Stack<Vector3>();
        bloomingPetals = new List<petal>();
        objects = new List<GameObject>();
        petals = new List<petal>();
        position = new Vector3(0,0,0);
        height = 0;
        polarAngle = 0.0f;
        move = 0;

        //LSystemCommand = "Unnsweww[F]UUsss";//"GGGGG[[PApA][PApA][PApA][PApA][PApA]]BEGGGGG";
    }

    // Update is called once per frame
    void Update()
    {
        int numOfMoves = (int)(ui.angle / 0.1);
         
        if(LSystemCommand is not null && LSystemCommand.Length > 0)
        {
            Debug.Log("Turtle received: " + LSystemCommand);
            switch (LSystemCommand[0])
            {
                // L System 1 scenarios
                case 'A':
                    //Debug.Log('A');
                    AddToBloom();
                    break;

                case 'B':
                    //Debug.Log('B');
                    Bloom();
                    if(move < numOfMoves)
                    {
                        LSystemCommand = LSystemCommand.Insert(0, "B");
                    }
                    move++;
                    break;
                
                case 'b':
                    //Debug.Log('b');
                    // Setup bloom procedure
                    move = 0;
                    Bud();
                    break;

                case 'G':
                    //Debug.Log('G');
                    Grow();
                    break;

                case 'P':
                    //Debug.Log('P');
                    SetupPetal();
                    break;

                case 'p':
                    //Debug.Log('p');
                    SetupPetal();
                    break;


                // L System 2 scenarios

                case 'F':
                    //Debug.Log('F');
                    MakeFruit();
                    break;

                case 'U':
                    //Debug.Log('U');
                    GrowTree();
                    break;

                case 'n':
                    //Debug.Log('n');
                    GrowTree();
                    break;

                case 's':
                    //Debug.Log('s');
                    GrowTree();
                    break;

                case 'e':
                    //Debug.Log('e');
                    GrowTree();
                    break;

                case 'w':
                    //Debug.Log('w');
                    GrowTree();
                    break;

                // Common L System operation
                case '[':
                    //Debug.Log('[');
                    memoryPos.Push(position);
                    //Debug.Log("Position saved: " + position);
                    break;

                case ']':
                    //Debug.Log(']');
                    try {
                        position = memoryPos.Pop();
                    }
                    catch (Exception ex) {}
                    height = (int)position.y;
                    //Debug.Log("New Position Loaded: " + position);
                    break;

                default:
                    break;
            }
            LSystemCommand = LSystemCommand.Remove(0,1);
        }

        // Also make a symbol to add a game object (petal) to a list of objects to rotate (and what axis to rotate by)
    }

    // Helper function that resets structure whenever the start button is called.
    public void Reset()
    {
        foreach(GameObject obj in objects)
        {
            Destroy(obj);
        }

        memoryPos = new Stack<Vector3>();
        bloomingPetals = new List<petal>();
        objects = new List<GameObject>();
        petals = new List<petal>();
        position = new Vector3(0,0,0);
        height = 0;
        polarAngle = 0.0f;
        move = 0;
    }


    // A helper function that grows a plant stem: Activates on Symbol 'G'
    void Grow()
    {
        Vector3 endpt = new Vector3(0, height + 1, 0);

        GameObject stem = LGeom.Cylinder(position, endpt, 0.5f, material: MaterialList[0]);
        objects.Add(stem);
        position = endpt;
        height++;
    }

    void Bud() {
        GameObject bud = LGeom.Sphere(position, 0.75f, material: MaterialList[1]);
        objects.Add(bud);
    }

    void SetupPetal()
    {
        float polarSeparationDegree = 360 / 5;
        float polarSeparationRadian = (polarSeparationDegree * MathF.PI)/180;

        position = new Vector3(MathF.Sin(polarAngle), height, MathF.Cos(polarAngle));
        polarAngle += polarSeparationRadian;
        Vector3 endPos = new Vector3(MathF.Sin(polarAngle), height, MathF.Cos(polarAngle));

        Vector3 axis = new Vector3(position.x - endPos.x, 0, position.z - endPos.z);
        
        Vector3 refPoint; 
        GameObject shape;

        //Scenario1: Building foundation of petal (rectangle/square)
        if(LSystemCommand[0] == 'P')
        {
            Vector3 startPosUp = new Vector3(position.x, height + 2, position.z);
            Vector3 endPosUp = new Vector3(endPos.x, height + 2, endPos.z);

            refPoint = new Vector3((position.x - endPos.x)/2, height, (position.z - endPos.z)/2);

            List<Vector3> list =  new List<Vector3>();
            list.Add(position);
            list.Add(endPos);
            list.Add(startPosUp);
            list.Add(endPosUp);
            shape = LGeom.FilledPolygon_Strip(list, material: MaterialList[1]);

            polarAngle -= polarSeparationRadian;
            height += 2;
        }
        //Scenario2: Building edge of petal (triangle)
        else
        {
            Vector3 point = new Vector3(0, height + 2, 0);

            refPoint = new Vector3((position.x - endPos.x)/2, height-2, (position.z - endPos.z)/2);

            List<Vector3> list =  new List<Vector3>();
            list.Add(position);
            list.Add(endPos);
            list.Add(point);
            shape = LGeom.FilledPolygon_Triangles(list, material: MaterialList[1]);
        }

        petal petalIn;
        petalIn.item = shape;
        petalIn.axisToRotateAbout = axis;
        petalIn.positionToRotateAbout = refPoint;

        petals.Add(petalIn);
        objects.Add(shape);
    }

    void AddToBloom()
    {
        bloomingPetals.Add(petals[petals.Count - 1]);
    }

    void Bloom()
    {
        // Divide ui.Angle by 2 so you know how many times we need to unfurl the petal
            foreach(petal petalPiece in bloomingPetals)
            {
                petalPiece.item.transform.RotateAround(petalPiece.positionToRotateAbout, petalPiece.axisToRotateAbout, -0.1f);
            }
    }

    // 2nd idea: use randomised param to make a turtle grow a tree, and randomly decide if the tree sprouts more branches/leaves or sprouts a fruit.
    // Improvement: 
    void GrowTree()
    {
        float xDeviation = 0.0f;
        float zDeviation = 0.0f;
        float heightDeviation = 0.0f;
        float deviationRadian = (ui.angle * MathF.PI)/180;

        switch (LSystemCommand[0])
        {

            case 'n':
                zDeviation += (float)(3 * MathF.Sin(deviationRadian));
                heightDeviation += (float)(3 * MathF.Cos(deviationRadian));
                break;

            case 's':
                zDeviation -= (float)(3 * MathF.Sin(deviationRadian));
                heightDeviation += (float)(3 * MathF.Cos(deviationRadian));
                break;

            case 'e':
                xDeviation += (float)(3 * MathF.Sin(deviationRadian));
                heightDeviation += (float)(3 * MathF.Cos(deviationRadian));
                break;

            case 'w':
                xDeviation -= (float)(3 * MathF.Sin(deviationRadian));
                heightDeviation += (float)(3 * MathF.Cos(deviationRadian));
                break;

            default:
            heightDeviation += 3;
                break;
        }
        
        Vector3 endpt = new Vector3((float)(position.x + xDeviation), height + heightDeviation, (float)(position.z + zDeviation));

        GameObject treeStem  = LGeom.Cylinder(position, endpt, 1, material: MaterialList[2]);
        objects.Add(treeStem);
        position = endpt;
        height += 5;
    }

    void MakeFruit()
    {
        position.y--;
        Vector3 endpt = new Vector3(position.x, position.y - 1, position.z);
        GameObject fruitStem = LGeom.Cylinder(position, endpt, 0.1f, material: MaterialList[0]);
        position = endpt;
        Vector3 center = new Vector3(position.x, position.y - 1.5f, position.z);
        GameObject fruit = LGeom.Sphere(center, 1.5f, material: MaterialList[1]);
        objects.Add(fruitStem);
        objects.Add(fruit);
    }

}
