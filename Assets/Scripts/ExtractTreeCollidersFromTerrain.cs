// Borrowed and refactored to work correctly from:
// https://forum.unity.com/threads/navmeshsurface-and-terrain-trees.1295496/#post-8504006

using System.Linq;
using UnityEngine;
 
[RequireComponent(typeof(Terrain))]
public class ExtractTreeCollidersFromTerrain : MonoBehaviour
{
    [ContextMenu("Extract")]
    public void Extract()
    {
        Terrain terrain = GetComponent<Terrain>();
        Transform[] transforms = terrain.GetComponentsInChildren<Transform>();
 
        //Skip the first, since its the Terrain Collider
        for (int i = 1; i < transforms.Length; i++)
        {
            //Delete all previously created colliders first
            DestroyImmediate(transforms[i].gameObject);
        }

        for (int i = 0; i < terrain.terrainData.treePrototypes.Length; i++)
        {
            TreePrototype tree = terrain.terrainData.treePrototypes[i];

            //Get all instances matching the prefab index
            TreeInstance[] instances = terrain.terrainData.treeInstances.Where(x => x.prototypeIndex == i).ToArray();

            for (int j = 0; j < instances.Length; j++)
            {
                //Un-normalize positions so they're in world-space
                instances[j].position = Vector3.Scale(instances[j].position, terrain.terrainData.size);
                instances[j].position += terrain.GetPosition();

                //Fetch the collider from the prefab object parent
                CapsuleCollider prefabCollider = tree.prefab.GetComponent<CapsuleCollider>();
                if(!prefabCollider) continue;

                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                obj.name = tree.prefab.name + j;

                if (terrain.preserveTreePrototypeLayers) obj.layer = tree.prefab.layer;
                else obj.layer = terrain.gameObject.layer;

                obj.transform.localScale = Vector3.one * prefabCollider.radius * 2;
                obj.transform.position = instances[j].position;
                obj.transform.parent = terrain.transform;
                obj.isStatic = true;
            }
        }
    }
}