using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummRiddle2 : MonoBehaviour
{
    public int summ1;
    public int summ2;

    /// <summary>Загадка уже решена (событие аналитики отправляется один раз).</summary>
    [System.NonSerialized] public bool solved;
}
