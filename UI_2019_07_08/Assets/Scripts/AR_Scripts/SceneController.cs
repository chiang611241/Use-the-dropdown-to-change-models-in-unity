using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public Camera firstPersonCamera;
    public GameObject Book;
    public GameObject Sound;
    public GameObject Media;
    private const float k_ModelRotation = 180.0f;
    public int caset;
    void Start(){
        QuitOnConnectionErrors ();
    }
    void QuitOnConnectionErrors(){
        if (Session.Status ==  SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
        }
        else if (Session.Status.IsError())
        {
           _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
        }
    }
    void SetSelectedPlane (DetectedPlane selectedPlane){
        Debug.Log ("Selected plane centered at " + selectedPlane.CenterPose.position);
    }
    void Update(){
        caset = PrefabController.prefab;
        // The session status must be Tracking in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking){
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began){
            return;
        }

            // Should not handle input if the player is pointing on UI.
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }
        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;
        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit)){
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(firstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else{ 
                GameObject Prefab;
                if(caset == 1 ){
                    Prefab = Book;
                    var bookObject = Instantiate(Prefab, hit.Pose.position, hit.Pose.rotation);
                    bookObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                    bookObject.transform.parent = anchor.transform;

                    SceneManager.LoadScene(1);
                }
                else if(caset == 2){
                    Prefab = Sound;
                    var bookObject = Instantiate(Prefab, hit.Pose.position, hit.Pose.rotation);
                    bookObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                    bookObject.transform.parent = anchor.transform;

                    //SceneManager.LoadScene(1);
                }
                else if(caset == 3){
                    Prefab = Media;
                    var bookObject = Instantiate(Prefab, hit.Pose.position, hit.Pose.rotation);
                    bookObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                    bookObject.transform.parent = anchor.transform;

                    //SceneManager.LoadScene(1);
                }
            }
        }
    }
    private void _ShowAndroidToastMessage(string message){
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
}
