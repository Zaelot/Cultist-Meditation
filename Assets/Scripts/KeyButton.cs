using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Is added on buttons that want activation through keyboard commands. Disables their normal function as UI elements.
/// Listens for the keypress assigned in the Inspector and simulates the action of being clicked.
/// </summary>
/// <notes>~Z 16.01.30 | Seems a bit unnecessary though, as we could listen for key presses on the GameManager (singleton) in a single Update() loop.
/// I realize this is so that the players see something is happening.
/// </notes>
[RequireComponent(typeof(Button))]
public class KeyButton : MonoBehaviour {

	public KeyCode key;

	public Button button {get; private set;}

	Graphic targetGraphic;
	Color normalColor;

	void Awake() {
		button = GetComponent<Button>();
		button.interactable = false;
		targetGraphic = GetComponent<Graphic>();

		ColorBlock cb = button.colors;
		cb.disabledColor = cb.normalColor;
		button.colors = cb;
	} //End.Awake()

	void Start() {
		button.targetGraphic = null;
		Up();
	} //End.Start()

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(key)) {
			Down();
		} else if (Input.GetKeyUp(key)) {
			Up();
		}
	} //End.Update()

	void Up() {
		StartColorTween(button.colors.normalColor, false);
	} //End.Up()

	void Down() {
		StartColorTween(button.colors.pressedColor, false);
		button.onClick.Invoke();
	} //End.Down()

	void StartColorTween(Color targetColor, bool instant) {
		if (targetGraphic == null)
			return;

		targetGraphic.CrossFadeColor(targetColor, instant ? 0f : button.colors.fadeDuration, true, true);
	} //End.StartColorTween()

	void OnApplicationFocus(bool focus) {
		Up();
	} //End.OnApplicationFocus()

	public void LogOnClick() {
		Debug.Log ("LogOnClick() - " + GetComponentInChildren<Text>().text);
	} //End.LogOnClick()
} //End.KeyButton{}