using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // 목표: 적을 죽일 때마다 점수를 올리고, 현재 점수를 UI에 표시하고 싶다.

    // 응집도를 높혀라
    // 응집도: '데이터'와 '데이터를 조직하는 로직'이 얼마나 잘 모여있냐

    // 필요 속성
    // - 현재 점수 UI(Text 컴포넌트) (규칙: UI 요소는 항상 변수명 뒤에 UI 붙인다)
    // Field를 Unity가 이해할 수 있도록 Serialize (직렬화) 처리
    [SerializeField]
    private Text _currentScoreTextUI;
    // - 현재 점수를 기억할 변수
    private int _currentSocre = 0;
    public int CurrentScore => _currentSocre;



    private void Start()
    {
        Refresh();
    }

    public void AddScore(int score)
    {
        if (score <= 0)
            return;

        _currentSocre += score;
        Refresh();
    }

    private void Refresh()
    {
        _currentScoreTextUI.text = $"현재 점수: {_currentSocre}";
    }
}
