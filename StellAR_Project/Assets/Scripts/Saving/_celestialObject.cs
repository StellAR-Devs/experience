﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct _celestialObject 
{
    public Vector3 position;
    public Vector3 velocity;
    public bool staticBody;
    public float mass;
    public float[] rotation;

    public _celestialObject(CelestialObject planet){
        position = planet.GetPosition();
        velocity=planet.velocity;
        staticBody=planet.staticBody;
        mass =planet.mass;
        rotation = planet.gameObject.GetComponent<RotationSim>().GetRotation();

    }
}
