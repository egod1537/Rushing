using System;

[Flags]
public enum EntityState
{
    Invincibility = 1 << 0,
    Dead = 1 << 1
}