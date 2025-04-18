using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComplements
{
    public class PlayerDiveBehaviour
    {
        Player _myPlayer;
        PlayerModel _myModel;

        public PlayerDiveBehaviour(PlayerModel newPlayer)
        {
            _myModel = newPlayer;
        }

        public void FakeUpdate()
        {
            //if (_myPlayer.CurrenState == PlayerState.Diving)
            if (_myModel.CurrenState == PlayerState.Diving)
                CheckCollision();
        }

        void CheckCollision()
        {
            var offset = new Vector3(0, 0, 0);
            //var other = Physics.OverlapSphere(_myPlayer.transform.position + offset, 1f);
            var other = Physics.OverlapSphere(_myModel.transform.position + offset, 1f);

            foreach (var collider in other)
            {
                if (collider.TryGetComponent<IDiveable>(out var diveable))
                {

                    Debug.Log($"{diveable} dentro de area de Dive ");
                    //diveable.Dived(_myPlayer.transform);
                    diveable.Dived(_myModel.transform);
                }
            }
        }
    }
}