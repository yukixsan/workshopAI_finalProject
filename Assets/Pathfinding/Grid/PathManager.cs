using System;
using UnityEngine;
using System.Collections.Generic;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;
    bool isProcessingPath;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
        if (pathfinding == null) Debug.LogError("PathRequestManager needs Pathfinding component on same GameObject.");
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd, FinishedProcessingPath);
        }
    }

    void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _cb)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _cb;
        }
    }
}
