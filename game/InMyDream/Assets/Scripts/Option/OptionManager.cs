using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    // OptionCanvas를 할당하기 위한 변수
    [SerializeField] private GameObject optionCanvas;

    void Start()
    {
        // OptionCanvas가 할당되지 않았다면 경고 메시지 출력
        if (optionCanvas == null)
        {
            Debug.LogWarning("OptionCanvas가 OptionManager에 할당되지 않았습니다.");
        }
        else
        {
            // 시작 시 OptionCanvas를 비활성화
            optionCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // ESC 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionCanvas();
        }
    }

    // OptionCanvas의 활성화 상태를 토글하는 함수
    public void ToggleOptionCanvas()
    {
        if (optionCanvas != null)
        {
            bool isActive = optionCanvas.activeSelf;
            optionCanvas.SetActive(!isActive);

            // OptionCanvas가 활성화될 때 게임을 일시 정지하고, 비활성화될 때 다시 재개
            //Time.timeScale = isActive ? 1 : 0;
        }
    }
}
