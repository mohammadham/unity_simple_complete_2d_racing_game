using UnityEngine;
using System.Collections;

public class AssetLoader : MonoBehaviour {
    public static IEnumerator LoadRoadSegmentAsync(string path) {
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        while (!request.isDone) {
            yield return null;
        }
        Instantiate(request.asset as GameObject);
    }
}
