using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Music
{

    public enum GameState
    {
        normal = 0,
        death = 1,
        combat = 2,
        fighting = 3
    }
    public class MusicManager : MonoBehaviour
    {
        EnemyManager enemyMan;
        float lastTimePlayerAttacked = 5f;
        [SerializeField] AudioSource combatAudio, mainAudio, shootAudio;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(MusicChecker());
            enemyMan = FindObjectOfType<EnemyManager>();
        }

        public void ShootGun()
        {
            lastTimePlayerAttacked = 0;
        }

        IEnumerator MusicChecker()
        {
            while(true)
            {
                yield return new WaitForSeconds(2);
                if (lastTimePlayerAttacked < 5) StartCoroutine(FadeIn(shootAudio));
                else StartCoroutine(FadeOut(shootAudio));
            }
        }

        IEnumerator FadeIn(AudioSource nextMusic)

        {
            while (nextMusic.volume < 1)
            {
                yield return new WaitForSecondsRealtime(Time.deltaTime);
                nextMusic.volume = Mathf.Clamp(nextMusic.volume + Time.deltaTime, 0, 1);
            }
            nextMusic.volume = 1;
        }

        IEnumerator FadeOut(AudioSource nextMusic)

        {
            while (nextMusic.volume > 0)
            {
                yield return new WaitForSecondsRealtime(Time.deltaTime);
                nextMusic.volume = Mathf.Clamp(nextMusic.volume - Time.deltaTime, 0, 1);
            }
            nextMusic.volume = 0;
        }


        public void SetMusic(GameState status)
        {
            switch (status)
            {
                case GameState.normal:
                    break;
                case GameState.combat:
                    break;
                case GameState.fighting:
                    break;
                case GameState.death:
                    break;
            }

            // Update is called once per frame
            void Update()
            {
                lastTimePlayerAttacked+=Time.deltaTime;
            }
        }

    }
}