using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
// using UnityEditor.VersionControl;
using UnityEngine;

public class FlyBrain : Tonguable
{
    private const float Tau = 6.284f;

    private enum FlyState {
        Idle,
        Roaming,
    }
    private FlyState flyState;

    [Header("Bob Range")]
    [SerializeField][Range(0, 1)] private float amplitude = 0.01f;
    [SerializeField][Range(2, 5)] private float frequency = 2;
    [Header("Hover Range")]
    [SerializeField]private Transform hoverZone;
    [SerializeField]private float HoverRaduis;
    [SerializeField]private float approximate_player_size = 0.5f;
    [Header("State Settings")]
    [SerializeField][Range(0,2)] private float roamspeed = 1;
    [SerializeField][Range(1, 5)] private float min_idleTime = 1;
    [SerializeField][Range(1,5)] private float max_idleTime = 4;

    private Rigidbody rigidBody;
    private float timePassed;
    
    
    // Roaming Stuff
    private Vector3 targetLocationInSphere;
    Quaternion rotateTo;
    Vector3 angle;
    void Start()
    {
        ChangebugState(FlyState.Idle);
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Warpnumber(ref timePassed,0,Tau);
        timePassed += Time.deltaTime;
        BobUpAndDown();
    }
    void Warpnumber(ref float a,float min, float max)
    {
        if (a < min) { a = max; }
        if (a > max) { a = min; }


    }
    private void OnDrawGizmos()
    {

        if (hoverZone == null) { return; }
        Gizmos.DrawWireSphere(hoverZone.position,HoverRaduis);
    }

    private void BobUpAndDown()
    {
        
       // timepassed = Mathf.Clamp(timepassed, 0, 2 * Mathf.PI);
        rigidBody.position += Vector3.up * (amplitude * 0.1f * Mathf.Sin(timePassed*frequency));
        if (flyState == FlyState.Roaming) {
            RoamUpdate();
        }
    }

    private void ChangebugState(FlyState flystate)
    {
        this.flyState = flystate;
        switch (flystate)
        {
            case FlyState.Idle:
                Idle();
                break;
            case FlyState.Roaming:
                break;
        }
    }

    private async void Idle()
    {
        await Awaitable.WaitForSecondsAsync(Random.Range(min_idleTime,max_idleTime));
        //Debug.Log("ss");

        GetRoamLocation();
        ChangebugState(FlyState.Roaming);
    }

    private void RoamUpdate()
    {
        rigidBody.velocity = (targetLocationInSphere - transform.position) * roamspeed;
        rotateTo = Quaternion.LookRotation(targetLocationInSphere - transform.position);
        angle = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(angle.x, rotateTo.eulerAngles.y, angle.z);
        if ((targetLocationInSphere - transform.position).magnitude <= 0.1f) { 
            ChangebugState(FlyState.Idle);
            return;
        }
    }

    private void GetRoamLocation()
    {
        if (hoverZone == null) { return; }
        targetLocationInSphere.x = Random.Range(0, HoverRaduis);
        targetLocationInSphere.y = Random.Range(0, HoverRaduis - targetLocationInSphere.x);
        targetLocationInSphere.z = Random.Range(0, HoverRaduis - targetLocationInSphere.y);
        targetLocationInSphere += hoverZone.position - (Vector3.one * approximate_player_size);
    }

    public override void GotRetrieved()
    {
        GameManager.Instance.FlyCount++;
        base.GotRetrieved();
    }
}
