﻿//
//  GrowthbeatComponent.cs
//
//  Created by Shigeru Ogawa on 2015/06/15.
//  Copyright (c) 2015年 SIROK, Inc. All rights reserved.
//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IPHONE
using NotificationServices = UnityEngine.iOS.NotificationServices;
#endif

public class GrowthbeatComponent : MonoBehaviour
{
	#if UNITY_IPHONE
  	bool tokenSent = false;
  	#endif

  	public string applicationId = "";
  	public string credentialId = "";
  	public string senderId = "";
  	public GrowthPush.Environment environment = GrowthPush.Environment.Unknown;

	void Awake ()
	{
		Growthbeat.GetInstance ().Initialize (applicationId, credentialId);
		IntentHandler.GetInstance ().AddNoopIntentHandler ();
		IntentHandler.GetInstance ().AddUrlIntentHandler ();
		IntentHandler.GetInstance ().AddCustomIntentHandler ("GrowthbeatComponent", "HandleCustomIntent");
		GrowthLink.GetInstance().Initialize (applicationId, credentialId);
		GrowthPush.GetInstance ().RequestDeviceToken (senderId, environment);
		Growthbeat.GetInstance ().Start ();

	}   
	
	void Start ()
	{
	}
	
	void Update ()
	{
	#if UNITY_IPHONE
		if (!tokenSent) {
			byte[] token = NotificationServices.deviceToken;
			if (token != null) {
				GrowthPush.GetInstance ().SetDeviceToken(System.BitConverter.ToString(token).Replace("-", "").ToLower());
				tokenSent = true;
			}
		}
	#endif
	}

	void OnDisable ()
	{
		Growthbeat.GetInstance ().Stop ();
	}

	void HandleCustomIntent(string extra) {
		Debug.Log("Enter HandleCustomIntent");
		Debug.Log(extra);
	}

	public void ClickedRandom() {
		GrowthAnalytics.GetInstance ().SetRandom ();
	}

	public Toggle developmentToggle;

	public void ClickedDevelopment () {
		bool development = developmentToggle.isOn ? true : false;
		GrowthAnalytics.GetInstance ().SetDevelopment (development);
	}

	public InputField levelField;

	public void EndInputLevel () {
		string level = levelField.text;
		GrowthAnalytics.GetInstance ().SetLevel (int.Parse(level));
	}

	public InputField itemField;
	public InputField priceField;

	public void ClickedPurchase () {
		string item = itemField.text;
		string price = priceField.text;
		GrowthAnalytics.GetInstance ().Purchase (int.Parse(price), null, item);
	}


}
  