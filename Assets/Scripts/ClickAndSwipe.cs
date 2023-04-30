using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class ClickAndSwipe : MonoBehaviour
{
    private GameManager gameManager;
    private new Camera camera;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private new BoxCollider collider;
    private bool swiping = false;

    void Awake() {
        camera = Camera.main;
        trail = GetComponent<TrailRenderer>();
        collider = GetComponent<BoxCollider>();
        trail.enabled = false;
        collider.enabled = false;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive) {
            if (Input.GetMouseButtonDown(0)) {
                swiping = true;
                UpdateComponents();
            }
            else if (Input.GetMouseButtonUp(0)) {
                swiping = false;
                UpdateComponents();
            }
            if (swiping) {
                UpdateMousePosition();
            }
        }
    }
    void UpdateMousePosition() {
        mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }
    void UpdateComponents() {
        trail.enabled = swiping;
        collider.enabled = swiping;
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Target>()) {
            collision.gameObject.GetComponent<Target>().DestroyTarget();
        }
    }
}
