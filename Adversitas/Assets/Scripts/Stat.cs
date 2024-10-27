using System;

public class Stat
{

	string m_name; {  get; set; }
	Level m_level; { get; set; }

    int firstLimitValue; { get; set; }
    int secondLimitValue;{ get; set; }
    float firstLimitRate; { get; set; }
    float secondLimitRate;{ get; set; }

    public Stat(string name, int level)
	 :m_name(name), m_level(level)
	{

	}


}
