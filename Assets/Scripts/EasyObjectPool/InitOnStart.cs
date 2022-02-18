using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SG
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class InitOnStart : MonoBehaviour
    {
        public int totalCount = -1;
        LoopScrollRect rect;
        void Awake()
        {
            rect = GetComponent<LoopScrollRect>();
            rect.totalCount = totalCount;
        }
        private void OnEnable()
        {
            rect.RefillCells();
            //Debug.Log("初始关卡");
        }
    }
}