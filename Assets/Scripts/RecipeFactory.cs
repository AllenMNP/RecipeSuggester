using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeFactory : MonoBehaviour {
	public GameObject recipePref;
	public GameObject recipeContainer;
	public Text loadMoreText;
	public Button loadMore;

	string PicParserURL = "http://192.168.1.83:80/recipesuggester/parseURL.php";
	string info_string = "";
	string[] picInfo;
	List<GameObject> recipeList;
	int posts_created = 1;

	
	// Use this for initialization
	void Start () {
		recipeList = new List<GameObject>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void create(){
		GameObject new_button = Instantiate(recipePref) as GameObject;
		new_button.transform.SetParent(recipeContainer.transform,false);
	}

	IEnumerator getRecipeInfo(List <string> urlList){
		foreach(string url in urlList){
			Debug.Log("URL BELOW");
			Debug.Log(url);
			posts_created += 1;
			WWWForm form = new WWWForm();
			form.AddField("urlPost", url);
			yield return form;

			WWW www = new WWW(PicParserURL, form);
			yield return www; 

			info_string = www.text;
			picInfo = info_string.Split('|');

			StartCoroutine(createRecipe(picInfo, url));
			yield return new WaitForSeconds(1f);
		}
		if(posts_created % 10 != 0){
			loadMoreText.text = "No More Results";
			loadMore.interactable = false;
		}
	}

	public IEnumerator createRecipe(string[] info, string url){
		GameObject new_panel = Instantiate(recipePref) as GameObject;
		new_panel.transform.SetParent(recipeContainer.transform,false);

		Text[] texts = new_panel.GetComponentsInChildren<Text>();
		texts[0].text = info[2];
		texts[1].text = info[3];
		texts[2].text = url;
		RawImage[] recipe_raw_images = new_panel.GetComponentsInChildren<RawImage>();
		WWW pic = new WWW(info[1]);
		yield return pic;
		recipe_raw_images[1].texture = pic.texture;

		recipeList.Add(new_panel);
		

	}

	public void createPosts(List <string> urlList){
		Debug.Log("CREATING POSTS");
		StartCoroutine(getRecipeInfo(urlList));
	}

	public void requestMoreLinks(){
		if(posts_created % 10 == 0){
			posts_created += 1;
			StartCoroutine(gameObject.GetComponent<GoogleInteract>().getMoreLinks(posts_created));
			posts_created -= 1;
		}
		
	}

	public void deletePosts(){
		StopAllCoroutines();
		foreach(GameObject recipe in recipeList){
			Destroy(recipe);
		}

		if(recipeList.Count > 0){
			foreach(GameObject recipe in recipeList){
				Destroy(recipe);
			}
		}
	}

	public void reset(){
		posts_created = 0;
		loadMore.interactable = true;
		loadMoreText.text = "Load More Results";
	}
}
