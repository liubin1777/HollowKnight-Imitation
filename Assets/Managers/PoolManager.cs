using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.UI.Image;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;
    Dictionary<string, Transform> poolContainer;
    Transform poolRoot;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer = new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
    }

    public T Get<T>(T original, Vector3 position, Transform parent) where T : Object    // T is Object
    {
        if(original is GameObject)  // ����ȯ(Casting) �� �����ϸ� true
        {
            GameObject prefab = original as GameObject;     // original �� GameObject�� Casting
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))      // poolDic�� ������ ���� ����
                CreatePool(key, prefab);

            GameObject obj = poolDic[key].Get();    // use ObjectPool
            obj.transform.parent = parent;
            obj.transform.position = position;
            return obj as T;                    // T�� Casting �� return
        }
        else if (original is Component)
        {
            Debug.Log("Component�� ��, Get����");
            return null;
        }
        else
        {
            Debug.Log("else �� ��, Get����");
            return null;
        }

    }

    public bool Release<T>(T instance) where T : Object     // pool�� ��ȯ
    {
        if(instance is GameObject)
        {
            GameObject gameObject = instance as GameObject;
            string key = gameObject.name;

            if(!poolDic.ContainsKey(key))       // poolDic�� �ش��ϴ� Obj�� ������ ��ȯ�Ұ� ����� �ϹǷ� false
                return false;

            poolDic[key].Release(gameObject);   // ������ ObjPool�� Release ���� �� true
            return true;
        }
        else if (instance is Component)
        {
            Debug.Log("Component�� ��, release����");
            return false;
        }
        else
        {
            Debug.Log("else�� ��, Release����");
            return false;
        }
    }

    public bool IsContain<T>(T original) where T : Object       // ObjPool�� Key���� �ִ��� Ȯ��
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else if (original is Component)
        {
            Debug.Log("Component�� ��, IsContain����");
            return false;
        }
        else
        {
            Debug.Log("else�� ��, IsContain����");
            return false;
        }
    }
    
    private void CreatePool(string key, GameObject prefab)      // Ǯ�� ����
    {
        GameObject root = new GameObject(key);
        root.gameObject.name = $"{key}Container";
        root.transform.parent = poolRoot;
        poolContainer.Add(key, root.transform);

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
                obj.transform.parent = null;
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.parent = poolContainer[key];
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );
        poolDic.Add(key, pool);
    }
}
