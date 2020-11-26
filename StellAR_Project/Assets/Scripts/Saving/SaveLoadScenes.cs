﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadScenes : MonoBehaviour
{

    public int sceneIndex;
    public bool save = false;
    public bool load = false;
    public bool loadNewPlanet = false;
    public bool delete = false;
    public GameObject rockPrefab;
    public GameObject gasPrefab;
    bool gasy = false;
    bool rocky = false;
    int startingIndex = 0;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 1)
        {
            //load = true;
            loadNewPlanet = true;
        }
    }

    void Update()
    {
        if(save){
            if(sceneIndex == 2){
                SaveLoadStarSystem.SaveStarSystem(true);
                save=false;
            }
            else if (sceneIndex == 1)
            {
                SaveLoadStarSystem.SaveStarSystem(false);
                save = false;
            }
            /*
            else if (sceneIndex == 8)
                //Maybe use for going from planet creation to solar system when there's just one planet in the scene.
            {
                GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
                if (planets.Length > 0)
                {
                    MotherPlanet mp = planets[0].GetComponent<MotherPlanet>();
                    SaveLoadStarSystem.SavePlanet(mp);
                }
                save = false;
            }
            */
        }
        if(load){
            if(sceneIndex == 1){
                SystemSimulationData data = SaveLoadStarSystem.LoadStarSystem(false);
                if( data != null){

                    CelestialObject.DestroyAll();
                    int rocky_i = 0;
                    int gasy_i = 0;
                    for (int i=0; i<data.planetCount; i++){
                        GameObject obj = getPrefab(data, i);
                        obj.GetComponent<CelestialObject>().enabled = true;
                        CelestialObject co = obj.GetComponent<CelestialObject>();
                        co.SetState(data.physicsData[i]);

                        MotherPlanet mp = obj.GetComponentInChildren<MotherPlanet>();
                        if (mp != null)
                        {
                            mp.GeneratePlanet();
                            mp.SetShape(data.planetList[rocky_i]);
                            mp.UpdateMesh();
                            rocky_i += 1;
                        }
                        GasPlanetShaderMAterialPropertyBlock gp = co.GetComponentInChildren<GasPlanetShaderMAterialPropertyBlock>();
                        if (gp != null)
                        {
                            gp.SetMaterial(data.gasPlanetList[gasy_i]);
                            gasy_i += 1;
                        }

                    }
                    load =false;
                }
            }
            /*
            else if (sceneIndex == 8){
                PlanetData data = SaveLoadStarSystem.LoadPlanets();
                if (data != null)
                {
                    CelestialObject.DestroyAll();
                    GameObject obj = Instantiate(rockPrefab);
                    obj.GetComponent<MotherPlanet>().enabled = true;
                    MotherPlanet mo = obj.GetComponent<MotherPlanet>();
                    mo.GeneratePlanet();
                    mo.SetShape(data.planetData);
                    mo.UpdateMesh();
                    load = false;
                }
            }
            */
            else{
                load=false;
                Debug.Log("failed to load");
            }
                
        }
        if (loadNewPlanet)
        {
            if (sceneIndex == 1)
            {
                SystemSimulationData data = SaveLoadStarSystem.LoadStarSystem(true);
                if (data != null)
                {

                    CelestialObject.DestroyAll();
                    int rocky_i = 0;
                    int gasy_i = 0;
                    for (int i = 0; i < data.planetCount; i++)
                    {
                        GameObject obj = getPrefab(data, i);
                        obj.GetComponent<CelestialObject>().enabled = true;

                        MotherPlanet mp = obj.GetComponentInChildren<MotherPlanet>();
                        if (mp != null)
                        {
                            mp.GeneratePlanet();
                            mp.SetShape(data.planetList[rocky_i]);
                            mp.UpdateMesh();

                            rocky_i += 1;
                        }
                        GasPlanetShaderMAterialPropertyBlock gp = obj.GetComponentInChildren<GasPlanetShaderMAterialPropertyBlock>();
                        if (gp != null)
                        {
                            gp.SetMaterial(data.gasPlanetList[gasy_i]);
                            gasy_i += 1;
                        }

                        GameObject ARSessOrig = GameObject.Find("AR Session Origin");
                        ARPlacementTrajectory placement = ARSessOrig.GetComponent<ARPlacementTrajectory>();
                        placement.setGOtoInstantiate(obj);

                    }
                    loadNewPlanet = false;
                }
            }
        }
        if(delete){
            if(sceneIndex == 0){
                SaveLoadStarSystem.DeleteStarSystem();
                delete=false;
            }
        }

        
    }

    GameObject getPrefab(SystemSimulationData data, int i)
    {
        for (int j = 0; j < data.planetList.Length; j++)
        {
            if (data.planetList[j].id == data.physicsData[i].id)
            {
                return Instantiate(rockPrefab);
            }
        }
        for (int j = 0; j < data.gasPlanetList.Length; j++)
        {
            if (data.gasPlanetList[j].id == data.physicsData[i].id)
            {
                return Instantiate(gasPrefab);
            }
        }
        return Instantiate(gasPrefab);
    }

    public void ToggleSave(){
        save = !save;
    }

    public void ToggleLoad(){
        load = !load;

    }

    public void ToggleDelete(){
        delete = !delete;
    }

    public void NextScene()
    {
        SaveLoadStarSystem.SaveStarSystem(true);
        save = false;
        SceneManager.LoadScene(1);
    }
}
