using UnityEngine;
using UnityEngine.UI;

public class PlayerHPViewer : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        player.onHPValueChanged.AddListener(UpdateValue);
        UpdateValue();
    }

    private void UpdateValue()
    {
        slider.value = player.GetHPRatio();
    }
}
