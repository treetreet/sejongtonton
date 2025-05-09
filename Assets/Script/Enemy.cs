using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] List<AudioSource> playingAudio;
        [SerializeField] Transform targetTransform;
        [SerializeField] float maxDistance;

        private Transform _playerTransform;
        private Vector3 _lastPlayerPosition;
        private GameManager _gameManager;

        private void Awake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _lastPlayerPosition = _playerTransform.position;
        }

        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            StartCoroutine(playSFX());
        }
        private void OnEnable()
        {
            PlayerCtrl.OnPlayerMove += CheckDistance;
        }

        private void OnDisable()
        {
            PlayerCtrl.OnPlayerMove -= CheckDistance;
        }

        void CheckDistance()
        {
            float distance = Vector3.Distance(transform.position, _lastPlayerPosition);

            if (distance < 1.2f)
            {
                if (transform.position == _playerTransform.position)
                {
                    playingAudio.Add(SoundManager.Instance.PlaySFX("enemygroaning", transform.position));
                    _gameManager.GameOver();
                }
                else
                {
                    transform.position = _lastPlayerPosition;
                    Debug.Log("Enemy followed previous position");
                }

                EnemyWalkingSound();
            }

            _lastPlayerPosition = _playerTransform.position; // 다음 이동을 위해 현재 위치 저장
        }

        IEnumerator playSFX()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 6.5f));
                
                playingAudio.Add(SoundManager.Instance.PlaySFX("enemygroaning", transform.position));
            }
        }
        void EnemyWalkingSound()
        {
            int temp = Random.Range(0, 3);
            string clipName = temp switch
            {
                0 => "enemywalking1",
                1 => "enemywalking2",
                2 => "enemywalking3",
            };
            playingAudio.Add(SoundManager.Instance.PlaySFX(clipName, transform.position));
        }
    }
}