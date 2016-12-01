using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SprayPaint : MonoBehaviour {

    GameObject theCamera;
    public Transform bluePaint, greenPaint, redPaint, whitePaint;
    int paintsIndex = 0;
    List<Transform> paints = new List<Transform>();
    Image sprayPaintUI;
    public Image canFillUI;
    public Sprite[] colors = new Sprite[4];
    Color[] canColors = new Color[4];

    double dspTime;
    public AudioSource sprayLoopSource, sprayAttackSource, sprayReleaseSource, sprayEmptySource, sprayEmptyFadeSource;
    public AudioClip sprayLoopClip, sprayAttackClip, sprayReleaseClip, sprayEmptyClip;
    bool spraying = false;
    bool emptySpraying = false;

    public float paintRemaining = 22;

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
            paintRemaining =  paintRemaining - 0.1f; 
            if (paintRemaining < 0)
            {
                paintRemaining = 0;
            }
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


                    }
                }
            }
        }

        if (Input.GetButtonDown("RightBumper"))
        {
            if (paintRemaining > 0)
            {
                dspTime = AudioSettings.dspTime;

                sprayAttackSource.PlayScheduled(dspTime);

                sprayLoopSource.PlayScheduled(dspTime + sprayAttackClip.length);
                spraying = true;
            }
            else
            {
                sprayEmptySource.Play();
                emptySpraying = true;
            }

            
        }
        if (Input.GetButtonUp("RightBumper"))
        {
            if (spraying)
            {
                sprayLoopSource.Stop();
                sprayReleaseSource.Play();
                spraying = false;
            }

            if (emptySpraying)
            {
                sprayEmptySource.Stop();
                sprayEmptyFadeSource.Play();
                emptySpraying = false;
            }
            
        }
        if (paintRemaining == 0)
        {
            
            if (spraying && !emptySpraying)
            {
                sprayLoopSource.Stop();
                spraying = false;
                sprayEmptySource.Play();
                emptySpraying = true;
            }
        }
        

    }
}
