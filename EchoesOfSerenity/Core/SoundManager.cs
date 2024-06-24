using Raylib_cs;

namespace EchoesOfSerenity.Core;

public static class SoundManager
{
    private static List<Sound> _playingSounds = new();

    public static void PlaySound(Sound sound)
    {
        Sound alias = Raylib.LoadSoundAlias(sound);
        Raylib.PlaySound(alias);
        _playingSounds.Add(alias);
    }

    public static void Update()
    {
        for (int i = _playingSounds.Count - 1; i >= 0; i--)
        {
            if (!Raylib.IsSoundPlaying(_playingSounds[i]))
            {
                Raylib.UnloadSoundAlias(_playingSounds[i]);
                _playingSounds.RemoveAt(i);
            }
        }
    }
}
