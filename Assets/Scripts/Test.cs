using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public enum IngredientUnit { Spoon, Cup, Bowl, Piece }
[Serializable]
public class Ingredient
{
    public string name;
    public int amount = 1;
    public IngredientUnit unit;
}
public class Test : MonoBehaviour
{
    public Button remote_static;
    public Button remote_non_static;
    public Button local_static;
    public Button local_non_static;
    public Button update_catelog;
    public Button downAsset;
    public Button clearCache;
    public Text text;

    public Transform remote_static_tf;
    public Transform remote_non_static_tf;
    public Transform local_static_tf;
    public Transform local_non_static_tf;
    private List<object> _updateKeys = new List<object>();
    
    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += (condition,trace,type) =>
        {
            text.text += condition + "\n";
        };
        Debug.Log("=======");
        remote_static.onClick.AddListener(()=> { LoadRes("remote_static", remote_static_tf.transform); });
        remote_non_static.onClick.AddListener(() => { LoadRes("remote_non_static", remote_non_static_tf.transform); });
        local_static.onClick.AddListener(() => { LoadRes("local_static", local_static_tf); });
        local_non_static.onClick.AddListener(() => { LoadRes("local_non_static", local_non_static_tf.transform); });
        update_catelog.onClick.AddListener(() => { UpdateCatalog(); });
        downAsset.onClick.AddListener(() => { DownLoad(); });
    }

    void LoadRes(string address,Transform parent)
    {
        var handle = Addressables.InstantiateAsync(address,parent);
        handle.Completed += (arg) =>
        {
            if (arg.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError(arg.OperationException);
            }
        };
        
    }

    public async void UpdateCatalog()
    {
        //开始连接服务器检查更新
        var handle = Addressables.CheckForCatalogUpdates(false);
        await handle.Task;
        Debug.Log("check catalog status " + handle.Status);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            List<string> catalogs = handle.Result;
            if (catalogs != null && catalogs.Count > 0)
            {
                foreach (var catalog in catalogs)
                {
                    Debug.Log("catalog  " + catalog);
                }
                Debug.Log("download catalog start ");
                var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                await updateHandle.Task;
                foreach (var item in updateHandle.Result)
                {
                    Debug.Log("catalog result " + item.LocatorId);
                    foreach (var key in item.Keys)
                    {
                        Debug.Log("catalog key " + key);
                    }
                    _updateKeys.AddRange(item.Keys);
                }
                Debug.Log("download catalog finish " + updateHandle.Status);
            }
            else
            {
                Debug.Log("dont need update catalogs");
            }
        }
        Addressables.Release(handle);
    }


    public IEnumerator DownAssetImpl()
    {
        var downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
        yield return downloadsize;
        Debug.Log("start download size :" + downloadsize.Result);

        if (downloadsize.Result > 0)
        {
            var download = Addressables.DownloadDependenciesAsync(_updateKeys, Addressables.MergeMode.Union);
            yield return download;
            //await download.Task;
            Debug.Log("download result type " + download.Result.GetType());
            foreach (var item in download.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>)
            {
                var ab = item.GetAssetBundle();
                Debug.Log("ab name " + ab.name);
                foreach (var name in ab.GetAllAssetNames())
                {
                    Debug.Log("asset name " + name);
                }
            }
            Addressables.Release(download);
        }
        Addressables.Release(downloadsize);
    }

    public void DownLoad()
    {
        StartCoroutine(DownAssetImpl());
    }
    public void CleadCache()
    {
    }
}
