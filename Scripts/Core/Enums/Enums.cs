namespace LordAmbermaze.Core
{
    public enum Group { Enemy, Ally, Neutral}
	public enum AnimTypes { idle, attack, move,
        prepare, move_up, move_down, move_left, move_right, attack_up, attack_down, attack_left, attack_right, kick
    }

    public enum AnimParam {agro}

    public enum CellEffect { drown, fall, toxicDrown, teleport,
        teleportArrival
    }
    public enum EffectType { waterSplash, explosion, toxicSplash, poison, stink, teleport, teleport2 }
    public enum DropType { none, apple, gold, door_key }

    public enum ItemType { chest_key = 1, door_key = 2, boss_key = 3}
    public enum InteractibleType { gateLock, kickable, poke }
    public enum ObjectEffectType { fall, restore_after_fall, teleport, teleportArrival }
    public enum CellHighlightType
    {
        Red,
        Yellow,
        Green
    };
}