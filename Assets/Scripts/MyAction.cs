using System;
using UnityEngine;

public static class MyAction
{
    public static Action<int, Vector3, float> onLoadMagic;
    public static Action<int, Vector3, Vector3, float> onShootMagic;
    public static Func<int, Magic> onCreateMagic;
}
