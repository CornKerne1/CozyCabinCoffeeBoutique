
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DynamicBatcher : MonoBehaviour
{
    [SerializeField] private string foliageTag = "Foliage";
    [SerializeField] private int maxBatchSize = 1023;
    private readonly List<GameObject> _foliageObjects = new List<GameObject>(),_addedObjects=new List<GameObject>();
    private readonly Dictionary<Material, List<GameObject>> _foliageByMaterial = new Dictionary<Material, List<GameObject>>();
    private bool hasGrown = false;

    private void Awake()
    {
        // Find all objects with the foliage tag
        _foliageObjects.AddRange(GameObject.FindGameObjectsWithTag(foliageTag));
    }

    private async void Start()
    {
        BatchGameObjects(_foliageObjects);
        
    }

    public async Task AddForBatching(GameObject batchedObj)
    {
        _addedObjects.Add(batchedObj);
        foreach (var trans in batchedObj.transform.GetChildren(false))
        {
            _addedObjects.Add(trans.gameObject);
        }
        if (!await BusyListGrowthAsync(3, _addedObjects))
            BatchGameObjects(_addedObjects);

    }
    private async Task<bool> BusyListGrowthAsync(int framesToWait, List<GameObject> myList)
    {
        int framesWaited = 0;
        var count = myList.Count;
        while (framesWaited < framesToWait)
        {
            await Task.Yield(); // Wait for the next frame
            framesWaited++;
        }
        if (myList.Count >  count) return true;
        else return false;
    }


    private async Task BatchGameObjects(List<GameObject> objArray)
    {
        var objectsForBatching = objArray;
        // Group foliage objects by material
        foreach (GameObject obj in objectsForBatching)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.sharedMaterial;
                if (material != null)
                {
                    if (!_foliageByMaterial.TryGetValue(material, out List<GameObject> foliageList))
                    {
                        foliageList = new List<GameObject>();
                        _foliageByMaterial.Add(material, foliageList);
                    }

                    foliageList.Add(obj);
                }
            }
        }

        // Batch foliage objects by material and draw them using Graphics.DrawMeshInstanced
        foreach (KeyValuePair<Material, List<GameObject>> entry in _foliageByMaterial)
        {
            List<GameObject> foliageList = entry.Value;
            MeshFilter meshFilter = foliageList[0].GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogWarning("Foliage object does not have a MeshFilter component.");
                continue;
            }

            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                Debug.LogWarning("Foliage object does not have a mesh.");
                continue;
            }

            Material material = entry.Key;

            for (int i = 0; i < foliageList.Count; i += maxBatchSize)
            {
                int batchSize = Mathf.Min(maxBatchSize, foliageList.Count - i);
                Matrix4x4[] matrices = new Matrix4x4[batchSize];
                Vector4[] colors = new Vector4[batchSize];
                for (int j = 0; j < batchSize; j++)
                {
                    int index = i + j;
                    if (index < foliageList.Count)
                    {
                        GameObject foliageObj = foliageList[index];
                        matrices[j] = foliageObj.transform.localToWorldMatrix;
                        colors[j] = foliageObj.GetComponent<Renderer>().sharedMaterial.color;
                    }
                }

                MaterialPropertyBlock props = new MaterialPropertyBlock();
                props.SetVectorArray("_Colors", colors);
                Graphics.DrawMeshInstanced(mesh, 0, material, matrices, batchSize, props);
            }
        }
        objArray.Clear();
    }
}
