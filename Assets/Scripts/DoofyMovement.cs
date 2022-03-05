using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoofyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 1;
    [Range(0.1f,1)]
    public float Acceleration = 0.5f;
    [Range(1f,2)]
    public float Stride = 1f;
    [Range(1f, 2f)]
    public float FootLag = 1.5f;
    [Range(0.1f, 2)]
    public float StepTime = 1f;
    [Range(0.1f, 2)]
    public float HeadMovement = 1f;
    [Range(0.01f, 1)]
    public float HeadMovementSpeed = 0.1f;

    [Header("Transforms")] 
    public GameObject LeftFootTarget;
    public GameObject RightFootTarget;
    public GameObject HeadTarget;

    private Target _leftFoot;
    private Target _rightFoot;
    private bool _footIsMoving = false;

    private Animator _animator;
    private Rigidbody _physics;



    void Start()
    {
        _animator = GetComponent<Animator>();
        _physics = GetComponent<Rigidbody>();
        _leftFoot = new Target()
        {
            Obj = LeftFootTarget,
            CurrentPos = LeftFootTarget.transform.position,
            TargetPos = LeftFootTarget.transform.position,
            Offset = Vector3.Scale(LeftFootTarget.transform.position - transform.position, new Vector3(1, 0, 0)),
        };
        _rightFoot = new Target()
        {
            Obj = RightFootTarget,
            CurrentPos = RightFootTarget.transform.position,
            TargetPos = RightFootTarget.transform.position,
            Offset = Vector3.Scale(RightFootTarget.transform.position - transform.position, new Vector3(1, 0, 0)),
        };
    }

    void FixedUpdate()
    {
        var forward = Flatten(Camera.main.transform.forward).normalized;
        var right = Flatten(Camera.main.transform.right).normalized;

        var movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var movementDirection = (forward * movementInput.y + right * movementInput.x).normalized;
        var movementVector = movementDirection * Speed;

        _physics.velocity = Vector3.Lerp(_physics.velocity, movementVector, Acceleration);
        var strideLength = Stride * (_physics.velocity.magnitude / Speed);
        var stride = movementDirection * strideLength;
        var centerOfGravity = transform.position;

        if (!_footIsMoving && _leftFoot.UpdateTargetPos(centerOfGravity, stride) && _rightFoot.UpdateTargetPos(centerOfGravity, stride))
        {
            var currentCenter = Center(_leftFoot.CurrentPos, _rightFoot.CurrentPos);
            var targetCenter = Center(_leftFoot.TargetPos, _rightFoot.TargetPos);
            if (_leftFoot.Distance > strideLength * FootLag)
            {
                StartCoroutine(MoveFoot(_leftFoot));
            }
            else if (_rightFoot.Distance > strideLength * FootLag)
            {
                StartCoroutine(MoveFoot(_rightFoot));
            }
        }
        else
        {
            //Ragdoll
        }

        _leftFoot.UpdateTarget();
        _rightFoot.UpdateTarget();
        var headOffset = Vector3.down * 0.5f * strideLength + transform.InverseTransformDirection(movementDirection) * strideLength;
        headOffset *= HeadMovement;
        headOffset *= (_physics.velocity.magnitude / Speed);
        HeadTarget.transform.localPosition = Vector3.Lerp(HeadTarget.transform.localPosition, headOffset, HeadMovementSpeed);
    }

    IEnumerator MoveFoot(Target foot)
    {
        var time = 0f;
        var start = foot.CurrentPos;
        _footIsMoving = true;
        while (time < StepTime)
        {
            var t = time / StepTime;
            foot.CurrentPos = Vector3.Lerp(start, foot.TargetPos, t);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        foot.CurrentPos = foot.TargetPos;
        _footIsMoving = false;
    }

    Vector3 Center(Vector3 p1, Vector3 p2) => p1 + (p1 - p2) * 0.5f;
    Vector3 Flatten(Vector3 v) => Vector3.Scale(v, new Vector3(1, 0, 1));

    class Target
    {
        public GameObject Obj;
        public Vector3 CurrentPos;
        public Vector3 TargetPos;
        public Vector3 Offset;
        public float Distance => Vector3.Distance(CurrentPos, TargetPos);

        public void UpdateTarget() => Obj.transform.position = CurrentPos;
        public bool UpdateTargetPos(Vector3 centerOfGravity, Vector3 stride)
        {
            if (!Physics.Raycast(new Ray(centerOfGravity + Offset + stride, Vector3.down), out var hit, 1.5f))
                return false;

            TargetPos = hit.point;
            return true;
        }
    }
}
