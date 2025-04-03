using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComplements
{
    public class PlayerDiveBehaviour
    {
        Player _myPlayer;

        public PlayerDiveBehaviour(Player newPlayer)
        {
            _myPlayer = newPlayer;
        }

        public void FakeUpdate()
        {
            if (_myPlayer.CurrenState == PlayerState.Diving)
                CheckCollision();
        }

        void CheckCollision()
        {
            var offset = new Vector3(0, 0, 0);
            var other = Physics.OverlapSphere(_myPlayer.transform.position + offset, 1f);

            foreach (var collider in other)
            {
                if (collider.TryGetComponent<IDiveable>(out var diveable))
                {

                    Debug.Log($"{diveable} dentro de area de Dive ");
                    diveable.Dived(_myPlayer.transform);
                }
            }
        }
    }
}