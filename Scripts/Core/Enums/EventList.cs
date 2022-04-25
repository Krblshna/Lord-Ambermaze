namespace LordAmbermaze.Core
{
	public static class EventList
	{
		public static string MakeMove => "MakeMove";
        public static string Death => "Death";
        public static string MoveStarted => "MoveStarted";
        public static string AllMovementsFinished => "MoveFinish";
        public static string ScreenHide => "ScreenHide";
        public static string ScreenShow => "ScreenShow";
        public static string OnScreenHided => "OnScreenHided";
        public static string OnScreenShowed => "OnScreenShowed";
        public static string PlayerMoveFinished => "PlayerMoveFinished";
        public static string TurnFinished => "TurnFinished";
        public static string GoldChanged => "gold:changed";
        public static string PlayerMaxHpChanged => "Player:MaxHp";
        public static string SceneLoaded => "SceneLoaded";
        public static string TeleportEnter => "TeleportEnter";
        public static string TeleportExit => "TeleportExit";
        public static string RoomCleared => "RoomCleared";
        public static string TeleportStart => "TeleportStart";
        public static string TeleportFinish => "TeleportFinish";
        public static string UnloadPrevScene => "UnloadPrevScene";
        public static string ForceUnloadPrevScene => "ForceUnloadPrevScene";
        public static string ManaChanged => "ManaChanged";
        public static string TargetCompleted => "TargetCompleted";
    }
}