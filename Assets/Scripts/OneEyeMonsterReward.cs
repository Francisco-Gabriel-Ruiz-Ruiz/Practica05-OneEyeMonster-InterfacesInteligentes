using UnityEngine;

public class OneEyeMonsterReward : MonoBehaviour
{
    public PlayerPoints player;
    public int rewardForCollecting = 1;

    void Start() {
        if (!player) {
            Debug.LogError(name + ": ¡No hay script PlayerPoints asignado!");
        }
    }

    void OnPointerEnter() {
        player.AddPoints(rewardForCollecting);
    }

    void OnPointerExit() {
        Destroy(gameObject);
    }
}
