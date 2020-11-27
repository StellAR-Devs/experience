﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcoPlanet : MotherPlanet{
    [Range(0, 5)]
    public int LOD;

    // for planetGeneration
    IcoSphere icoSphere;
    [SerializeField, HideInInspector]
    MeshFilter meshFilter;
    MouseInteraction interaction;

    public override void Initialize(){
        if(shapeSettings == null || colorSettings == null){
            ShapeSettings tempShapeSettings = SettingSpawner.loadDefaultShape();
            ColorSettings tempColSettings = SettingSpawner.loadDefaultColor();
            shapeSettings = tempShapeSettings;
            colorSettings = tempColSettings;
        }

        if(interaction == null){
            interaction = GetComponent<MouseInteraction>();
        }

        if(this.GetComponent<SphereCollider>() == null){
            this.gameObject.AddComponent<SphereCollider>();
        }

        if(craterSettings == null){
            CraterSettings tempCraterSettings = SettingSpawner.loadDefaultCraters();
            craterSettings = tempCraterSettings;
        }

        if(this.transform.Find("mesh") == null){ // no meshObj initialized yet
            GameObject meshObj = new GameObject("mesh");
            meshObj.transform.parent = transform;
            meshObj.AddComponent<MeshRenderer>(); //.material = (Material) Resources.Load("defaultMat");
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();     
            meshObj.GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
        }
        else{
            GameObject meshObj = this.transform.Find("mesh").gameObject;
            meshFilter = meshObj.GetComponent<MeshFilter>();
            meshObj.GetComponent<MeshRenderer>(); //.material = (Material) Resources.Load("defaultMat");
            meshFilter.sharedMesh = new Mesh();
            meshObj.GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

        }

        craterGenerator = new CraterGenerator(craterSettings);
        shapeGenerator = new ShapeGenerator(shapeSettings, interaction, craterGenerator);
        colorGenerator = new ColorGenerator();

        colorGenerator.UpdateSettings(colorSettings);

        icoSphere = new IcoSphere(shapeGenerator, shapeSettings.radius, LOD, meshFilter.sharedMesh); 
        icoSphere.UpdateUVs();   
    }

    public override void GenerateMesh(){
        icoSphere.ConstructMesh();
    }

    public override void UpdateMesh(){
        if(icoSphere == null){
            Initialize();
            icoSphere.ConstructMesh();
            icoSphere.UpdateMesh();
        }
        else{
            icoSphere.UpdateMesh();
        }           
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    public override void GenerateColors(){   
        //Debug.Log(colorGenerator);
        //icoSphere.UpdateUVs();
        colorGenerator.UpdateColors();
        if(meshFilter.gameObject.activeSelf){
            //icoSphere.UpdateUVs();
        }
    }
    
    public override void OnCraterSettingsUpdated(){ //Rebuild planet when color is updated
        if (autoUpdate)
        {
            clearCraters = false;
            Initialize();
            GenerateMesh();
        }
    }


    public void MakeCrater(Collision collision, float otherRadius)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point - this.transform.localPosition;
        Vector3 planetRotEuler = gameObject.transform.localRotation.eulerAngles;
        Quaternion rotation = Quaternion.AngleAxis(-planetRotEuler[2], Vector3.forward)
            * Quaternion.AngleAxis(-planetRotEuler[0], Vector3.right)
            * Quaternion.AngleAxis(-planetRotEuler[1], Vector3.up);
        position = rotation * position;

        float velocity = collision.relativeVelocity.magnitude;
        craterSettings.impact = Mathf.Min(0.1f + velocity / 2, 1.6f);
        craterSettings.radius = otherRadius * 0.6f;
        shapeGenerator.craterGenerator.CreateCrater(position.normalized, 1f);
        UpdateMesh();
    }

}
