using UnityEngine;
using UnityEngine.UI;
// DOTween을 사용해보자
using DG.Tweening;
// 데이터 저장 JSON 파일을 위함
using System.IO;

public class ScoreManager : MonoBehaviour
{
    // 단 하나여야 한다.
    // 전역적인 접근점을 제공해야 한다.
    // 게임 개발에서는 Manager(관리자) 클래스를 보통 싱글톤 패턴으로 사용하는것이 관행이다.
    private static ScoreManager _instance = null;
    public static ScoreManager Instance => _instance;
    private void Awake()
    {
        // 인스턴스가 이미 생성(참조)된게 있다면
        // 후발주자들은 삭제해버린다
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // 목표: 적을 죽일 때마다 점수를 올리고, 현재 점수를 UI에 표시하고 싶다.

    // 응집도를 높혀라
    // 응집도: '데이터'와 '데이터를 조직하는 로직'이 얼마나 잘 모여있냐

    // 필요 속성
    // - 현재 점수 UI(Text 컴포넌트) (규칙: UI 요소는 항상 변수명 뒤에 UI 붙인다)
    // Field를 Unity가 이해할 수 있도록 Serialize (직렬화) 처리
    [SerializeField]
    private Text _currentScoreTextUI;
    [SerializeField]
    private Text _bestScoreTextUI;

    // - 점수 저장 변수
    private int _currentScore = 0;
    private int _bestScore = 0;

    public class UserData
    {
        public int BestScore = 0;
    }

    private string _jsonUserDataFilePath;

    UserData userData = new UserData();

    private void Start()
    {
        // JSON 파일 경로 설정
        _jsonUserDataFilePath = Application.dataPath + "/09.Saves/UserData.json";

        JSONBestScoreLoad();

        // 시작 시 세이브 폴더 확인 및 생성
        if (!Directory.Exists(_jsonUserDataFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_jsonUserDataFilePath));
        }
    }

    // 하나의 메서드는 한 가지 일만 잘하면 된다

    public void AddScore(int score)
    {
        if (score <= 0)
            return;

        _currentScore += score;

        Refresh();

        // 최고 점수 갱신하고 저장
        if (_currentScore > _bestScore)
        {
            JSONBestScoreSave();
        }
    }

    private void Refresh()
    {
        _currentScoreTextUI.text = $"현재 점수: {_currentScore:N0}";
        // DoTween을 사용하여 애니메이션 효과 주기
        _currentScoreTextUI.transform.DOKill(true); // 이전 애니메이션이 있으면 제거
        _currentScoreTextUI.transform.DOPunchScale(Vector3.one *2f, 0.2f, 3, 1);
    }

    private void JSONBestScoreSave()
    {
        _bestScore = _currentScore;
        userData.BestScore = _bestScore;

        // 객체를 JSON 문자열로 변환
        string json = JsonUtility.ToJson(userData);
        // 파일로 저장
        File.WriteAllText(_jsonUserDataFilePath, json);
    }

    /*
    private void SaveLoad()
    {
        // 유니티에서는 값을 저장할 때 PlayerPrefs 모듈을 사용
        // 저장 가능한 자료형: int, float, string
        // 저장을 할 때는 저장할 이름(key)과 값(value) 이 두 형태로 저장
        // 저장: Set
        // 로드: Get

        PlayerPrefs.SetInt("BestScoreKey", _currentSocre);
        _bestScore = PlayerPrefs.GetInt("BestScoreKey", 0);  // 저장값 없으면 0 반환
        string name = PlayerPrefs.GetString("name", "티모");  // name 없으면 티모 반환
    }
    */

    private void JSONBestScoreLoad()
    {
        // JSON 파일이 존재하면 로드
        if (File.Exists(_jsonUserDataFilePath))
        {
            // 파일에서 JSON 문자열 읽기
            string json = File.ReadAllText(_jsonUserDataFilePath);
            // JSON 문자열을 객체로 변환
            userData = JsonUtility.FromJson<UserData>(json);
            // 객체로 변환한 값으로 최고 점수 설정
            _bestScore = userData.BestScore;
        }
        else
        {
            // 파일이 없으면 기본값 설정
            _bestScore = 0;
        }
        BestScoreRefresh();
    }

    private void BestScoreRefresh()
    {
        _bestScoreTextUI.text = $"최고 점수: {_bestScore:N0}";
    }
}
