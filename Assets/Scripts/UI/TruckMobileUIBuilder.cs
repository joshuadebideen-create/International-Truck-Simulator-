
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TruckMobileUIBuilder : MonoBehaviour
{
    [SerializeField] private TruckController truck;

    public void Build(TruckController truckController)
    {
        truck = truckController;

        if (truck == null)
        {
            Debug.LogError("TruckMobileUIBuilder: TruckController is missing.");
            return;
        }

        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        GameObject canvasObject = new GameObject("Truck Mobile HUD");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        CreateButton(canvas.transform, "LEFT", new Vector2(0.10f, 0.18f),
            TruckTouchButton.ControlType.SteerLeft);

        CreateButton(canvas.transform, "RIGHT", new Vector2(0.23f, 0.18f),
            TruckTouchButton.ControlType.SteerRight);

        CreateButton(canvas.transform, "BRAKE", new Vector2(0.78f, 0.18f),
            TruckTouchButton.ControlType.Brake);

        CreateButton(canvas.transform, "ACCEL", new Vector2(0.90f, 0.18f),
            TruckTouchButton.ControlType.Throttle);
    }

    private void CreateButton(
        Transform parent,
        string label,
        Vector2 anchor,
        TruckTouchButton.ControlType controlType)
    {
        GameObject buttonObject = new GameObject(label);
        buttonObject.transform.SetParent(parent, false);

        RectTransform rect = buttonObject.AddComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(150f, 110f);

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.08f, 0.10f, 0.13f, 0.78f);

        TruckTouchButton touch =
            buttonObject.AddComponent<TruckTouchButton>();

        var serializedTouch = new SerializedObject(touch);
        serializedTouch.FindProperty("truck").objectReferenceValue = truck;
        serializedTouch.FindProperty("controlType").enumValueIndex =
            (int)controlType;
        serializedTouch.ApplyModifiedProperties();

        GameObject textObject = new GameObject("Label");
        textObject.transform.SetParent(buttonObject.transform, false);

        RectTransform textRect = textObject.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObject.AddComponent<Text>();
        text.text = label;
        text.alignment = TextAnchor.MiddleCenter;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 30;
        text.color = Color.white;
    }
}
