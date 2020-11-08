using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    public Transform rightHand;
    public Transform leftHand;
    public Arsenal[] arsenal;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (arsenal.Length > 0)
            SetArsenal(arsenal[0].name);
    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightHand.childCount > 0)
                {
                    Destroy(rightHand.GetChild(0).gameObject);
                }

                if (leftHand.childCount > 0)
                {
                    Destroy(leftHand.GetChild(0).gameObject);
                }
                if (hand.rightWeapon != null)
                {
                    GameObject newRightWeapon = (GameObject)Instantiate(hand.rightWeapon);
                    newRightWeapon.transform.parent = rightHand;
                    newRightWeapon.transform.localPosition = Vector3.zero;
                    newRightWeapon.transform.localRotation = Quaternion.Euler(180, -90, 90);
                }
                if (hand.leftWeapon != null)
                {
                    GameObject newLeftWeapon = (GameObject)Instantiate(hand.leftWeapon);
                    newLeftWeapon.transform.parent = leftHand;
                    newLeftWeapon.transform.localPosition = Vector3.zero;
                    newLeftWeapon.transform.localRotation = Quaternion.Euler(3, 125, -143);
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightWeapon;
        public GameObject leftWeapon;
        public RuntimeAnimatorController controller;
    }
}
