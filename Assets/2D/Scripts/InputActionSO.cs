using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputAction", menuName = "Scriptable Objects/InputAction")]
public class InputActionSO : ScriptableObject
{
	[Header("Input Action Asset")]
	public InputActionReference inputActionReference;

	[Header("Unity Actions")]
	public UnityAction<Vector2> OnVector2Input;
	public UnityAction<float> OnFloatInput;
	public UnityAction OnButtonPressed;
	public UnityAction OnButtonReleased;

	public string actionName => inputActionReference != null ? inputActionReference.action.name : string.Empty;

	private bool initialized;

	public void Initialize()
	{
		if (initialized || inputActionReference == null) return;

		InputAction action = inputActionReference.action;

		// Subscribe based on type
		switch (action.expectedControlType)
		{
			case "Vector2":
				action.performed += ctx => OnVector2Input?.Invoke(ctx.ReadValue<Vector2>());
				break;
			case "Axis":
			case "Float":
				action.performed += ctx => OnFloatInput?.Invoke(ctx.ReadValue<float>());
				break;
			case "Button":
				action.started += ctx => OnButtonPressed?.Invoke();
				action.canceled += ctx => OnButtonReleased?.Invoke();
				break;
		}

		action.Enable();
		initialized = true;
	}

	public void Deinitialize()
	{
		if (!inputActionReference) return;

		inputActionReference.action.Disable();
		initialized = false;
	}
}
