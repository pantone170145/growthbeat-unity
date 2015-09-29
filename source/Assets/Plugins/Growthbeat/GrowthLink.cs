using UnityEngine;
using System.Collections;

public class GrowthLink {

	private static GrowthLink instance = new GrowthLink ();

	
	#if UNITY_IPHONE
	[DllImport("__Internal")] static extern void growthLinkInitializeWithApplicationId(string applicationId, string credentialId);
	[DllImport("__Internal")] static extern void growthLinkHandleOpenUrl(string url);
	#endif
	
	public static GrowthLink GetInstance ()
	{
		return GrowthLink.instance;
	}
	
	#if UNITY_ANDROID
	private static AndroidJavaObject growthLink;
	#endif
	
	private GrowthLink() {
		#if UNITY_ANDROID
		using(AndroidJavaClass gbcclass = new AndroidJavaClass( "com.growthbeat.link.GrowthLink" )) {
			growthLink = gbcclass.CallStatic<AndroidJavaObject>("getInstance"); 
		}
		#endif
	}
	
	public void Initialize (string applicationId, string credentialId)
	{
		#if UNITY_ANDROID
		if (growthLink == null)
			return;
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"); 
		growthLink.Call("initialize", activity, applicationId, credentialId);
		#elif UNITY_IPHONE && !UNITY_EDITOR
		growthLinkInitializeWithApplicationId(applicationId, credentialId);
		#endif
	}
	
	
	public void HandleOpenUrl ()
	{
		#if UNITY_ANDROID
		AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent");
		AndroidJavaObject uri = intent.Call<AndroidJavaObject>("getData");
		growthLink.Call("handleOpenUrl", uri);
		#elif UNITY_IPHONE && !UNITY_EDITOR
		#endif
	}

}
