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
                    //rightHand.GetChild(0).gameObject.GetComponent<Collider>().enabled = false; //:(
                    Destroy(rightHand.GetChild(0).gameObject);
                    //rightHand.GetChild(0).gameObject.SetActive(false);
                }

                if (leftHand.childCount > 0)
                {
                    //leftHand.GetChild(0).gameObject.GetComponent<Collider>().enabled = false; //:(
                    Destroy(leftHand.GetChild(0).gameObject);
                    //leftHand.GetChild(0).gameObject.SetActive(false);
                }
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightHand;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(180, -90, 90);
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftHand;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(3, 125, -143);
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
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}
