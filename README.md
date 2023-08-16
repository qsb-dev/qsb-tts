# QSB Text to Speech
This addon provides text-to-speech (TTS) functionality to QSB's text chat.

The (default) TTS voice is Perfect Paul from DECTalk / FonixTalk - most notably used as the famous Moonbase Alpha voice. John Madden.

Any message posted to QSB's text chat will be read out. This includes annoucements like "player has joined", but these will be read out in a different voice.

## API
You can use this addon in other QSB addons! I don't know why you would want to, but it was no extra work for me to set it up.

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
