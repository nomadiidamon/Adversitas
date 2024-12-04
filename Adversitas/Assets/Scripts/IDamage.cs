using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IDamage
{
    Rigidbody Rigidbody { get; }
    DamageValue DamageValue { get; set; }
}
