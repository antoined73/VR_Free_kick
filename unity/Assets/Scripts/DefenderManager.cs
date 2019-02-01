using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine.UI;

public class DefenderManager : DefenderManagerBehavior
{
    public int kinectTiltAngle = 5;
    public Image kinectPreview;
    public RectTransform TrackingMarker;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public GameObject CalibrationObject;
    public float smoothFactor = 5f;

    private float distanceToCamera = 10f;
    private int angle;

    public GameObject defender;
    private DefenderBehavior[] defenders = new DefenderBehavior[0];
    private GameObject[] defendersOffline = new GameObject[0];
    private Vector3 shoulderReference;

    private void Awake()
    {
        KinectWrapper.NuiCameraElevationGetAngle(out angle);
        Debug.Log("Current angle = " + angle);
        KinectWrapper.NuiCameraElevationSetAngle(0);
    }

    void Start()
    {
        KinectManager manager = KinectManager.Instance;
        if (CalibrationObject)
        {
            distanceToCamera = (CalibrationObject.transform.position - Camera.main.transform.position).magnitude;
        }
        if ((networkObject == null))
        {
            int quantity = 4;
            defendersOffline = new GameObject[quantity];
            for (int x = 0; x < quantity; x++)
            {
                float position = x;
                GameObject def = Object.Instantiate(defender, new Vector3(position, 0, 39), Quaternion.identity);
                defendersOffline[x] = def;
            }
        } else
        {
            GenerateDefenders();
        }
        manager.ResetAvatarControllers();
    }

    public void JumpOffline()
    {
        foreach (GameObject d in defendersOffline)
        {
            d.GetComponent<DefenderMovement>().Jump();
        }
    }

    // Make the defenders jump on the server side.
    public void Jump()
    {
        foreach (DefenderBehavior d in defenders)
        {
            d.gameObject.GetComponent<DefenderMovement>().Jump();
        }
    }

    public void GenerateDefenders()
    {
        RemoveDefenders();
        defenders = new DefenderBehavior[4];
        for (int x = 0; x < 4; x++)
        {
            float position = x;
            // Instantiate a new Defender Network Object.
            DefenderBehavior def = NetworkManager.Instance.InstantiateDefender(
                0,
                new Vector3(position, 0, 38),
                Quaternion.identity
            );
            defenders[x] = def;
        }
    }

    public void RemoveDefenders()
    {
        foreach (DefenderBehavior d in defenders)
        {
            d.networkObject.Destroy();
        }
    }

    void Update()
    {
        // if (!networkObject.IsServer)
        // {
        KinectManager manager = KinectManager.Instance;
        KinectWrapper.NuiCameraElevationGetAngle(out angle);

        if (manager && manager.IsInitialized())
        {
            if (angle < 4 && !manager.IsUserDetected())
            {
                Debug.Log("Current angle = " + angle);
                KinectWrapper.NuiCameraElevationSetAngle(kinectTiltAngle);
            }


            //backgroundImage.renderer.material.mainTexture = manager.GetUsersClrTex();
            if (kinectPreview)
            {
                Texture2D tex = manager.GetUsersClrTex();
                Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                kinectPreview.sprite = sprite;
            }

            //Vector3 vRight = BottomRight - BottomLeft;
            //Vector3 vUp = TopLeft - BottomLeft;

            int iJointIndex = (int)TrackedJoint;

            if (manager.IsUserDetected())
            {
                uint userId = manager.GetPlayer1ID();

                if (manager.IsJointTracked(userId, iJointIndex))
                {
                    Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);
                    // Debug.Log("Joint position = " + posJoint);

                    shoulderReference = posJoint;

                    if (posJoint != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);

                        // depth pos to color pos
                        Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);

                        float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;

                        //Vector3 localPos = new Vector3(scaleX * 10f - 5f, 0f, scaleY * 10f - 5f); // 5f is 1/2 of 10f - size of the plane
                        //Vector3 vPosOverlay = backgroundImage.transform.TransformPoint(localPos);
                        //Vector3 vPosOverlay = BottomLeft + ((vRight * scaleX) + (vUp * scaleY));

                        if (TrackingMarker)
                        {
                            TrackingMarker.anchoredPosition = new Vector2(posColor.x - kinectPreview.rectTransform.rect.width, -posColor.y);
                            // Debug.Log(TrackingMarker.anchoredPosition);
                        }

                        if (CalibrationObject)
                        {
                            Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                            CalibrationObject.transform.position = Vector3.Lerp(CalibrationObject.transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
                        }

                        if (networkObject == null)
                        {
                            Vector3 newPosition = defendersOffline[0].transform.position;
                            newPosition.x = posJoint.x;
                            Debug.Log("X position = " + posJoint.x);
                            foreach (GameObject d in defendersOffline)
                            {
                                newPosition.x += 1f;
                                d.transform.position = Vector3.Lerp(d.transform.position, newPosition, smoothFactor * Time.deltaTime);
                            }
                        }

                        if (networkObject != null)
                        {
                            Vector3 newPosition = defenders[0].gameObject.transform.position;
                            newPosition.x = posJoint.x;
                            newPosition.z = 38;
                            Debug.Log("X position = " + posJoint.x);
                            foreach (DefenderBehavior d in defenders)
                            {
                                newPosition.x += 1f;
                                d.gameObject.transform.position = Vector3.Lerp(d.gameObject.transform.position, newPosition, smoothFactor * Time.deltaTime);
                            }
                        }
                    }
                }

            }

        }
        // }
    }
}
