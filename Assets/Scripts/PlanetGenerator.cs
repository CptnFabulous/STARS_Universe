using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
//using Firebase.Database;
using Firebase.Firestore;
//using Firebase.Extensions.TaskExtension; // for ContinueWithOnMainThread
using Firebase.Extensions;
using UnityEngine.Networking;

public class PlanetGenerator : MonoBehaviour
{
    [Header("Paths to database")]
    public string instanceName = "STARS Universe";
    public string databaseCollectionPath = "/starsData";

    [Header("Planet attributes")]
    public Mesh[] shapes;
    public Material[] materialTypes;
    public string planetTag = "Planet";

    public Dictionary<string, Color> stringToColour;

    [Header("Positioning")]
    public float minDistanceFromCentre = 50;
    public float maxDistanceFromCentre = 950;
    public float minScale = 50;
    public float maxScale = 200;

    [Header("If random")]
    public int randomPlanetCount = 100;
    public bool forceRandomPlanetGeneration;

    List<GameObject> currentPlanets = new List<GameObject>();
    bool generationComplete = false;
    public System.Func<bool> GenerationComplete() => () =>
    {
        return generationComplete == true;
    };

    // Start is called before the first frame update
    void Awake()
    {
        stringToColour = new Dictionary<string, Color>
        {
            {"red", Color.red },
            {"blue", Color.blue },
            {"green", Color.green },
            {"yellow", Color.yellow },
            {"orange", new Color(1, 0.5f, 0) },
            {"purple", new Color(0.5f, 0, 1) },
            {"brown", new Color(0.5f, 0.25f, 0) },
            {"pink", new Color(1f, 0.5f, 1f) },
            {"cyan", Color.cyan },
            {"magenta", Color.magenta },
            {"white", Color.white },
            {"black", Color.black },
            {"grey", Color.grey },
        };

        LoadingScreen.AddCriteriaToFulfil(GenerationComplete());
        StartCoroutine(LoadPlanetsFromDatabase());
    }



    IEnumerator LoadPlanetsFromDatabase()
    {
        //LoadRandomPlanets();
        //yield break;
        
        yield return StartCoroutine(ConnectionTest.GetStatus());
        if (forceRandomPlanetGeneration || ConnectionTest.successfulOnLastCheck == false)
        {
            LoadRandomPlanets(); // Generate random planets
            yield break;
        }
        
        FirebaseApp app = FirebaseApp.DefaultInstance;//GetInstance(instanceName);
        FirebaseFirestore database = FirebaseFirestore.GetInstance(app);
        CollectionReference collection = database.Collection(databaseCollectionPath);

        System.Threading.Tasks.Task<QuerySnapshot> snapshotObtainingTask = collection.GetSnapshotAsync();
        yield return new WaitUntil(() =>
        {
            return snapshotObtainingTask.IsCompleted || snapshotObtainingTask.IsFaulted || snapshotObtainingTask.IsCanceled;
        });
        QuerySnapshot snapshot = snapshot = snapshotObtainingTask.Result;

        if (snapshot != null)
        {
            StartCoroutine(LoadPlanetsFromSnapshot(snapshot)); // Generate new planets from snapshot
        }
        else
        {
             LoadRandomPlanets(); // Generate random planets
        }
        
    }
    
    IEnumerator LoadPlanetsFromSnapshot(QuerySnapshot snapshot)
    {
        //Debug.Log("New snapshot (length: " + snapshot.Count + ")");
        for (int i = 0; i < snapshot.Count; i++)
        {
            
            string name;
            Vector3 position = RandomPosition();
            Quaternion rotation = RandomRotation();
            float size = Random.value;
            int shapeIndex = 0; // Set up index finding later, when this data is stored on the database
            int materialIndex = 0; // Set up index finding later, when this data is stored on the database
            Color retrievedColour;
            Texture2D texture;


            string colourName = snapshot[i].GetValue<string>("colour");
            //Debug.Log(colourName);
            if (stringToColour.TryGetValue(colourName, out retrievedColour) == false)
            {
                retrievedColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            }

            string imageURL = snapshot[i].GetValue<string>("img");
            UnityWebRequest getTexture = UnityWebRequestTexture.GetTexture(imageURL);
            yield return getTexture.SendWebRequest();
            texture = DownloadHandlerTexture.GetContent(getTexture);

            string ownerName = snapshot[i].GetValue<string>("name");
            name = ownerName;
            if (ownerName.EndsWith("s", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                name += "'";
            }
            else
            {
                name += "'s";
            }
            name += " planet";

            GeneratePlanet(name, position, rotation, size, shapeIndex, materialIndex, retrievedColour, texture);

            Debug.Log("Loaded planet #" + (i + 1) + " out of " + snapshot.Count + " on frame " + Time.frameCount);
        }

        generationComplete = true;
    }
    
    void LoadRandomPlanets()
    {
        Debug.Log("Obtaining snapshot failed");

        for (int i = 0; i < randomPlanetCount; i++)
        {
            string name = "Random Planet #" + (i + 1);

            Vector3 position = Random.insideUnitSphere.normalized * Random.Range(minDistanceFromCentre, maxDistanceFromCentre);
            Quaternion rotation = Quaternion.Euler(Random.Range(0f, 360), Random.Range(0f, 360), Random.Range(0f, 360));
            float scaleValue = Random.value;

            int shapeIndex = Random.Range(0, shapes.Length - 1);
            int materialIndex = Random.Range(0, materialTypes.Length - 1);
            Color planetColour = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1)/*, Random.Range(0, 255)*/);
            GeneratePlanet(name, position, rotation, scaleValue, shapeIndex, materialIndex, planetColour, null);
        }

        generationComplete = true;
    }


    Vector3 RandomPosition()
    {
        return Random.insideUnitSphere.normalized * Random.Range(minDistanceFromCentre, maxDistanceFromCentre);
    }
    Quaternion RandomRotation()
    {
        return Quaternion.Euler(Random.Range(0f, 360), Random.Range(0f, 360), Random.Range(0f, 360));
    }


    void GeneratePlanet(string name, Vector3 position, Quaternion rotation, float size, int shapeIndex, int materialIndex, Color planetColour, Texture2D texture)
    {
        GameObject newPlanet = new GameObject(name);
        newPlanet.transform.SetParent(transform);
        newPlanet.tag = planetTag;
        currentPlanets.Add(newPlanet);

        newPlanet.transform.localPosition = position;
        newPlanet.transform.localRotation = rotation;
        float scaleValue = Mathf.Lerp(minScale, maxScale, size);
        Vector3 planetScaleValues = new Vector3(scaleValue, scaleValue, scaleValue);
        newPlanet.transform.localScale = planetScaleValues;

        MeshFilter filter = newPlanet.AddComponent<MeshFilter>();
        MeshCollider collider = newPlanet.AddComponent<MeshCollider>();
        Mesh shape = shapes[shapeIndex];
        filter.mesh = shape;
        collider.sharedMesh = shape;
        collider.convex = true;

        MeshRenderer renderer = newPlanet.AddComponent<MeshRenderer>();
        renderer.material = materialTypes[materialIndex];
        renderer.material.color = planetColour;
        renderer.material.mainTexture = texture;
    }
}
