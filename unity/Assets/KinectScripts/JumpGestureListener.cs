using UnityEngine;
using UnityEngine.UI;

public class JumpGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
    public Text GestureInfo;

    private DefenderManager DefenderManager;

    public void Awake()
    {
        DefenderManager = GameObject.FindObjectOfType<DefenderManager>();
    }

    public void UserDetected(uint userId, int userIndex)
    {
        // as an example - detect these user specific gestures
        KinectManager manager = KinectManager.Instance;

        manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
        manager.DetectGesture(userId, KinectGestures.Gestures.Squat);

        if (GestureInfo != null)
        {
            GestureInfo.text = "Jump or Squat.";
        }
    }

    public void UserLost(uint userId, int userIndex)
    {
        GestureInfo.text = "User lost";
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        string sGestureText = gesture + " detected";
        if (gesture == KinectGestures.Gestures.Jump)
        {
            DefenderManager.Jump();
        }
        GestureInfo.text = sGestureText;
        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        return true;
    }
}
