using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class UIAbstinenceBar : MonoBehaviour {

	private Player _player;
	private Image _image;
	private Color colorA = new Color(0.8f,0.8f,0);
	private Color colorB = new Color(0.8f,0,0);

	void Start()
	{
		_image = GetComponent<Image> ();
		_player = FindObjectOfType<Player> ();
	}

	void Update()
	{
		transform.localScale = new Vector3 (_player.abstinence, 1, 1);
		_image.color = Color.Lerp ( colorA, colorB, (1 + _player.abstinence) * 0.5f );
	}

}
