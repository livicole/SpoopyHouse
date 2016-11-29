using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SprayPaint : MonoBehaviour {

    GameObject theCamera;
    public Transform bluePaint, greenPaint, redPaint, whitePaint;
    int paintsIndex = 0;
    List<Transform> paints = new List<Transform>();
    Image sprayPaintUI;
    public Image canFillUI;
    public Sprite[] colors = new Sprite[4];
    Color[] canColors = new Color[4];

    float paintRemaining = 22;

    // Use this for initialization
    void Start () {
        sprayPaintUI = GameObject.Find("SprayCan").GetComponent<Image>();
        Debug.Log("hey");
        theCamera = GameObject.Find("ChildCamera");
        paints.Add(bluePaint);
        paints.Add(greenPaint);
        paints.Add(redPaint);
        paints.Add(whitePaint);
        canColors[0] = Color.blue;
        canColors[1] = Color.green;
        canColors[2] = Color.red;
        canColors[3] = Color.white;
    }
	
	// Update is called once per frame
	void Update () {

        canFillUI.GetComponent<RectTransform>().sizeDelta = new Vector2(12f, paintRemaining);

        if (Input.GetButtonDown("LeftBumper"))
        {
            paintsIndex = (paintsIndex + 1) % 4;
            //Debug.Log(paints[paintsIndex]);
            sprayPaintUI.sprite = colors[paintsIndex];
            canFillUI.color = canColors[paintsIndex];
        }
        if (Input.GetButton("RightBumper"))
        {
            if (paintRemaining > 0)
            {
                Ray sprayRay = new Ray(theCamera.transform.position, theCamera.transform.forward);
                RaycastHit sprayRayHit = new RaycastHit();
                if (Physics.Raycast(sprayRay, out sprayRayHit, 3f))
                {
                    if (sprayRayHit.collider.tag == "Walls")
                    {
                        Debug.Log(sprayRayHit.normal);
                        Vector3 rotationVector = Vector3.zero;
                        if (sprayRayHit.normal.x != 0)
                        {
                            rotationVector = new Vector3(0, 90f, 0);
                        }



                        Transform temp = Instantiate(paints[paintsIndex], sprayRayHit.point - theCamera.transform.forward * .01f, Quaternion.Euler(rotationVector)) as Transform;
                        temp.transform.SetParent(sprayRayHit.collider.transform.GetChild(0));

                        paintRemaining = paintRemaining - 0.1f;
                    }
                }
            }
        }
    }
}
