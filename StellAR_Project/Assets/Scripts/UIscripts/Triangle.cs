﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour{
    Mesh mesh;
    MeshRenderer meshRenderer;
    Vector3[] vertices;
    int[] triangles;
    Ray ray;
    RaycastHit2D hit;
    Transform selection;
    Renderer selectionRenderer;
    BaryCentric bary;
    GameObject handle;
    PolygonCollider2D col;
    Vector2[] colPoints;
    Vector2 worldPos;
    

    public Material material;

    [Range(1,10)]
    public float size;
    RaycastHit hit2;

    void Awake(){
        if(gameObject.GetComponent<MeshFilter>() == null){
            Debug.Log("adding");
            gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            col = gameObject.AddComponent<PolygonCollider2D>();
        }
        else{
            Debug.Log("getting");
            //gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            col = gameObject.GetComponent<PolygonCollider2D>();
        }
        
        meshRenderer.material = material;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(0.5f,0.866025404f,0)*size,
            new Vector3(1,0,0)*size
        };

        triangles = new[]{0,1,2};
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        // set handle sprite
        handle = this.transform.GetChild(0).gameObject;

        // create shape of collider 
        colPoints = new Vector2[3];
        for(int i = 0; i < 3; i++){
            colPoints[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        col.pathCount = 1;
        col.SetPath(0, colPoints);
    }
  
    void Update(){
        if(Input.GetMouseButton(0)){
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z)) * -1f;
            hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if(hit){
                selection = hit.transform;
                //selectionRenderer = selection.GetComponent<Renderer>();
                handle.transform.position = hit.point;
                //Debug.Log(BaryCentric.getWeights(handle.transform.position, vertices));
                UpdateColor(BaryCentric.getWeights(handle.transform.position, vertices));
            }
        }  
    }
    void UpdateColor(Vector3 weights){
        Color newColor = new Color(weights.x, weights.y, weights.z, 0.5f);
        //Debug.Log(meshRenderer);
        meshRenderer.material.SetColor("_Color", newColor);
    }
}
