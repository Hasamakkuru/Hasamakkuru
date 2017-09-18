using UnityEngine.EventSystems;

public interface EventController : IEventSystemHandler
{
	void KilledPoint();
    void BonusPoint();
    void DeathPenalty();
}

