using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipWarpMenu : MonoBehaviour
{
    public SpaceshipMovement ship;

    [Header("Stats")]
    public float warpRotateTime = 1;
    public float warpDelayTime = 1;
    public float warpTravelTime = 1;
    public float warpPaddingDistance = 20f;
    public string planetCheckTag = "Planet";

    [Header("HUD elements")]
    public Button enterButton;
    public Dropdown locationList;
    public Button confirm;
    public Button cancel;
    List<Collider> celestialBodies;
    IEnumerator currentWarp;
    public bool IsWarping
    {
        get
        {
            return currentWarp != null;
        }
    }

    private void Awake()
    {
        // Add listener to warp button to open warp menu
        enterButton.onClick.AddListener(Enter);
        confirm.onClick.AddListener(InitiateWarp);
        cancel.onClick.AddListener(Exit);
    }


    public void Enter()
    {
        celestialBodies = new List<Collider>(FindObjectsOfType<Collider>());
        celestialBodies.RemoveAll((body) => body.tag != planetCheckTag);

        List<Dropdown.OptionData> bodies = new List<Dropdown.OptionData>();
        for (int i = 0; i < celestialBodies.Count; i++)
        {
            Dropdown.OptionData body = new Dropdown.OptionData(celestialBodies[i].name, null);
            bodies.Add(body);
        }
        locationList.ClearOptions();
        locationList.AddOptions(bodies);
        gameObject.SetActive(true);
        ship.manualControlDisabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void Exit()
    {
        gameObject.SetActive(false);
        ship.manualControlDisabled = false;
        ship.SetControlsToComputerOrMobile();
    }

    public void InitiateWarp()
    {
        Exit();
        int index = locationList.value;
        Bounds b = celestialBodies[index].bounds;
        currentWarp = WarpSequence(ship, b);
        ship.StartCoroutine(currentWarp);
    }
    public IEnumerator WarpSequence(SpaceshipMovement ship, Bounds thingToWarpTo)
    {
        ship.manualControlDisabled = true;
        ship.rb.isKinematic = true;
        ship.c.enabled = false;
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
            ship.transform.rotation = Quaternion.Lerp(oldRotation, lookingTowardsDestination, timer);

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
            ship.transform.position = Vector3.Lerp(oldPosition, destinationPoint, timer);

            yield return null;
        }

        ship.manualControlDisabled = false;
        ship.rb.isKinematic = false;
        ship.c.enabled = true;

        EndWarp();
    }

    void EndWarp()
    {
        StopCoroutine(currentWarp);
        currentWarp = null;
    }
    
}
