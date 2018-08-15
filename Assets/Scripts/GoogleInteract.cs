using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleInteract : MonoBehaviour {

	public GameObject input_panel;
	public InputField commonOne;
	public InputField commonTwo;
	public InputField keywords;
	public Button commonButtonOne;
	public Button commonButtonTwo;
	string savedQueryOne = "";
	string savedQueryTwo = "";
	string APIKey = "AIzaSyBOilDxGQ5JgYdE2JtveSUcRyHxKdcHViA";
	string query = "";

	// Use this for initialization
	void Start () {
		commonOne.text = savedQueryOne;
		commonTwo.text = savedQueryTwo;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startProcess(){
		StartCoroutine(getLinks(0));
		input_panel.SetActive(false);
	}

	public void commonSearchOne(){
		StartCoroutine(getLinks(1));
		input_panel.SetActive(false);
		gameObject.GetComponent<RecipeFactory>().reset();
	}

	public void commonSearchTwo(){
		StartCoroutine(getLinks(2));
		input_panel.SetActive(false);
		gameObject.GetComponent<RecipeFactory>().reset();
	}

	public void showInput(){
		input_panel.SetActive(true);
		gameObject.GetComponent<RecipeFactory>().reset();
	}

	public IEnumerator getLinks(int x){
		switch(x){
			case 0:
				query = keywords.text;
				break;
			case 1:
				query = commonOne.text;
				savedQueryOne = query;
				break;
			case 2:
				query = commonTwo.text;
				savedQueryTwo = query;
				break;
		}

		query = query.Replace(" ", "+");

		int rowNum = 1;
		string url = "https://www.googleapis.com/customsearch/v1?q=" + query + "&cx=014134128980261072471%3A8bnmg-ibhsw&filter=1&lr=lang_en&num=10&start=1&fields=items%2Flink&key=AIzaSyBOilDxGQ5JgYdE2JtveSUcRyHxKdcHViA";

		WWW www = new WWW(url);
		yield return www;
		Debug.Log("GRABBING LINKS");
		gameObject.GetComponent<RecipeFactory>().createPosts(ParseResponse(www.text));
/*
		for(int i = 1; i <= 30; i += 10){
			string url = "https:/www.googleapis.com/customsearch/v1?q=" + query + "&cx=014134128980261072471%3A8bnmg-ibhsw&filter=1&num=10&start=" +i.ToString()+ "&fields=items%2Flink&key=AIzaSyBOilDxGQ5JgYdE2JtveSUcRyHxKdcHViA";

			WWW www = new WWW(url);
			yield return www;
			Debug.Log(www.error);
			gameObject.GetComponent<RecipeFactory>().createPosts(ParseResponse(www.text), rowNum);

			rowNum++;
		}*/
	}

	public IEnumerator getMoreLinks(int lastSpot){

		//string url = "https:/www.googleapis.com/customsearch/v1?q=" + query + "&cx=014134128980261072471%3A8bnmg-ibhsw&filter=1&lr=lang_en&num=10&start=" + lastSpot.ToString() + "&fields=items%2Flink&key=AIzaSyBOilDxGQ5JgYdE2JtveSUcRyHxKdcHViA";
		string url = "https://www.googleapis.com/customsearch/v1?q=" + query + "&cx=014134128980261072471%3A8bnmg-ibhsw&filter=1&lr=lang_en&num=10&start=1&fields=items%2Flink&key=AIzaSyBOilDxGQ5JgYdE2JtveSUcRyHxKdcHViA";
		WWW www = new WWW(url);
		yield return www;

		gameObject.GetComponent<RecipeFactory>().createPosts(ParseResponse(www.text));
	}

	List<string> ParseResponse(string text){
		Debug.Log("PARSING RESPONSE");
		List<string> urlList = new List<string>();
		string[] urls = text.Split('\n');

		foreach(string line in urls){
			Debug.Log(line);
			if(line.Contains("link")){
				string url = line.Substring(12, line.Length - 13);
				urlList.Add(url);
			}
		}

		foreach(string l in urlList){
			Debug.Log(l);
		}

		return urlList;
	}
}
