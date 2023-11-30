using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform PlayerTransform = null;
    [SerializeField] float FollowSpeed = 15f;

    Vector3 DistanceBetweenCameraAndPlayer = new Vector3();


    [SerializeField] float ZoomOutDistance = -1.25f;
    float mHitDistance = 0f;
    // Start is called before the first frame update
    void Start()
    {
        DistanceBetweenCameraAndPlayer = transform.position - PlayerTransform.position; 

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destPostion = PlayerTransform.position + DistanceBetweenCameraAndPlayer + (transform.forward * mHitDistance);
        transform.position = Vector3.Lerp(transform.position, destPostion, FollowSpeed * Time.deltaTime);
    }
    public IEnumerator ZoomCamera()
    {
        mHitDistance = ZoomOutDistance;
        yield return new WaitForSeconds(0.15f);
        mHitDistance = 0f;
    }
}
