using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectPool<T>
{
    List<T> _stock = new List<T>();
    Func<T> _Factory;
    Action<T> _On;
    Action<T> _Off;
    public ObjectPool(Func<T> Factory, Action<T> ObjOn, Action<T> ObjOff, int currentStock = 5)
    {
        _Factory = Factory;
        _On = ObjOn;
        _Off = ObjOff;

        for (int i = 0; i < currentStock; i++)
        {
            var x = _Factory();
            _Off(x);
            _stock.Add(x);
        }
    }

    public T Get()
    {
        T x;

        if (_stock.Count > 0)
        {
            x = _stock[0];
            _stock.Remove(x);
        }
        else
            x = _Factory();

        _On(x);

        return x;
    }

    public void Return(T obj)
    {
        _Off(obj);
        _stock.Add(obj);
    }
}
