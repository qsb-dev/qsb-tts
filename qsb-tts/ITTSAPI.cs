namespace QSBTTS;

public interface ITTSAPI
{
	void PlayTTS(string text, TTSVoice voice);

	public enum TTSVoice : uint
	{
		Paul,
		Betty,
		Harry,
		Frank,
		Dennis,
		Kit,
		Ursula,
		Rita,
		Wendy
	}
}