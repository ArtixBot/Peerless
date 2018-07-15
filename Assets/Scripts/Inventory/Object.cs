using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object {

	protected string id;
	protected string name;
	protected string desc;
	protected int cooldown;
	protected int charges;
	protected enum item_type{Weapon, Hacking, Stim, Utility};

}
