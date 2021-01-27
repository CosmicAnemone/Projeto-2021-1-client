using UnityEngine;

public class MyAssetLoader<T> where T : Object
{
	private readonly string assetPath;
	private T asset;
	public T Asset {
		get {
			if (asset != null) return asset;
			resourceRequest = null;
			asset = Resources.Load<T>(assetPath);
			return asset;
		}
	}
	private ResourceRequest resourceRequest;

	public MyAssetLoader(string assetPath) {
		this.assetPath = assetPath;
	}

	public static implicit operator T(MyAssetLoader<T> mal) {
		return mal.Asset;
	}

	public void tryPreload(System.Action<AsyncOperation> completed = null) {
		if (asset != null || resourceRequest != null) return;
		resourceRequest = Resources.LoadAsync<T>(assetPath);
		resourceRequest.completed += preloadComplete;
		resourceRequest.completed += completed;
	}

	private void preloadComplete(AsyncOperation asyncOperation) {
		if(asyncOperation == resourceRequest) {
			if(asset == null) {
				asset = (asyncOperation as ResourceRequest)?.asset as T;
			}
			resourceRequest = null;
		}
		else if(resourceRequest != null) {
			Debug.LogError("Weird AsyncOperation completing: " + asyncOperation);
		}
	}
}
