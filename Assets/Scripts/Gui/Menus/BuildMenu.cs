using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildMenu : MonoBehaviour {

	[SerializeField] private GameObject _panel;
	[SerializeField] private GameObject _buttonOpen;
	[SerializeField] private GameObject _buttonClose;

	void Start()
	{
		Button open = _buttonOpen.GetComponent<Button> ();
		Button close = _buttonClose.GetComponent<Button> ();
		open.onClick.AddListener (OpenClick);
		close.onClick.AddListener (CloseClick);
	}

	void OpenClick()
	{
		_panel.SetActive (true);
		Debug.Log ("Open button clicked");
	}

	void CloseClick()
	{
		_panel.SetActive (false);
		Debug.Log ("Close button clicked");
	}
}
