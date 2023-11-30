using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool sIsCanPressKey = true;
    public const int S_FALLING_DAMAGE = 1;

    // Move Section
    [SerializeField] float MoveSpeed = 3f;
    Vector3 mDirectionToGo = new Vector3();
    public Vector3 mDestinationPosition { get; set; } =  new Vector3();
    Vector3 mOriginPostion = new Vector3();

    // Rotation Section
    [SerializeField] float SpinSpeedDeg = 270f;
    Vector3 mRotationDirection = new Vector3();
    Quaternion mDestinationRotation = new Quaternion();

    // 가짜 큐브를 먼저 돌려놓고, 돌아간 만큼의 값을 mDestinationRotation으로 대입할 것임.
    [SerializeField] Transform FakeCubeTransformToRotate = null;
    [SerializeField] Transform RealCubeTransformToRotate = null;

    // 들썩이게 만들기 위한 Section
    [SerializeField] float RecoilPosY = 1.0f;
    [SerializeField] float RecoilSpeed = 1.5f;


    bool mbIsCanMove = true;
    bool mbIsFalling = false;

    TimingManager mTimingManager;
    CameraController mCameraController;
    StatusManager mStatusManager;

    Rigidbody mRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        mTimingManager = FindObjectOfType<TimingManager>();
        Debug.Assert(mTimingManager != null);
        mCameraController = FindObjectOfType<CameraController>();
        Debug.Assert(mCameraController != null);
        mStatusManager = FindObjectOfType<StatusManager>();
        Debug.Assert(mStatusManager != null);
        mRigidBody = GetComponentInChildren<Rigidbody>();
        Debug.Assert(mRigidBody != null);

        mOriginPostion = transform.position;
    }
    public void Init()
    {
        transform.position = Vector3.zero;
        mDestinationPosition = Vector3.zero;
        RealCubeTransformToRotate.localPosition = Vector3.zero;
        mbIsCanMove = true;
        sIsCanPressKey = true;
        mbIsFalling = false;
        mRigidBody.useGravity = false;
        mRigidBody.isKinematic = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsStartGame)
        {
            return;
        }

        if (!mbIsFalling && mbIsCanMove)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1.5f) == false)
            {
                mbIsFalling = true;
                SetRigidBodySettingForFalling();
                Debug.Log("Start Falling");
            }
        }


        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D))
        {
            if (mbIsCanMove && sIsCanPressKey && !mbIsFalling)
            {
                CalculateDestPosAndRot();
                if (mTimingManager.IsHitCorrectTiming())
                {
                    MoveToDestination();
                }
            }
        }
    }

    private void CalculateDestPosAndRot()
    {
        // Cube의 X축을 위, 아래로 설정했다고 함.
        // Direction 계산.
        // x축에 왜 Vertical을 넣나면 W,S를 통해 우린 X축으로 1만큼 이동하게 만들고 싶기 때문.
        // 마찬가지로 Z축에 왜 Horizontal을 넣냐면, A,S를 통해 우리는 Z축으로 1만큼 이동하고 싶기 때문.
        mDirectionToGo.Set(Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));
        mDestinationPosition = transform.position + new Vector3(-mDirectionToGo.x, 0, mDirectionToGo.z);

        // Z축을 굴리면 W,S에 맞춰 앞 뒤로 돌려질 것임.
        // X축을 굴리면 A,S에 맞춰 옆 으로 돌려질 것임.
        mRotationDirection = new Vector3(-mDirectionToGo.z, 0f, -mDirectionToGo.x);
        // RotateAround()는 공전에 쓰이는 녀석
        // RotateAround(공전대상, 회전축, 회전값)을 이용한 편법 회전구현
        FakeCubeTransformToRotate.RotateAround(transform.position, mRotationDirection, SpinSpeedDeg);
        mDestinationRotation = FakeCubeTransformToRotate.rotation;
    }
    private void MoveToDestination()
    {
        StartCoroutine(MoveCoroutine());
        StartCoroutine(RotateCoroutine());
        StartCoroutine(RecoilCoroutine());

        // Camera
        StartCoroutine(mCameraController.ZoomCamera());
    }



    private void SetRigidBodySettingForFalling()
    {
        mRigidBody.useGravity = true;
        mRigidBody.isKinematic = false;
    }

    IEnumerator MoveCoroutine()
    {
        mbIsCanMove = false;
        while (Vector3.SqrMagnitude(transform.position - mDestinationPosition) >= 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, mDestinationPosition, MoveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = mDestinationPosition;
        mbIsCanMove = true;
    }

    IEnumerator RotateCoroutine()
    {
        while (Quaternion.Angle(RealCubeTransformToRotate.rotation, mDestinationRotation) > 0.5f)
        {
            RealCubeTransformToRotate.rotation = Quaternion.RotateTowards(RealCubeTransformToRotate.rotation, mDestinationRotation, SpinSpeedDeg * Time.deltaTime);
            yield return null;
        }
        RealCubeTransformToRotate.rotation = mDestinationRotation;
    }

    IEnumerator RecoilCoroutine()
    {
        while (RealCubeTransformToRotate.position.y < RecoilPosY)
        {
            RealCubeTransformToRotate.position += new Vector3(0f, RecoilSpeed * Time.deltaTime, 0f);
            yield return null;
        }
        while (RealCubeTransformToRotate.position.y > 0f)
        {
            RealCubeTransformToRotate.position -= new Vector3(0f, RecoilSpeed * Time.deltaTime, 0f);
            yield return null;
        }
        // 완전히 원위치 시켜줌.
        RealCubeTransformToRotate.localPosition = Vector3.zero;
    }

    public void ResetPostionAndRigidBodySetting()
    {
        mStatusManager.DecreaseHp(S_FALLING_DAMAGE);
        AudioManager.Instance.PlaySFX(AudioManager.FALLING_SFX_CLI_NAME);
        if (!mStatusManager.IsDead)
        {
            mbIsFalling = false;
            mRigidBody.useGravity = false;
            mRigidBody.isKinematic = true;
            transform.position = mOriginPostion;
            RealCubeTransformToRotate.localPosition = Vector3.zero; ;
        }
    }

}
