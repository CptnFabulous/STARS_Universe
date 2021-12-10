using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipWarp : MonoBehaviour
{
    public SpaceshipMovement ship;

    [Header("Stats")]
    public float warpRotateTime = 1;
    public float warpDelayTime = 1;
    public float warpTravelTime = 1;
    public float warpPaddingDistance = 20f;

    [Header("References")]
    public RectTransform warpMenu;
    public Dropdown locationList;
    public Button confirm;
    public Button cancel;
    public string planetCheckTag = "Planet";
    List<Collider> celestialBodies;
    IEnumerator currentWarp;

    private void Awake()
    {
        // Add listener to warp button to open warp menu
        confirm.onClick.AddListener(InitiateWarp);
        cancel.onClick.AddListener(ExitWarpMenu);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Warp"))
        {
            EnterWarpMenu();
        }
    }

    public void EnterWarpMenu()
    {
        celestialBodies = new List<Collider>(FindObjectsOfType<Collider>());
        celestialBodies.RemoveAll((body) => body.tag != planetCheckTag);

        List<Dropdown.OptionData> bodies = new List<Dropdown.OptionData>();
        for (int i = 0; i < celestialBodies.Count; i++)
        {
            Dropdown.OptionData body = new Dropdown.OptionData(celestialBodies[i].name, null);
            bodies.Add(body);
        }

        locationList.AddOptions(bodies);

        warpMenu.gameObject.SetActive(true);
        ship.manualControlDisabled = true;
    }
    public void ExitWarpMenu()
    {
        warpMenu.gameObject.SetActive(false);
        ship.manualControlDisabled = false;
    }

    public void InitiateWarp()
    {
        int index = locationList.value;
        Bounds b = celestialBodies[index].bounds;
        currentWarp = Warp(ship, b);
        StartCoroutine(currentWarp);
    }
    public IEnumerator Warp(SpaceshipMovement ship, Bounds thingToWarpTo)
    {
        ship.rb.isKinematic = true;
        ship.rb.velocity = Vector3.zero;
        ship.rb.angularVelocity = Vector3.zero;

        Quaternion oldRotation = transform.rotation;
        Quaternion lookingTowardsDestination = Quaternion.LookRotation(thingToWarpTo.center - transform.position);

        float timer = 0;
        while (timer != 1)
        {
            timer += Time.deltaTime / warpRotateTime;
            timer = Mathf.Clamp01(timer);

            //transform.rotation = Quaternion.Lerp(oldRotation, lookingTowardsDestination, timer);
            ship.rb.MoveRotation(Quaternion.Lerp(oldRotation, lookingTowardsDestination, timer));

            yield return null;
        }

        yield return new WaitForSeconds(warpDelayTime);

        Vector3 oldPosition = transform.position;
        Vector3 destinationPoint = (oldPosition - thingToWarpTo.center).normalized * (thingToWarpTo.extents.magnitude + warpPaddingDistance);

        timer = 0;
        while (timer != 1)
        {
            timer += Time.deltaTime / warpTravelTime;
            timer = Mathf.Clamp01(timer);

            //transform.position = Vector3.Lerp(oldPosition, destinationPoint, timer);
            ship.rb.MovePosition(Vector3.Lerp(oldPosition, destinationPoint, timer));

            yield return null;
        }

        ship.manualControlDisabled = false;

        ship.rb.isKinematic = false;
    }
    
}
