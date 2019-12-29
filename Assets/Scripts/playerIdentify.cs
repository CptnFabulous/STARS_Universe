using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerIdentify : MonoBehaviour {

    Ray planetCheck;
    RaycastHit planetFound;
    float range = 100;

    public RectTransform planetInfo;
    public Text title;
    public Text description;
    public Text author;

    float screenWidth;


    // Use this for initialization
    void Start ()
    {
        title.GetComponent<Text>().enabled = (false);
        author.GetComponent<Text>().enabled = (false);
        description.GetComponent<Text>().enabled = (false);
        screenWidth = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        planetCheck.origin = transform.position;
        planetCheck.direction = transform.forward;

        if (Physics.Raycast(planetCheck, out planetFound, range))
        {
            if (planetFound.collider.tag == "Planet")
            {
                if (planetFound.collider.GetComponent<planetInfo>() != null)
                {

                }
                else
                {
                    title.text = (planetFound.collider.name);
                    print("Title changed");
                    author.text = ("Its creator is a mystery.");
                    print("Author changed");
                    description.text = ("Nobody knows anything about this planet.");
                    print("Description changed");
                }

                title.GetComponent<Text>().enabled = (true);
                print("Title enabled");
                author.GetComponent<Text>().enabled = (true);
                print("Author enabled");
                description.GetComponent<Text>().enabled = (true);
                print("Description enabled");

                screenWidth = Mathf.Lerp(screenWidth, 800, 5 * Time.deltaTime);
            }
            else
            {
                hideDisplay();
            }
        }
        else
        {
            hideDisplay();
        }

        planetInfo.sizeDelta = new Vector2(screenWidth, 1080);
    }

    void hideDisplay()
    {
        title.GetComponent<Text>().enabled = (false);
        author.GetComponent<Text>().enabled = (false);
        description.GetComponent<Text>().enabled = (false);
        screenWidth = Mathf.Lerp(screenWidth, 0, 5 * Time.deltaTime);
    }
}
