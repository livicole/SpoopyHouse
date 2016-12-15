using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dresser : MonoBehaviour
{

	private Animator dresserAnimator;
	Transform flashlight; 
	AudioSource soundManager;
	public AudioClip drawerOpen, drawerClose;
	Transform handCursor;

    float Timer = 0;


	// Use this for initialization
	void Start ()
	{
		soundManager = GameObject.Find ("SoundManager").GetComponent<AudioSource>();
		handCursor = this.gameObject.GetComponent<Transform> ().Find ("Quad");
		dresserAnimator = GetComponent<Animator> ();
        flashlight = GameObject.Find("Flashlight").transform;
		handCursor.GetComponent<MeshRenderer> ().enabled = false;
	}	

    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= 2)
        {
            handCursor.GetComponent<MeshRenderer>().enabled = false;
            Timer = 0f;
        }


    }

    public void CheckAndTrigger()
    {
        flashlight = GameObject.Find("Flashlight").transform;
        Ray drawerRay = new Ray(flashlight.position, flashlight.forward);
        RaycastHit rayHitInfo = new RaycastHit();
        if (Physics.Raycast(drawerRay, out rayHitInfo, 5f))
        {
            Debug.DrawRay (flashlight.position, flashlight.forward * 1000f, Color.blue);
            Debug.Log (rayHitInfo.collider.name);
            if (rayHitInfo.collider.gameObject == this.gameObject)
            {
                handCursor.GetComponent<MeshRenderer>().enabled = true;
                handCursor.transform.LookAt(flashlight);
                //	Debug.Log ("it hit");
                dresserAnimator.SetTrigger("Trigger");
                //					if (dresserAnimator.GetCurrentAnimatorStateInfo (0).IsName ("CloseDrawerIdle")) {
                //						soundManager.PlayOneShot (drawerOpen, 1.0f);
                //					}
                //					if (dresserAnimator.GetCurrentAnimatorStateInfo (0).IsName ("OpenDrawerIdle")) {
                //						soundManager.PlayOneShot (drawerClose, 1.0f);
                //					}
                //handCursor.GetComponent<MeshRenderer>().enabled = false;
                Timer = 0;
                
            }
        }
    }

	//plays drawer opening sound when DrawerOpenAnim begins
	void PlayOpenSound(){
		soundManager.PlayOneShot (drawerOpen, 1.0f);
	}

	//plays drawer closing sound when DrawerCloseAnim begins
	void PlayCloseSound(){
		soundManager.PlayOneShot (drawerClose, 1.0f);
	}
}