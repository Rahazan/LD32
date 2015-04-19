using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

    public bool gameOver = false;

    public List<GameObject> targets;



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
        View();

    }



    public void ExplodePlayer()
    {
        gameOver = true;
        Explode(player);
        followCamera.enabled = false;
        cameraControl.enableMovement = true;

        foreach (GameObject target in targets)
        {
            if (Vector3.Distance(target.transform.position, player.transform.position) < 5f)
            {
                Explode(target);
            }
        }
    }

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
        RespawnTargets();
        foreach (TrailSegment s in TrailSegment.allSegments) {
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


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            View();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Launch();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            ExplodePlayer();
        }

	}
}
