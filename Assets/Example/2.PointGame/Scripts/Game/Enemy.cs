using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace QFramework.Example
{
    public class Enemy : MonoBehaviour, IController
    {
        private void OnMouseDown()
        {
            gameObject.SetActive(false);
            this.SendCommand<KillEnemyCommand>();
        }

        private Vector3 originalPos;
        private Vector3 direction;
        private void Start()
        {
            var rand = this.GetUtility<IRandomUtil>();
            originalPos = transform.position;
            direction = Vector3.Normalize(rand.NextSingle(-1,1) * Vector3.up +
                                          rand.NextSingle(-1,1) * Vector3.right);
        }

        private void FixedUpdate()
        {
            // transform.Rotate(transform.forward, 50.0f * Time.deltaTime, Space.World);
            transform.Rotate(0,0,50.0f * Time.deltaTime);
            transform.position = originalPos + direction * (Mathf.Sin(Time.unscaledTime* 10));
            
        }
        public IArchitecture GetArchitecture() => PointGame.Interface;
    }
}