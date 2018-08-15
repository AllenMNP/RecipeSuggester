using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBehavior : MonoBehaviour {
	
	// Use this for initialization

	public void openURL(){
		Debug.Log("Here");
		Text[] texts = this.GetComponentsInChildren<Text>();
		Debug.Log(texts[2].text);
		Debug.Log(texts[1].text);
		Debug.Log(texts[0].text);
		Application.OpenURL(texts[2].text);
	}
}
