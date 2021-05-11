using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace offlineplay
{
    public class LudoDiceRollController : MonoBehaviour
    {

        public void Finish_RollDice()
        {
            print("A");
            gameObject.SetActive(false);
            transform.parent.GetComponent<LudoDiceController>().Dice_RandomValue();
        }

    }
}
