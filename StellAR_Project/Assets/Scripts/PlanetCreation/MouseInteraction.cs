﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour{
    Ray ray;
    RaycastHit hit;
    Transform selection;
    Renderer selectionRenderer;
    Mesh terrainFaceMesh;
    [HideInInspector]
    public List<Vector3> hitCoords;
    Vector3[] vertices;
    MotherPlanet planet;
    bool craterPlacement = false;
    //bool placingCrater = false;

    [SerializeField]
    public float brushSize = 0.2f;
    public Vector3 interactionPoint;
    float timeToGo;
    
    void Start(){
        planet = gameObject.GetComponent<MotherPlanet>();
        timeToGo = Time.fixedTime + 0.1f;
    }

    void Update(){
        if(Time.fixedTime >=timeToGo){
            timeToGo += 0.1f;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
            {
                Debug.Log("toggle craterCreator");
                craterPlacement ^= true;
            }
            if (Physics.Raycast(ray, out hit)){
                selection = hit.transform;
                if(craterPlacement){
                    if(Input.GetMouseButtonDown(0)){
                        planet.shapeGenerator.craterGenerator.CreateCrater(hit.point);
                    }
                }
                else{
                    if(Input.GetMouseButton(0)){
                        interactionPoint = selection.InverseTransformPoint(hit.point); 
                        //hitCoords.Add(selection.InverseTransformPoint(hit.point));
                    }
                }
                planet.UpdateMesh();
            }   
        }
       
        /*
        if (placingCrater)
        {
            //planet.PlaceCrater(selection.InverseTransformPoint(hit.point));
            planet.shapeGenerator.craterGenerator.CreateCrater(hit.point);
        }
        if (Input.GetMouseButtonUp(0))
        {
            placingCrater = false;
        }
        */
       
    }
    public List<Vector3> GetPaintedVertices(){
        return hitCoords;
    } 
}
