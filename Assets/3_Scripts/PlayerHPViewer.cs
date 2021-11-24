using UnityEngine;
using UnityEngine.UI;

public class PlayerHPViewer : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.onHPValueChanged.AddListener(UpdateValue);
        UpdateValue();
    }

    private void UpdateValue()
    {
        image.fillAmount = player.GetHPRatio();
        text.text = $"¢¾ {player.Status.HP}/{player.Status.MaxHP}";
    }
}
