using UnityEngine;
using UnityEngine.XR; //needs to be UnityEngine.VR in Versions before 2017.2

public class HandGrabbing : MonoBehaviour
{
    [Header("References")]
    public HandGrabbing OtherHandReference;

    [Header("Settings")]
    public string InputName;
    public LayerMask GrabLayer;
    public Vector3 ObjectGrabOffset;
    public float GrabDistance = 0.1f;
    public float ThrowMultiplier = 1.5f;

    internal Grabbable currentGrabObject;

    private Vector3 _lastFramePosition;
    private bool _isGrabbing = false;
    private bool _isInputPushed = false;
    private Transform _oldGrabbedObjectParent;
    private SphereCollider _grabCollider;
    private Grabbable nearestGrabbable;
    private Collider nearestGrabbableCollider;

    // Use this for initialization
    void Start()
    {
        _lastFramePosition = transform.position;

        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);

        currentGrabObject = null;

        _isGrabbing = false;
        _grabCollider = GetComponent<SphereCollider>();
        _grabCollider.radius = GrabDistance;

    }

    // Update is called once per frame
    void Update()
    {
        //if we don't have an active object in hand, look if there is one in proximity
        if (currentGrabObject == null)
        {
            //if there are colliders, take the first one if we press the grab button and it has the tag for grabbing
            if (nearestGrabbable != null && !_isGrabbing && !_isInputPushed && Input.GetAxis(InputName) >= 0.01f)
            {
                _isGrabbing = true;

                //Remember old parent of the object unless it is on the other hand
                if(OtherHandReference.currentGrabObject != nearestGrabbable)
                    _oldGrabbedObjectParent = nearestGrabbable.transform.parent;

                //set current object to the object we have picked up (set it as child)
                nearestGrabbable.transform.SetParent(transform);

                //if there is no rigidbody to the grabbed object attached, add one
                if (nearestGrabbable.GetComponent<Rigidbody>() == null)
                {
                    nearestGrabbable.gameObject.AddComponent<Rigidbody>();
                }

                //set grab object to kinematic (disable physics)
                nearestGrabbable.GetComponent<Rigidbody>().isKinematic = true;
                nearestGrabbable.Grab();

                //save a reference to grab object
                currentGrabObject = nearestGrabbable;

                //does other hand currently grab object? then release it!
                if (OtherHandReference.currentGrabObject != null)
                {
                    OtherHandReference.currentGrabObject = null;
                }
            }
        }
        else
        //we have object in hand, update its position with the current hand position (+defined offset from it)
        {
            //if we we release grab button, release current object
            if (Input.GetAxis(InputName) < 0.01f)
            {
                //set grab object to non-kinematic (enable physics)
                Rigidbody _objectRB = currentGrabObject.GetComponent<Rigidbody>();
                _objectRB.isKinematic = false;
                _objectRB.collisionDetectionMode = CollisionDetectionMode.Continuous;

                //calculate the hand's current velocity
                Vector3 CurrentVelocity = (transform.position - _lastFramePosition) / Time.deltaTime;

                //set the grabbed object's velocity to the current velocity of the hand
                _objectRB.velocity = CurrentVelocity * ThrowMultiplier;

                //release the the object (unparent it)
                currentGrabObject.transform.SetParent(_oldGrabbedObjectParent);
                currentGrabObject.UnSelect();

                //release reference to object
                currentGrabObject = null;
                _oldGrabbedObjectParent = null;
            }

        }

        //release grab ?
        if (Input.GetAxis(InputName) < 0.01f && _isGrabbing)
        {
            _isGrabbing = false;
        }

        //save the current position for calculation of velocity in next frame
        _lastFramePosition = transform.position;


        if (Input.GetAxis(InputName) >= 0.01f)
            _isInputPushed = true;
        else
            _isInputPushed = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            if (nearestGrabbable != null)
            {
                nearestGrabbable.UnSelect();
                nearestGrabbable = null;
            }
            nearestGrabbable = other.GetComponent<Grabbable>();
            nearestGrabbable.Select();
            nearestGrabbableCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable") && other == nearestGrabbableCollider)
        {
            //always unselect but when you are stil grabbing it with your other hand
            if(OtherHandReference.currentGrabObject != nearestGrabbable)
                nearestGrabbable.UnSelect();

            nearestGrabbable = null;
            nearestGrabbableCollider = null;
        }
    }
}
