﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class SizeChanger : MonoBehaviour
{
    private GameObject Planet = null;
    private bool Gas = false;
    public Slider SizeSlider;
    private float scaler = 2.5f / Mathf.Log(100);
    //float solidScaler, gasScaler = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
        if (Planet == null)
        {
            Planet = GameObject.FindGameObjectWithTag("Planet");
            if(Planet == null)
            {
                Planet = GameObject.FindGameObjectWithTag("GasPlanet");
                Gas = true;
            }
        }
        if (Gas)
        {
            //gasScaler = 1 / (Mathf.Log(11.2f * 100) / 5f);
           
            SizeSlider.value = 11.20f; //Defaults to Jupiter
            SizeSlider.minValue = 2.50f;
            SizeSlider.maxValue = 14.00f; //Jupiter is 11.2, 95% of all exo planets nasa has confirmed has a radius lower than 13.25
            SizeUpdate(11.20f);
        }
        else 
        {
            // scale the planet to fit in screen 
            //solidScaler = (5*Planet.GetComponent<IcoPlanet>().shapeSettings.radius) / Mathf.Log(100);
            
            // set default sliderValues
            SizeSlider.value = 1.00f; //Defaults to Earth
            SizeSlider.minValue = 0.3f; //Slightly smaller than Mercury
            SizeSlider.maxValue = 2.50f; //Super-terrans
            SizeUpdate(SizeSlider.value);
        }
    }

    public void SizeUpdate(float value) {
        value = Mathf.Log(value*100)/5;
        if (Gas)
        {
            value *= scaler/* * 0.5f*/;
            Planet.transform.localScale = new Vector3(value * 2, value * 2, value * 2); //localscale adjusts diameter, to keep consistency with rocky icospheres we halve it to get a radius
            
        }
        else
        {
            IcoPlanet ico = Planet.GetComponent<IcoPlanet>();
            ico.shapeSettings.radius = value * scaler;
            ico.UpdateMesh();
        }
    }
    public void RandomSize(){
        if (Gas){
            float number = Random.Range(2.50f, 14.00f);
            SizeSlider.value = number;
            SizeUpdate(number);
        }
        else {
            float number = Random.Range(0.3f, 2.50f);
            SizeSlider.value = number;
            SizeUpdate(number);
        }
        
    }
}
