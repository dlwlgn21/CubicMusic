using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject GoPreFab;
    public int Count;
    public Transform PoolParentTransform;
}


public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] ObjectInfos = null;

    public static ObjectPool instance;

    public Queue<GameObject> NoteQue = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        NoteQue = InsertQue(ObjectInfos[0]);
    }

    Queue<GameObject> InsertQue(ObjectInfo info)
    {
        Queue<GameObject> tmpQue = new Queue<GameObject>();
        for (int i = 0; i < info.Count; ++i)
        {
            GameObject clone = Instantiate(info.GoPreFab, this.transform.position, Quaternion.identity);
            clone.SetActive(false);
            if (info.PoolParentTransform != null)
            {
                clone.transform.SetParent(info.PoolParentTransform);
            }
            else
            {
                clone.transform.SetParent(this.transform);
            }
            tmpQue.Enqueue(clone);
        }
        return tmpQue;
    }

}
