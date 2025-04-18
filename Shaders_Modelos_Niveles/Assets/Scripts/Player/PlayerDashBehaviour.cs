using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerComplements
{
    public class PlayerDashBehaviour
    {
        PlayerModel _myModel;

        public PlayerDashBehaviour(PlayerModel newPlayer)
        {
            _myModel = newPlayer;
        }


        public void FakeUpdate()
        {
            if (_myModel.CurrenState == PlayerState.Dashing || _myModel.CurrenState == PlayerState.Powerdashing)
                CheckCollision();
        }

        void CheckCollision()
        {

            var offset = new Vector3(0, 1, 0);
            var other = Physics.OverlapSphere(_myModel.transform.position + offset , 1f);

            foreach (var collider in other) 
            {
                if (collider.TryGetComponent<IDasheable>(out var dasheable))
                {

                    Debug.Log($"{dasheable} dentro de area de dash ");
                    dasheable.Dashed(_myModel.transform);
                }
            }

        }

    }
}
