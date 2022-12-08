using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    public static PathRequestManager _instance;
    Pathfinding _pathfinding;
    bool isProcessingPath;
    private void Awake()
    {
        _instance = this;
        _pathfinding = GetComponent<Pathfinding>();
    }
    public static void RequestPath(Vector3 pathStartPoint, Vector3 pathEndPoint, Action<Vector3[], bool> callback) {  // Unitler buradan path isteyecek ama requestleri bir kaç frame'e yayacagiz (stutter olmamasi icin). Method action delegate ile kaydedilecek ve ancak path hazir olunca calllanacak
        PathRequest newRequest = new PathRequest(pathStartPoint,pathEndPoint, callback);        
        _instance.pathRequestsQueue.Enqueue(newRequest);                                        // static methodumuz yeni bir path request olusturup bunu "pathRequestsQueue"ye ekliyor(enqueuing it)
        _instance.TryProcessNext();
    }
    void TryProcessNext()       // bir path hesapliyor muyuz onu kontrol edip eger etmiyorsak pathfinding scripte siradaki pathi hesaplatiyor
    {
        if (!isProcessingPath && pathRequestsQueue.Count > 0)
        {
            currentRequest = pathRequestsQueue.Dequeue();           // Queue'deki ilk itemi queue'den cikariyor
            isProcessingPath = true;
            _pathfinding.StartFindPath(currentRequest.pathStartPoint, currentRequest.pathEndPoint);
        }
    }
    public void FinishedProcessingPath(Vector3[] path, bool success)        // structimizin "callback" adindaki action'i icin method
    {
        //currentRequest.callback?.Invoke(path, success);
        currentRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest    {
        public Vector3 pathStartPoint;
        public Vector3 pathEndPoint;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)     // constructor for our data structure
        {
            pathStartPoint = _start;
            pathEndPoint = _end;
            callback = _callback;
        }
    }
}   
