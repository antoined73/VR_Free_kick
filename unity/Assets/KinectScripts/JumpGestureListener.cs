using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
    public Text GestureInfo;

    private DefenderManager DefenderManager;
    private List<uint> players = new List<uint>();

    public void Awake()
    {
        KinectManager manager = KinectManager.Instance;
        DefenderManager = FindObjectOfType<DefenderManager>();
    }

    public void UserDetected(uint userId, int userIndex)
    {
        KinectManager manager = KinectManager.Instance;
        players.Add(userId);
        if (players.Count > 1)
        {
            manager.Player1Avatars = new List<GameObject>(new GameObject[] { DefenderManager.defendersOffline[0], DefenderManager.defendersOffline[1] });
            manager.Player2Avatars = new List<GameObject>(new GameObject[] { DefenderManager.defendersOffline[2], DefenderManager.defendersOffline[3] });
            manager.ResetAvatarControllers();
        } else
        {
            manager.Player2Avatars = new List<GameObject>();
            manager.ResetAvatarControllers();
        }
        DefenderManager.ResetDebugTracker();
        manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
        manager.DetectGesture(userId, KinectGestures.Gestures.Squat);

        if (GestureInfo != null)
        {
            GestureInfo.text = "Jump or Squat.";
        }
    }

    public void UserLost(uint userId, int userIndex)
    {
        KinectManager manager = KinectManager.Instance;
        GestureInfo.text = "User lost";
        players.Remove(userId);
        manager.Player1Avatars = new List<GameObject>();
        manager.Player2Avatars = new List<GameObject>();
        manager.ClearKinectUsers();
        manager.ResetAvatarControllers();
        DefenderManager.ResetDebugTracker();

    }

        public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        string sGestureText = gesture + " detected";
        if (gesture == KinectGestures.Gestures.Jump)
        {
            DefenderManager.Jump(userId, players.Count);
        }
        GestureInfo.text = sGestureText;
        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        return true;
    }
}
