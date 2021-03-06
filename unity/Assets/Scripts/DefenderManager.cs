﻿using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine.UI;
using System.Collections.Generic;

public class DefenderManager : DefenderManagerBehavior
{
    public int kinectTiltAngle = 5;
    public bool enablePreview = false;
    public Image kinectPreview;
    public RectTransform TrackingMarker;
    public RectTransform TrackingMarker2;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public float smoothFactor = 5f;
    public int number = 4;

    // Interval to run the code every x frame
    public int interval = 3;
    
    private bool isNetwork;

    public GameObject defender;
    public DefenderBehavior[] defenders = new DefenderBehavior[0];
    public GameObject[] defendersOffline = new GameObject[0];

    private void Awake()
    {
        isNetwork = (networkObject != null);
        if (!enablePreview)
        {
            kinectPreview.gameObject.SetActive(false);
            TrackingMarker.gameObject.SetActive(false);
            TrackingMarker2.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        KinectManager Manager = KinectManager.Instance;
        if (!isNetwork)
        {
            // Generate defenders in offline mode.
            defendersOffline = new GameObject[number];
            for (int x = 0; x < number; x++)
            {
                GameObject def = Instantiate(defender, new Vector3(
                    transform.position.x + 0.5f - number / 2,
                    0,
                    transform.position.z - 4), 
                    Quaternion.Euler(0, 180, 0));
                defendersOffline[x] = def;
            }
        } else if (networkObject.IsServer)
        {
            GenerateDefenders();
        }
        if (Manager && Manager.IsInitialized())
            Manager.ResetAvatarControllers();
    }

    public void GenerateDefenders()
    {
        RemoveDefenders();
        defenders = new DefenderBehavior[4];
        for (int x = 0; x < number; x++)
        {
            // Instantiate a new Defender Network Object.
            DefenderBehavior def = NetworkManager.Instance.InstantiateDefender(
                0, 
                new Vector3(
                    transform.position.x + 0.5f - number / 2,
                    0,
                    transform.position.z - 4),
                Quaternion.Euler(0, 180, 0)
            );
            defenders[x] = def;
        }
    }

    // Make the defenders jump on the server side.
    public void Jump(uint userId, int usersCount)
    {
        KinectManager Manager = KinectManager.Instance;
        uint user1Id = Manager.GetPlayer1ID();
        uint user2Id = Manager.GetPlayer2ID();
        if (usersCount == 2)
        {
            if (userId == user1Id)
            {
                if (networkObject == null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        defendersOffline[i].GetComponent<DefenderMovement>().Jump();
                    }

                } else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        defenders[i].gameObject.GetComponent<DefenderMovement>().Jump();
                    }
                }
            } else
            {
                if (networkObject == null)
                {
                    for (int i = 2; i < 4; i++)
                    {
                        defendersOffline[i].GetComponent<DefenderMovement>().Jump();
                    }
                }
                else
                {
                    for (int i = 2; i < 4; i++)
                    {
                        defenders[i].gameObject.GetComponent<DefenderMovement>().Jump();
                    }
                }
            }
        } else
        {
            foreach (DefenderBehavior d in defenders)
            {
                d.gameObject.GetComponent<DefenderMovement>().Jump();
            }
            foreach (GameObject d in defendersOffline)
            {
                d.GetComponent<DefenderMovement>().Jump();
            }
        }
    }

    public void RemoveDefenders()
    {
        foreach (DefenderBehavior d in defenders)
        {
            d.networkObject.Destroy();
        }
    }

    public void ResetDebugTracker()
    {
        if (enablePreview && TrackingMarker)
        {
            TrackingMarker.anchoredPosition = new Vector2(0, 0);
        }
        if (enablePreview && TrackingMarker2)
        {
            TrackingMarker2.anchoredPosition = new Vector2(0, 0);
        }
    }

    void Update()
    {
        KinectManager Manager = KinectManager.Instance;
        if (Manager && Manager.IsInitialized() && (Time.frameCount % interval == 0))
        {
            if (enablePreview && kinectPreview)
            {
                Texture2D tex = Manager.GetUsersClrTex();
                Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                kinectPreview.sprite = sprite;
            }

            int iJointIndex = (int)TrackedJoint;
            if (Manager.IsUserDetected())
            {
                uint user1Id = Manager.GetPlayer1ID();
                uint user2Id = Manager.GetPlayer2ID();

                // Tracking user 1
                if (Manager.IsJointTracked(user1Id, iJointIndex))
                {
                    Vector3 posJoint = Manager.GetRawSkeletonJointPos(user1Id, iJointIndex);

                    if (posJoint != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = Manager.GetDepthMapPosForJointPos(posJoint);

                        // depth pos to color pos
                        Vector2 posColor = Manager.GetColorMapPosForDepthPos(posDepth);

                        //float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        //float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;

                        if (TrackingMarker)
                        {
                            TrackingMarker.anchoredPosition = new Vector2(posColor.x - kinectPreview.rectTransform.rect.width, -posColor.y);
                        }

                        if (!isNetwork)
                        {
                            for (int i = 0; i < (Manager.IsJointTracked(user2Id, iJointIndex) ? 2 : 4); i++)
                            {
                                GameObject d = defendersOffline[i];
                                float newX = transform.position.x - posJoint.x + i + 0.5f - number / 2;
                                d.transform.position = Vector3.Lerp(d.transform.position, new Vector3(newX, d.transform.position.y, d.transform.position.z), smoothFactor * Time.deltaTime);
                            }
                        } else {
                            for (int i = 0; i < (Manager.IsJointTracked(user2Id, iJointIndex) ? 2 : 4); i++)
                            {
                                DefenderBehavior d = defenders[i];
                                float newX = transform.position.x - posJoint.x + i + 0.5f - number / 2;
                                d.gameObject.transform.position = Vector3.Lerp(
                                    d.gameObject.transform.position,
                                    new Vector3(newX,
                                                d.gameObject.transform.position.y,
                                                d.gameObject.transform.position.z),
                                    smoothFactor * Time.deltaTime);
                            }
                        }
                    }
                }
                // Tracking user 2
                if (Manager.IsJointTracked(user2Id, iJointIndex))
                {
                    Vector3 posJoint = Manager.GetRawSkeletonJointPos(user2Id, iJointIndex);
                    if (posJoint != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = Manager.GetDepthMapPosForJointPos(posJoint);

                        // depth pos to color pos
                        Vector2 posColor = Manager.GetColorMapPosForDepthPos(posDepth);

                        if (TrackingMarker2)
                        {
                            TrackingMarker2.anchoredPosition = new Vector2(posColor.x - kinectPreview.rectTransform.rect.width, -posColor.y);
                        }

                        if (!isNetwork)
                        {
                            for (int i = 2; i < 4; i++)
                            {
                                GameObject d = defendersOffline[i];
                                Vector3 newPosition = defendersOffline[i].transform.position;
                                newPosition.x = transform.position.x - posJoint.x;
                                newPosition.x += i - number / 2;
                                d.transform.position = Vector3.Lerp(d.transform.position, newPosition, smoothFactor * Time.deltaTime);
                            }
                        } else {
                            for (int i = 2; i < 4; i++)
                            {
                                DefenderBehavior d = defenders[i];
                                float newX = transform.position.x - posJoint.x + i - number / 2;
                                d.gameObject.transform.position = Vector3.Lerp(
                                    d.gameObject.transform.position,
                                    new Vector3(newX,
                                                d.gameObject.transform.position.y,
                                                d.gameObject.transform.position.z),
                                    smoothFactor * Time.deltaTime);
                            }
                        }
                    }
                }
            }
        }
    }
}
