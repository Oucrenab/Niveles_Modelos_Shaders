using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerComplements
{
    public class PlayerDashBehaviour
    {
        Player _myPlayer;

        public PlayerDashBehaviour(Player newPlayer)
        {
            _myPlayer = newPlayer;
        }


        public void FakeUpdate()
        {
            if (_myPlayer.CurrenState == PlayerState.Dashing || _myPlayer.CurrenState == PlayerState.Powerdashing)
                CheckCollision();
        }

        void CheckCollision()
        {

            var offset = new Vector3(0, 1, 0);
            var other = Physics.OverlapSphere(_myPlayer.transform.position + offset , 1f);

            foreach (var collider in other) 
            {
                if (collider.TryGetComponent<IDasheable>(out var dasheable))
                {

                    Debug.Log($"{dasheable} dentro de area de dash ");
                    dasheable.Dashed(_myPlayer.transform);
                }
            }

        }

    }
}
