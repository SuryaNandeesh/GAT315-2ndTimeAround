using UnityEngine;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
	[SerializeField] InputActionSO inputEvent;

	[Header("Unity Events")]
	public UnityEvent<Vector2> OnVector2Input;
	public UnityEvent<float> OnFloatInput;
	public UnityEvent OnButtonPressed;
	public UnityEvent OnButtonReleased;

	public InputActionSO InputEvent => inputEvent;

	private void OnEnable()
	{
		if (inputEvent != null) inputEvent.Initialize();

		OnVector2Input.AddListener((value) => inputEvent.OnVector2Input?.Invoke(value));
		OnFloatInput.AddListener((value) => inputEvent.OnFloatInput?.Invoke(value));
		OnButtonPressed.AddListener(() => inputEvent.OnButtonPressed?.Invoke());
		OnButtonReleased.AddListener(() => inputEvent.OnButtonReleased?.Invoke());
	}

	private void OnDisable()
	{
		if (inputEvent != null)	inputEvent.Deinitialize();
	}
}

