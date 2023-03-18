using System.Collections.Generic;
using UnityEngine;

public class FoliageBatcher : MonoBehaviour
{
    public string foliageTag = "Foliage";
    public int maxBatchSize = 100;

    void Start()
    {
        // Find all objects with the foliage tag
        GameObject[] foliageObjects = GameObject.FindGameObjectsWithTag(foliageTag);

        // Group foliage objects by material
        Dictionary<Material, List<GameObject>> foliageByMaterial = new Dictionary<Material, List<GameObject>>();
        foreach (GameObject obj in foliageObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.sharedMaterial;
                if (material != null)
                {
                    if (!foliageByMaterial.ContainsKey(material))
                    {
                        foliageByMaterial.Add(material, new List<GameObject>());
                    }
                    foliageByMaterial[material].Add(obj);
                }
            }
        }

        // Batch foliage objects by material and draw them using Graphics.DrawMeshInstanced
        foreach (KeyValuePair<Material, List<GameObject>> entry in foliageByMaterial)
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
                    GameObject foliageObj = foliageList[i + j];
                    matrices[j] = foliageObj.transform.localToWorldMatrix;
                    colors[j] = foliageObj.GetComponent<Renderer>().sharedMaterial.color;
                }

                MaterialPropertyBlock props = new MaterialPropertyBlock();
                props.SetVectorArray("_Colors", colors);
                Graphics.DrawMeshInstanced(mesh, 0, material, matrices, batchSize, props);
            }
        }
    }
}