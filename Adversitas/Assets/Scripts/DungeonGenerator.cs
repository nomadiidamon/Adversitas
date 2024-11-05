using System;
using System.Collections.Generic;
using UnityEngine;

enum Shapes { CAPSULE, SPHERE, CUBE, RECTANGLE, PRISM, CYLINDER, CEILING, DOME, TUNNEL, }


public class Space
{
	Vector2 m_center;
	float m_spaceWidth;
	float m_spaceHeight;
	float m_offset;

}


public class Chunk
{
	Space m_space;
	List<Space> sections;
}

public class Room
{
	Space m_space;
	Shapes roomShape;
	List<Chunk> chunks;
	float m_roomWidth;
	float m_roomHeight;
}


public class DungeonGenerator
{
	public bool corridorFirst = true;

	public float canvasHeight;
	public float canvasWidth;


}