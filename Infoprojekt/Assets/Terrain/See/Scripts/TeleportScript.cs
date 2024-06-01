using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportScript : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject PlayerCanvas;
    public ToggleManager togglemanager;
    public MapDiscover mapdiscover;
    public GameObject Player;
    public GameObject LazyLake;
    public GameObject StartMap;
    public GameObject TundraLake;

    public ToggleGroup ToggleGroup;
    private bool enter;

    private bool showGUI;

    private Dictionary<string, (GameObject activeMap, Vector3 position)> teleportLocations;

    private void Start()
    {
        Canvas.SetActive(false);

        teleportLocations = new Dictionary<string, (GameObject, Vector3)>
        {
            { "lazylake", (LazyLake, new Vector3(-154, 15, -19)) },
            { "fisherslake", (TundraLake, new Vector3(223, 31, 201)) },
            { "village", (StartMap, new Vector3(861, 68, -147)) },
            { "tundracastle", (TundraLake, new Vector3(493, 56, 219)) },
            { "forest", (StartMap, new Vector3(423, 41, -309)) },
            { "graveyard", (StartMap, new Vector3(345, 27, -564)) },
            { "castle", (StartMap, new Vector3(156, 16, -205)) }
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && enter && Canvas != null)
        {
            OpenCanvas();
        }
    }

    private void OpenCanvas()
    {
        Canvas.SetActive(true);
        PlayerCanvas.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
        Player.SetActive(false);
    }

    private void OnGUI()
    {
        if (enter) GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 30), "Press 'F' to teleport");

        //das geht sicher schöner juckt aber
        if (showGUI)
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 300, 250, 30),
                "Du hast dieses Gebiet noch nicht erkundet");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) enter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) enter = false;
    }

    public void Teleport()
    {
        string mapName = togglemanager.GetMap();
        if (teleportLocations.TryGetValue(mapName, out var location))
        {
            ActivateLocation(location.activeMap);
            Player.transform.position = location.position;
        }
        else
        {
            StartCoroutine(Wait());
        }

        Cancel();
        enter = false;
    }

    private void ActivateLocation(GameObject activeLocation)
    {
        LazyLake.SetActive(activeLocation == LazyLake);
        StartMap.SetActive(activeLocation == StartMap);
        TundraLake.SetActive(activeLocation == TundraLake);
    }


    public void Cancel()
    {
        Canvas.SetActive(false);
        PlayerCanvas.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        Player.SetActive(true);
    }

    //ja leck eier ich weiß der code ist scuffed
    private IEnumerator Wait()
    {
        //zeigt dass man des Gebiet noch nicht unlocked hat
        showGUI = true;
        yield return new WaitForSeconds(5);
        showGUI = false;
    }
}