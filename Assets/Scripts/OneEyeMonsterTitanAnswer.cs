using NUnit.Framework;
using UnityEngine;

public class OneEyeMonsterTitanAnswer : MonoBehaviour
{
    public TitanOneEyeMonsterNotificator titanNotificator;
    public OneEyeMonsterTelevisionAnswer OneEyeMonsterTelevisionAnswer;
    public PlayerPoints player;
    public OneEyeMonsterReward oneEyeMonsterReward;

    private bool hasSuscribed;

    void Start() {
        hasSuscribed = false;
    }

  void Update() {
        if (!hasSuscribed && OneEyeMonsterTelevisionAnswer.GetIsObjectVisible()) {
            hasSuscribed = true;
            titanNotificator.OnGaze += AutoCollect;
        }
    }

  void AutoCollect() {
        titanNotificator.OnGaze -= AutoCollect;
        player.TakePoints(oneEyeMonsterReward.rewardForCollecting);
        Destroy(gameObject);
    }
}