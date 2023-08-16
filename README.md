# QSB Text to Speech
This addon provides text-to-speech (TTS) functionality to QSB's text chat.

The (default) TTS voice is Perfect Paul from DECTalk / FonixTalk - most notably used as the famous Moonbase Alpha voice. John Madden.

## API
You can use this addon in other QSB addons!

First, create this interface in your mod.

```
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
```

Then create and call the API like any other. For example :

```
var api = ModHelper.Interaction.TryGetModApi<ITTSAPI>("_nebula.QSBTTS");
api.PlayTTS("JOHN MADDEN", ITTSAPI.TTSVoice.Paul);
```

## Credits
- TTS voice generated with the [SharpTalk](https://github.com/whatsecretproject/SharpTalk) library.
