using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator : MonoBehaviour
{


    [SerializeField] private float timeElapsed = 0f;
    [SerializeField] private float timeToReachTarget = 0.05f;
    [SerializeField] private float movementThreshold = 0.05f;

    private readonly List<TransformUpdate> futureTransformUpdates = new List<TransformUpdate>();
    private float squareMovementThreshold;
    private TransformUpdate to;
    private TransformUpdate from;
    private TransformUpdate previous;

    private void Start(){
        squareMovementThreshold = movementThreshold*movementThreshold;
        to = new TransformUpdate(NetworkManager.Singleton.ServerTick, false, transform.position);
        from = new TransformUpdate(NetworkManager.Singleton.ServerTick, false, transform.position);
        previous = new TransformUpdate(NetworkManager.Singleton.ServerTick, false, transform.position);
    }

    private void Update() {
        //List<TransformUpdate> futureTransformUpdatesProxy = futureTransformUpdates;
        
        for (int i=0;i<futureTransformUpdates.Count;i++){
            if(NetworkManager.Singleton.ServerTick >= futureTransformUpdates[i].Tick){
                if(futureTransformUpdates[i].IsTeleport){
                    to=futureTransformUpdates[i];
                    from = to;
                    previous=to;
                    transform.position = to.Position;
                }
                else{
                    previous = to;
                    to = futureTransformUpdates[i];
                    from = new TransformUpdate(NetworkManager.Singleton.InterpolationTick, false, transform.position);
                }

                //removes an element from array
                futureTransformUpdates.RemoveAt(i);
                i--;
                timeElapsed = 0f;
                timeToReachTarget = (to.Tick - from.Tick)*Time.fixedDeltaTime;
            }
        }
        //changes here to 
        timeElapsed += Time.deltaTime;

        if (timeToReachTarget == 0)
        {
            timeToReachTarget = 0.025f;
        }
        // Debug.Log(timeToReachTarget);


        InterpolatePosition(timeElapsed / timeToReachTarget);
        //InterpolatePosition(timeElapsed / 0.05f);
        //to here
    }

    private void InterpolatePosition(float lerpAmount){
        if((to.Position-previous.Position).sqrMagnitude > squareMovementThreshold){
            if(to.Position != from.Position){
                transform.position = Vector3.Lerp(from.Position,to.Position,lerpAmount);
                //Debug.Log(to.Position + "," + from.Position + "," + lerpAmount);
            }
            return ;
        }

        transform.position = Vector3.LerpUnclamped(from.Position,to.Position,lerpAmount);
    }

    public void NewUpdate(ushort tick, bool isTeleport, Vector3 position){
        if(tick <= NetworkManager.Singleton.InterpolationTick && !isTeleport){
            return ;
        }
        for(int i=0;i<futureTransformUpdates.Count;i++){
            if(tick<futureTransformUpdates[i].Tick){
                futureTransformUpdates.Insert(i, new TransformUpdate(tick, isTeleport, position));
                return ;
            }
        }

        futureTransformUpdates.Add(new TransformUpdate(tick, isTeleport, position));
    }
}
