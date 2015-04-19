using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public DrawTrail drawer;
    private CameraControl cameraControl;
    private FollowCamera followCamera;

    public Transform spawnPoint;
    public Transform cameraStartPoint;
    public GameObject player;

    public static Game instance;

    public GameObject explosionPrefab;
    public GameObject targetExplosionPrefab;

    public bool gameOver = true;

    public List<GameObject> targets;

    public List<GameObject> levels;
    public GameObject currentLevel;
    public int currentLevelIndex = 0;

    public AudioClip successSound;


    public Text statusText;
    public Text levelText;
    public Text status2Text;

    public enum Mode
    {
        Launch,
        View
    }
    public Mode mode = Mode.Launch;


    void Awake()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
        followCamera = Camera.main.GetComponent<FollowCamera>();
        instance = this;

        if (currentLevelIndex != -1)
        {
            LoadLevel(currentLevelIndex);
        }

        View();

    }

    void LoadLevel(int number)
    {
        foreach (GameObject go in levels)
        {
            go.SetActive(false);
        }

        currentLevel = levels[number];
        levels[number].SetActive(true);
        currentLevelIndex = number;
        FindTarget();
        View();
        levelText.text = "Level " + (number+1);
    }


    void FindTarget()
    {
        var target = currentLevel.transform.FindChild("target").gameObject;
        targets.Add(target);
    }


    public void ExplodePlayer()
    {
        gameOver = true;
        Explode(player);
        followCamera.enabled = false;
        cameraControl.enableMovement = true;
        drawer.enabled = false;
        bool levelDone = true;

        foreach (GameObject target in targets)
        {
            if (Vector3.Distance(target.transform.position, player.transform.position) < 4f)
            {
                Explode(target);
                AudioSource.PlayClipAtPoint(successSound, Vector3.zero, 0.3f);
            }
            if (target.activeInHierarchy)
            {
                levelDone = false;
            }
        }

        


        if (levelDone && currentLevelIndex != -1)
        {
            readyForNextLevel = true;
            statusText.text = "Level completed, press SPACE";
            status2Text.text = "Or press R to play again";


            if (currentLevelIndex == (levels.Count-1)) {
                statusText.text = "The end, thanks for playing!";
                status2Text.text = "Press R to play level again";
                readyForNextLevel = false;
            }
        }
        else
        {
            statusText.text = "Press space to try again";
            status2Text.text = "Click and drag to look around";
            readyForNextLevel = false;
        }
    }

    private bool readyForNextLevel = false;


    public void RespawnTargets()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(true);
        }
    }

    public void Explode(GameObject go)
    {
        go.SetActive(false);
        Instantiate(explosionPrefab, go.transform.position, Quaternion.identity);
    }



    void Launch()
    {
        statusText.text = "";
        status2Text.text = "";
        RespawnTargets();
        foreach (TrailSegment s in TrailSegment.allSegments) {
            if (s)
                Destroy(s.gameObject);
        }
        TrailSegment.allSegments.Clear();

        mode = Mode.Launch;
        gameOver = false;
        //Put player on spawn
        player.transform.position = spawnPoint.position;
        player.transform.rotation = Quaternion.identity;
        var rb = player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        player.SetActive(true);

        cameraControl.enableMovement = false;
        followCamera.enabled = true;
        drawer.enabled = true;
    }

    void View()
    {

        foreach (TrailSegment s in TrailSegment.allSegments)
        {
            Destroy(s.gameObject);
        }

        RespawnTargets();
        if (mode == Mode.Launch)
        {
            Camera.main.transform.position = cameraStartPoint.position;
        }

        mode = Mode.View;
        player.SetActive(false);

        cameraControl.enableMovement = true;
        followCamera.enabled = false;
        drawer.enabled = false;
        
    }

    void Edit()
    {
        cameraControl.enableMovement = false;
        drawer.enabled = true;
    }


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && readyForNextLevel)
        {
            LoadLevel(currentLevelIndex + 1);
            readyForNextLevel = false;
            statusText.text = "";
            status2Text.text = "";
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) || (gameOver && Input.GetKeyDown(KeyCode.Space) && !readyForNextLevel))
        {
            Launch();
            readyForNextLevel = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            ExplodePlayer();
        }

	}
}
