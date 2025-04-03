using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBounce
{
    //
    //needed to bounce
    //dir
    //fuerza
    //Duracion
    //
    //

    public void Bounce(Vector3 direction, float bounceStrg, float bounceDuration);
}
