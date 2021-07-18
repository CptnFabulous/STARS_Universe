using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlanetGenerator : MonoBehaviour
{
    public int numberOfPlanets = 100;
    public Vector3 maxRadius = Vector3.one * 950;
    public float minScale = 1;
    public float maxScale = 10;
    public Mesh[] shapes;
    public Material[] materialTypes;

    void GeneratePlanets()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            Debug.Log("Initialising scale variables");
            Vector3 position = new Vector3(Random.Range(-maxRadius.x, maxRadius.x), Random.Range(-maxRadius.y, maxRadius.y), Random.Range(-maxRadius.z, maxRadius.z));
            Quaternion rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            float scale = Random.Range(minScale, maxScale);
            Vector3 planetScaleValues = new Vector3(scale, scale, scale);
            string name = "Planet #" + (i + 1);
            GameObject newPlanet = new GameObject(name);
            newPlanet.transform.position = position;
            newPlanet.transform.rotation = rotation;
            newPlanet.transform.localScale = planetScaleValues;

            Debug.Log("Assigning mesh data");
            MeshFilter filter = newPlanet.AddComponent<MeshFilter>();
            MeshCollider collider = newPlanet.AddComponent<MeshCollider>();
            Mesh shape = shapes[Random.Range(0, shapes.Length - 1)];
            filter.mesh = shape;
            collider.sharedMesh = shape;

            Debug.Log("Assigning material and colour");
            MeshRenderer renderer = newPlanet.AddComponent<MeshRenderer>();
            renderer.material = materialTypes[Random.Range(0, materialTypes.Length - 1)];
            //Color planetColour = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)/*, Random.Range(0, 255)*/);
            Color planetColour = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1)/*, Random.Range(0, 255)*/);
            renderer.material.color = planetColour;
            Debug.Log("Planet is created");
            //Instantiate(newPlanet, position, rotation);
        }
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        GeneratePlanets();
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
