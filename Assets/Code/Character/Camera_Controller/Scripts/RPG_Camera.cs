using UnityEngine;
using System.Collections;

public class RPG_Camera : MonoBehaviour
{
    public Transform cameraPivot;
    public float distance = 5f;
    public float distanceMax = 6f;
    public float mouseSpeed = 8f;
    public float mouseScroll = 15f;
    public float mouseSmoothingFactor = 0.00f;
    public float camDistanceSpeed = 0.7f;
    public float camBottomDistance = 1f;
    public float firstPersonThreshold = 0.8f;
    public float characterFadeThreshold = 1.8f;

    private Vector3 desiredPosition;
    private float desiredDistance;
    private float lastDistance;
    private float mouseX = 0f;
    private float mouseXSmooth = 0f;
    private float mouseXVel;
    private float mouseY = 0f;
    private float mouseYSmooth = 0f;
    private float mouseYVel;
    private float mouseYMin = -89.5f;
    private float mouseYMax = 89.5f;
    private float distanceVel;
    private bool camBottom;
    private bool constraint;

    private float halfFieldOfView;
    private float planeAspect;
    private float halfPlaneHeight;
    private float halfPlaneWidth;

    private bool guiMode = false;

    private Player playerInstance;
    private Camera cameraInstance;
    public RPG_Controller rpgController;

    void Awake()
    {
        cameraInstance = this.gameObject.GetComponent<Camera>();
    }


    void Start()
    {
        distance = Mathf.Clamp(distance, 0.05f, distanceMax);
        desiredDistance = distance;

        halfFieldOfView = (cameraInstance.fieldOfView / 2) * Mathf.Deg2Rad;
        planeAspect = cameraInstance.aspect;
        halfPlaneHeight = cameraInstance.nearClipPlane * Mathf.Tan(halfFieldOfView);
        halfPlaneWidth = halfPlaneHeight * planeAspect;

        mouseX = 0f;
        mouseY = 15f;
    }


    public void CameraSetup()
    {
        GameObject cameraUsed;
        GameObject cameraPivot;
        RPG_Camera cameraScript;

        if (cameraInstance != null)
            cameraUsed = cameraInstance.gameObject;
        else
        {
            cameraUsed = new GameObject("Main Camera");
            cameraUsed.AddComponent<Camera>();
            cameraUsed.tag = "MainCamera";
        }

        if (!cameraUsed.GetComponent("RPG_Camera"))
            cameraUsed.AddComponent<RPG_Camera>();
        cameraScript = cameraUsed.GetComponent("RPG_Camera") as RPG_Camera;

        cameraPivot = GameObject.Find("cameraPivot") as GameObject;
        cameraScript.cameraPivot = cameraPivot.transform;
    }


    void LateUpdate()
    {
        if (cameraPivot == null)
        {
            Debug.Log("Error: No cameraPivot found! Please read the manual for further instructions.");
            return;
        }

        GetInput();

        GetDesiredPosition();

        PositionUpdate();

        CharacterFade();
    }


    void GetInput()
    {

        if (distance > 0.1)
        { // distance > 0.05 would be too close, so 0.1 is fine
            Debug.DrawLine(transform.position, transform.position - Vector3.up * camBottomDistance, Color.green);
            camBottom = Physics.Linecast(transform.position, transform.position - Vector3.up * camBottomDistance);
        }

        bool constrainMouseY = camBottom && transform.position.y - cameraPivot.transform.position.y <= 0;
        if (guiMode == false)
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSpeed;

            if (constrainMouseY)
            {
                if (Input.GetAxis("Mouse Y") < 0)
                {
                    mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;
                }
            }
            else
            {
                mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;
            }
        }

        mouseY = ClampAngle(mouseY, -89.5f, 89.5f);
        mouseXSmooth = Mathf.SmoothDamp(mouseXSmooth, mouseX, ref mouseXVel, mouseSmoothingFactor);
        mouseYSmooth = Mathf.SmoothDamp(mouseYSmooth, mouseY, ref mouseYVel, mouseSmoothingFactor);

        if (constrainMouseY)
        {
            mouseYMin = mouseY;
        }
        else
        {
            mouseYMin = -89.5f;
        }

        mouseYSmooth = ClampAngle(mouseYSmooth, mouseYMin, mouseYMax);

        if (guiMode == false)
        {
            rpgController.transform.rotation = Quaternion.Euler(rpgController.transform.eulerAngles.x, cameraInstance.transform.eulerAngles.y, rpgController.transform.eulerAngles.z);

        }
    }


    void GetDesiredPosition()
    {
        distance = desiredDistance;
        desiredPosition = GetCameraPosition(mouseYSmooth, mouseXSmooth, distance);

        float closestDistance;
        constraint = false;

        closestDistance = CheckCameraClipPlane(cameraPivot.position, desiredPosition);

        if (closestDistance != -1)
        {
            distance = closestDistance;
            desiredPosition = GetCameraPosition(mouseYSmooth, mouseXSmooth, distance);

            constraint = true;
        }


        distance -= cameraInstance.nearClipPlane;

        if (lastDistance < distance || !constraint)
            distance = Mathf.SmoothDamp(lastDistance, distance, ref distanceVel, camDistanceSpeed);

        if (distance < 0.05)
            distance = 0.05f;

        lastDistance = distance;

        desiredPosition = GetCameraPosition(mouseYSmooth, mouseXSmooth, distance); // if the camera view was blocked, then this is the new "forced" position
    }

    public void setDesiredDistance(float distance)
    {
        desiredDistance = distance;
    }


    void PositionUpdate()
    {
        transform.position = desiredPosition;

        if (distance > 0.05)
            transform.LookAt(cameraPivot);
    }


    void CharacterFade()
    {
        //if (RPG_Animation.instance == null)
        //    return;

        //if (distance < firstPersonThreshold)
        //    RPG_Animation.instance.GetComponent<Renderer>().enabled = false;

        //else if (distance < characterFadeThreshold)
        //{
        //    RPG_Animation.instance.GetComponent<Renderer>().enabled = true;

        //    float characterAlpha = 1 - (characterFadeThreshold - distance) / (characterFadeThreshold - firstPersonThreshold);
        //    if (RPG_Animation.instance.GetComponent<Renderer>().material.color.a != characterAlpha)
        //        RPG_Animation.instance.GetComponent<Renderer>().material.color = new Color(RPG_Animation.instance.GetComponent<Renderer>().material.color.r, RPG_Animation.instance.GetComponent<Renderer>().material.color.g, RPG_Animation.instance.GetComponent<Renderer>().material.color.b, characterAlpha);

        //}
        //else
        //{

        //    RPG_Animation.instance.GetComponent<Renderer>().enabled = true;

        //    if (RPG_Animation.instance.GetComponent<Renderer>().material.color.a != 1)
        //        RPG_Animation.instance.GetComponent<Renderer>().material.color = new Color(RPG_Animation.instance.GetComponent<Renderer>().material.color.r, RPG_Animation.instance.GetComponent<Renderer>().material.color.g, RPG_Animation.instance.GetComponent<Renderer>().material.color.b, 1);
        //}
    }

    public void viewMode(string mode)
    {
        switch (mode)
        {
            case "firstPerson": if (distance != 0.05f) desiredDistance = 0.05f; break;
            case "thirdPerson": if (distance != 6f) desiredDistance = 6f; break; 
        }
    }

    Vector3 GetCameraPosition(float xAxis, float yAxis, float distance)
    {
        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(xAxis, yAxis, 0);
        return cameraPivot.position + rotation * offset;
    }


    float CheckCameraClipPlane(Vector3 from, Vector3 to)
    {
        var closestDistance = -1f;

        RaycastHit hitInfo;

        ClipPlaneVertexes clipPlane = GetClipPlaneAt(to);

        Debug.DrawLine(clipPlane.UpperLeft, clipPlane.UpperRight);
        Debug.DrawLine(clipPlane.UpperRight, clipPlane.LowerRight);
        Debug.DrawLine(clipPlane.LowerRight, clipPlane.LowerLeft);
        Debug.DrawLine(clipPlane.LowerLeft, clipPlane.UpperLeft);

        Debug.DrawLine(from, to, Color.red);
        Debug.DrawLine(from - transform.right * halfPlaneWidth + transform.up * halfPlaneHeight, clipPlane.UpperLeft, Color.cyan);
        Debug.DrawLine(from + transform.right * halfPlaneWidth + transform.up * halfPlaneHeight, clipPlane.UpperRight, Color.cyan);
        Debug.DrawLine(from - transform.right * halfPlaneWidth - transform.up * halfPlaneHeight, clipPlane.LowerLeft, Color.cyan);
        Debug.DrawLine(from + transform.right * halfPlaneWidth - transform.up * halfPlaneHeight, clipPlane.LowerRight, Color.cyan);


        if (Physics.Linecast(from, to, out hitInfo) && hitInfo.collider.tag != "PlayerCapsule")
            closestDistance = hitInfo.distance - cameraInstance.nearClipPlane;

        if (Physics.Linecast(from - transform.right * halfPlaneWidth + transform.up * halfPlaneHeight, clipPlane.UpperLeft, out hitInfo) && hitInfo.collider.tag != "PlayerCapsule")
            if (hitInfo.distance < closestDistance || closestDistance == -1)
                closestDistance = Vector3.Distance(hitInfo.point + transform.right * halfPlaneWidth - transform.up * halfPlaneHeight, from);

        if (Physics.Linecast(from + transform.right * halfPlaneWidth + transform.up * halfPlaneHeight, clipPlane.UpperRight, out hitInfo) && hitInfo.collider.tag != "PlayerCapsule")
            if (hitInfo.distance < closestDistance || closestDistance == -1)
                closestDistance = Vector3.Distance(hitInfo.point - transform.right * halfPlaneWidth - transform.up * halfPlaneHeight, from);

        if (Physics.Linecast(from - transform.right * halfPlaneWidth - transform.up * halfPlaneHeight, clipPlane.LowerLeft, out hitInfo) && hitInfo.collider.tag != "PlayerCapsule")
            if (hitInfo.distance < closestDistance || closestDistance == -1)
                closestDistance = Vector3.Distance(hitInfo.point + transform.right * halfPlaneWidth + transform.up * halfPlaneHeight, from);

        if (Physics.Linecast(from + transform.right * halfPlaneWidth - transform.up * halfPlaneHeight, clipPlane.LowerRight, out hitInfo) && hitInfo.collider.tag != "PlayerCapsule")
            if (hitInfo.distance < closestDistance || closestDistance == -1)
                closestDistance = Vector3.Distance(hitInfo.point - transform.right * halfPlaneWidth + transform.up * halfPlaneHeight, from);


        return closestDistance;
    }


    float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360 || angle > 360)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }


    public struct ClipPlaneVertexes
    {
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerLeft;
        public Vector3 LowerRight;
    }


    public ClipPlaneVertexes GetClipPlaneAt(Vector3 pos)
    {
        var clipPlane = new ClipPlaneVertexes();

        if (cameraInstance == null)
            return clipPlane;

        Transform transform = cameraInstance.transform;
        float offset = cameraInstance.nearClipPlane;

        clipPlane.UpperLeft = pos - transform.right * halfPlaneWidth;
        clipPlane.UpperLeft += transform.up * halfPlaneHeight;
        clipPlane.UpperLeft += transform.forward * offset;

        clipPlane.UpperRight = pos + transform.right * halfPlaneWidth;
        clipPlane.UpperRight += transform.up * halfPlaneHeight;
        clipPlane.UpperRight += transform.forward * offset;

        clipPlane.LowerLeft = pos - transform.right * halfPlaneWidth;
        clipPlane.LowerLeft -= transform.up * halfPlaneHeight;
        clipPlane.LowerLeft += transform.forward * offset;

        clipPlane.LowerRight = pos + transform.right * halfPlaneWidth;
        clipPlane.LowerRight -= transform.up * halfPlaneHeight;
        clipPlane.LowerRight += transform.forward * offset;


        return clipPlane;
    }


    public void RotateWithCharacter()
    {
        float rotation = Input.GetAxis("Horizontal") * rpgController.turnSpeed;
        mouseX += rotation;
    }

    public void setGuiMode(bool input)
    {
        if (input == true)
        {
            guiMode = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            guiMode = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool getGuiMode()
    {
        return guiMode;
    }
}